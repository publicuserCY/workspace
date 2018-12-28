using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo4OAuth.Tools
{
    public class EmailService : IIdentityMessageService
    {
        private NLog.Logger Log;

        public Task SendAsync(IdentityMessage message)
        {
            Log = NLog.LogManager.GetLogger(GetType().FullName);
            Log.Debug("目的:{0} 内容:{1} 邮箱:{2}", message.Subject, message.Body, message.Destination);
            string smsProvider = AppSettingTool.Value<string>(Model.Core.Constants.Key_Email_Provider);
            switch (smsProvider)
            {
                case "Configure":
                    return SendFromConfig(message);
                default:
                    return Task.FromResult(0);
            }
        }

        private Task SendFromConfig(IdentityMessage message)
        {
            var credentialUserName = AppSettingTool.Value<string>(Model.Core.Constants.Key_Email_CredentialUserName);
            var sentFrom = AppSettingTool.Value<string>(Model.Core.Constants.Key_Email_SentFrom);
            var pwd = AppSettingTool.Value<string>(Model.Core.Constants.Key_Email_Password);
            var host = AppSettingTool.Value<string>(Model.Core.Constants.Key_Email_Host);
            var port = AppSettingTool.Value<string>(Model.Core.Constants.Key_Email_Port);
            var enableSsl = AppSettingTool.Value<string>(Model.Core.Constants.Key_Email_EnableSsl);

            // Configure the client:
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(host);

            client.Port = int.Parse(port);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.EnableSsl = bool.Parse(enableSsl);
            client.UseDefaultCredentials = false;
            // Create the credentials:
            client.Credentials = new System.Net.NetworkCredential(credentialUserName, pwd);
            // Create the message:
            var from = new System.Net.Mail.MailAddress(sentFrom);
            var to = new System.Net.Mail.MailAddress(message.Destination);
            var mail = new System.Net.Mail.MailMessage(from, to);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;
            mail.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnFailure;
            // Send:
            return client.SendMailAsync(mail);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        private NLog.Logger Log;

        public SmsService()
        {
            Log = NLog.LogManager.GetLogger(GetType().FullName);
        }

        public Task SendAsync(IdentityMessage message)
        {
            Log.Debug("目的:{0} 内容:{1} 号码:{2}", message.Subject, message.Body, message.Destination);
            string smsProvider = AppSettingTool.Value<string>(Model.Core.Constants.Key_SMS_Provider);
            switch (smsProvider)
            {
                case "YunPian":
                    return SendFromYunPian(message);
                case "ZHHF":
                    return SendFromZHHF(message);
                case "haotingyun":
                    return SendFromHaoTingYun(message);
                default:
                    return Task.FromResult(0);
            }
        }

        /// <summary>
        /// 发送sms信息，服务提供商：云片网
        /// 模板地址 http://www.yunpian.com/api/tpl.html
        /// tpl_id = 5【#company#】感谢您注册#app#，您的验证码是#code#
        /// tpl_id = 6【#company#】欢迎使用#app#，您的手机验证码是#code#。本条信息无需回复
        /// tpl_id = 7【#company#】正在找回密码，您的验证码是#code#
        /// </summary>
        /// <param name="message">IdentityMessage</param>
        /// <returns>Task</returns>
        private Task SendFromYunPian(IdentityMessage message)
        {
            string apikey = AppSettingTool.Value<string>("SMS.YunPian.ApiKey");
            string baseAddress = "http://yunpian.com/v1/";
            string send_sms_template = "sms/tpl_send.json";
            JObject jo = JObject.Parse(message.Body);
            Model.Enums.ValidateOperation operation = (Model.Enums.ValidateOperation)Enum.Parse(typeof(Model.Enums.ValidateOperation), jo.Value<string>("Action"));
            string code = Uri.EscapeDataString(jo.Value<string>("Code"));
            string app = Uri.EscapeDataString(jo.Value<string>("App"));
            string company = Uri.EscapeDataString(jo.Value<string>("Company"));
            string tpl_id = null, encodedTplValue = null;
            switch (operation)
            {
                case Model.Enums.ValidateOperation.RegisterByPhoneNumber:
                    tpl_id = "5";//#app#=云片短信平台&#code#=1234&#company#=云片网
                    encodedTplValue = string.Format("#app#={0}&#code#={1}&#company#={2}", app, code, company);
                    break;
                case Model.Enums.ValidateOperation.GeneratePhoneNumberToken:
                    tpl_id = "6";//#app#=云片短信平台&#code#=1234&#company#=云片网
                    encodedTplValue = string.Format("#app#={0}&#code#={1}&#company#={2}", app, code, company);
                    break;
                case Model.Enums.ValidateOperation.ChangePhoneNumber:
                    tpl_id = "6";//#app#=云片短信平台&#code#=1234&#company#=云片网
                    encodedTplValue = string.Format("#app#={0}&#code#={1}&#company#={2}", app, code, company);
                    break;
                case Model.Enums.ValidateOperation.ResetPassword:
                    tpl_id = "7";//#code#=1234&#company#=云片网
                    encodedTplValue = string.Format("#code#={0}&#company#={1}", code, company);
                    break;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("apikey",apikey),
                    new KeyValuePair<string,string>("tpl_id",tpl_id),
                    new KeyValuePair<string,string>("tpl_value",encodedTplValue),
                    new KeyValuePair<string,string>("mobile",message.Destination)
                });
                var task = client.PostAsync(send_sms_template, content);
                if (task.Result.IsSuccessStatusCode)
                {
                    var jsonTask = task.Result.Content.ReadAsStringAsync();
                    JObject jsonObject = (JObject)JsonConvert.DeserializeObject(jsonTask.Result);
                    if ((int)jsonObject["code"] != 0)
                    {
                        throw new HttpRequestException(jsonObject["msg"].ToString() + jsonObject["detail"].ToString());
                    }
                }
                else
                {
                    throw new HttpRequestException(string.Format("发送失败，状态码：", task.Result.StatusCode));
                }
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 中化化肥的短信接口
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private Task SendFromZHHF(IdentityMessage message)
        {
            string baseUri = AppSettingTool.Value<string>("SMS.ZHHF.baseUri");
            string requestUri = AppSettingTool.Value<string>("SMS.ZHHF.requestUri");
            string cdkey = AppSettingTool.Value<string>("SMS.ZHHF.cdkey");
            string password = AppSettingTool.Value<string>("SMS.ZHHF.password");
            string msg = string.Empty;
            JObject jo = JObject.Parse(message.Body);
            Enums.ValidateOperation operation = (Model.Enums.ValidateOperation)Enum.Parse(typeof(Model.Enums.ValidateOperation), jo.Value<string>("Action"));
            string code = jo.Value<string>("Code");
            string app = jo.Value<string>("App");
            string company = jo.Value<string>("Company");
            switch (operation)
            {
                case Model.Enums.ValidateOperation.RegisterByPhoneNumber:
                    msg = "感谢您注册" + app + ",验证码为" + code;
                    break;
                default:
                    msg = "欢迎使用" + app + ",验证码为" + code;
                    break;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("cdkey",cdkey),
                    new KeyValuePair<string,string>("password",password),
                    new KeyValuePair<string,string>("phone",message.Destination),
                    new KeyValuePair<string,string>("message",msg)
                });
                var response = client.PostAsync(requestUri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonTask = response.Content.ReadAsStringAsync().Result;
                    Log.Debug("发送结束，返回值：" + jsonTask);
                }
                else
                {
                    Log.Debug("网络错误，状态码：" + response.StatusCode);
                    throw new HttpRequestException(string.Format("发送失败，状态码：", response.StatusCode));
                }
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 豪庭云
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private Task SendFromHaoTingYun(IdentityMessage message) {
            string apikey = AppSettingTool.Value<string>("SMS.YunPian.ApiKey");
            string baseAddress = "http://sms.haotingyun.com/v2/";
            string send_sms_template = "sms/tpl_single_send.json";
            JObject jo = JObject.Parse(message.Body);
            Model.Enums.ValidateOperation operation = (Model.Enums.ValidateOperation)Enum.Parse(typeof(Model.Enums.ValidateOperation), jo.Value<string>("Action"));
            string code = Uri.EscapeDataString(jo.Value<string>("Code"));
            string app = Uri.EscapeDataString(jo.Value<string>("App"));
            string company = Uri.EscapeDataString(jo.Value<string>("Company"));
            string tpl_id = null, encodedTplValue = null;
            switch (operation)
            {
                case Model.Enums.ValidateOperation.GeneratePhoneNumberToken:
                    tpl_id = "1623452";
                    encodedTplValue = string.Format("#code#={0}&#app#={1}",  code, company);
                    break;
              
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("apikey",apikey),
                    new KeyValuePair<string,string>("tpl_id",tpl_id),
                    new KeyValuePair<string,string>("tpl_value",encodedTplValue),
                    new KeyValuePair<string,string>("mobile",message.Destination)
                });
              
                var response = client.PostAsync(send_sms_template, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonTask = response.Content.ReadAsStringAsync().Result;
                    Log.Debug("发送结束，返回值：" + jsonTask);
                }
                else
                {
                    Log.Debug("网络错误，状态码：" + response.StatusCode);
                    throw new HttpRequestException(string.Format("发送失败，状态码：", response.StatusCode));
                }
            }
            return Task.FromResult<object>(null);
        }
    }

    public class ApplicationUserManager : UserManager<Model.Identity.ApplicationUser, string>
    {
        public ApplicationUserManager(IUserStore<Model.Identity.ApplicationUser, string> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<Model.Identity.ApplicationUser, Model.Identity.ApplicationRole, string, IdentityUserLogin, Model.Identity.ApplicationUserRole, IdentityUserClaim>(context.Get<Core.TPCISContext>()));
            // 配置用户名的验证逻辑
            manager.UserValidator = new UserValidator<Model.Identity.ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // 配置密码的验证逻辑
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 3,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // 配置用户锁定默认值
            manager.UserLockoutEnabledByDefault = false;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并将其插入到此处。
            manager.RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<Model.Identity.ApplicationUser>
            {
                MessageFormat = "你的安全代码是 {0}"
            });
            manager.RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<Model.Identity.ApplicationUser>
            {
                Subject = "安全代码",
                BodyFormat = "你的安全代码是 {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<Model.Identity.ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<Model.Identity.ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(Model.Identity.ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
