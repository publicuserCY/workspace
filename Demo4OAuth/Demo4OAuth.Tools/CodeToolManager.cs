
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo4OAuth.Tools
{
    /// <summary>
    /// 验证码生成工具
    /// </summary>
    public class CodeToolManager
    {
        private ICodeTool codeTool;
        public CodeToolManager(CodeType codeType)
        {
            switch (codeType)
            {
                case CodeType.Graphic:
                    codeTool = new GraphicCodeTool();
                    break;
                case CodeType.Email:
                    codeTool = new EmailCodeTool();
                    break;
                default:
                    codeTool = new PhoneCodeTool();
                    break;
            }
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="action">验证码用途</param>
        public Task<Model.Core.OperationResult<object>> SendCode(string key, string action)
        {
            return codeTool.SendCode(key, action);
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="code">验证码</param>
        /// <returns>true:验证通过;false:验证失败</returns>
        public Task<Model.Core.OperationResult> ValidateCode(string key, string code)
        {
            return codeTool.ValidateCode(key, code);
        }
    }

    internal interface ICodeTool
    {
        Task<Model.Core.OperationResult<object>> SendCode(string key, string action);
        Task<Model.Core.OperationResult> ValidateCode(string key, string code);
    }

    internal class EmailCodeTool : ICodeTool
    {
        private const string VALIDATE_CODE_DIRECTORY = "ValidateCodeDir";
        private string dir;
        private string path;
        private int expireDuration;
        private int sendInteval;

        public EmailCodeTool()
        {
            dir = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Model.Core.Constants.RUNTIME_DIRECTORY + "/" + VALIDATE_CODE_DIRECTORY);
            path = dir + "\\email_code.xml";
            //从配置中读取[验证码发送间隔]
            sendInteval = AppSettingTool.Value(Model.Core.Constants.Key_Api_SendCodeInteval, Model.Core.Constants.Default_Api_SendCodeInteval);
            //从配置中读取[验证码失效间隔]
            expireDuration = AppSettingTool.Value(Model.Core.Constants.Key_Api_CodeExpireDuration, Model.Core.Constants.Default_Api_CodeExpireDuration);
        }

        //internal ApplicationUserManager UserManager { get; private set; }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="key">Email</param>
        /// <param name="action">验证码用途</param>
        /// <returns></returns>
        public Task<Model.Core.OperationResult<object>> SendCode(string key, string action)
        {
            var opResult = new Model.Core.OperationResult<object>(true, Core.Constants.Ok);
            var model = GetCode(key);
            if (model != null && model.Send.AddMinutes(sendInteval) > DateTime.Now)
            {
                opResult = new Model.Core.OperationResult<object>(false, Model.Core.Constants.Error_FrequencyTooFast);
                return Task.FromResult(opResult);
            }

            string subject = string.Empty;
            string appName = AppSettingTool.Value<string>(Model.Core.Constants.Key_Vender_WebSiteName);
            string venderName = AppSettingTool.Value<string>(Model.Core.Constants.Key_Vender_Name);
            string content = string.Empty;
            IValidateCode vc = new SimpleValidateCode();
            string code = vc.GenerateCode(4);
            opResult.Data = code;
            switch (action)
            {
                case "ChangeEmail":
                    subject = "修改邮箱";
                    content = AppSettingTool.Value(Model.Core.Constants.Key_Template_Email_ChangeEmail, Model.Core.Constants.Default_Template_Email_ChangeEmail);
                    break;
                case "GenerateEmailToken":
                    subject = "生成邮箱验证码";
                    content = AppSettingTool.Value(Model.Core.Constants.Key_Template_Email_GenerateEmailToken, Model.Core.Constants.Default_Template_Email_GenerateEmailToken);
                    break;
                case "RegisterByEmail":
                    subject = "确认您的电子邮箱地址";
                    content = AppSettingTool.Value(Model.Core.Constants.Key_Template_Email_GenerateEmailConfirmation, Model.Core.Constants.Default_Template_Email_GenerateEmailConfirmation);
                    break;
                case "ResetPassword":
                    subject = "重置密码";
                    content = AppSettingTool.Value(Model.Core.Constants.Key_Template_Email_ResetPassword, Model.Core.Constants.Default_Template_Email_ResetPassword);
                    break;
                default:
                    break;
            }

            var message = new IdentityMessage
            {
                Subject = subject,
                Body = string.Format(content, code),
                Destination = key
            };
            new EmailService().SendAsync(message).Wait();
            SetCode(key, code);
            return Task.FromResult(opResult);
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="key">Email</param>
        /// <param name="code">验证码</param>
        /// <returns>true:验证通过;false:验证失败</returns>
        public Task<Model.Core.OperationResult> ValidateCode(string key, string code)
        {
            var opResult = new Model.Core.OperationResult(true, Core.Constants.Ok);
            ValidateCodeViewModel model = GetCode(key);
            if (model == null || model.Code != code)
            {
                opResult = new Model.Core.OperationResult(false, Model.Core.Constants.Error_InvalidCode);
            }
            else if (model.Expire < DateTime.Now)
            {
                opResult = new Model.Core.OperationResult(false, Model.Core.Constants.Error_ExpiredCode);
            }
            return Task.FromResult(opResult);
        }

        /// <summary>
        /// 保存验证码
        /// </summary>
        /// <param name="key">邮件</param>
        /// <param name="code">验证码</param>
        /// <param name="expire">失效时间</param>
        private void SetCode(string key, string code)
        {
            XDocument doc = null;
            if (File.Exists(path))
            {
                doc = XDocument.Load(path);
            }
            else
            {
                doc = new XDocument(new XElement("root"));
                var filesDir = new DirectoryInfo(dir);
                if (!filesDir.Exists) { filesDir.Create(); }
            }
            var elements = doc.Root.Descendants("item").Where(p => p.Attribute("key").Value == key || DateTime.Parse(p.Attribute("expire").Value, null, DateTimeStyles.RoundtripKind) < DateTime.Now);
            for (int i = elements.Count() - 1; i >= 0; i--)
            {
                elements.Last().Remove();
            }
            var element = new XElement("item",
                new XAttribute("key", key),
                new XAttribute("code", code),
                new XAttribute("send", DateTime.Now),
                new XAttribute("expire", DateTime.Now.AddMinutes(expireDuration))
            );
            doc.Root.Add(element);
            doc.Save(path);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="key">邮件</param>
        /// <returns>ValidateCodeDto</returns>
        private ValidateCodeViewModel GetCode(string key)
        {
            if (!File.Exists(path)) { return null; }
            XDocument doc = XDocument.Load(path);
            var element = doc.Root.Descendants("item").FirstOrDefault(p => p.Attribute("key").Value == key && DateTime.Parse(p.Attribute("expire").Value, null, DateTimeStyles.RoundtripKind) >= DateTime.Now);
            if (element == null) { return null; }
            var result = new ValidateCodeViewModel()
            {
                Key = key,
                Code = element.Attribute("code").Value,
                Send = DateTime.Parse(element.Attribute("send").Value, null, DateTimeStyles.RoundtripKind),
                Expire = DateTime.Parse(element.Attribute("expire").Value, null, DateTimeStyles.RoundtripKind)
            };
            return result;
        }
    }

    internal class GraphicCodeTool : ICodeTool
    {
        private const string VALIDATE_CODE_DIRECTORY = "ValidateCodeDir";
        private string dir;
        private string path;
        private int expireDuration;

        public GraphicCodeTool()
        {
            dir = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Model.Core.Constants.RUNTIME_DIRECTORY + "/" + VALIDATE_CODE_DIRECTORY);
            path = dir + "\\graphic_code.xml";
            //从配置中读取[验证码失效间隔]
            expireDuration = AppSettingTool.Value(Model.Core.Constants.Key_Api_CodeExpireDuration, Model.Core.Constants.Default_Api_CodeExpireDuration);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="key">account</param>
        /// <param name="action">验证码用途</param>
        /// <returns></returns>
        public Task<Model.Core.OperationResult<object>> SendCode(string key, string action)
        {
            var opResult = new Model.Core.OperationResult<object>(true, Core.Constants.Ok);
            IValidateCode vc = new SimpleValidateCode();
            string code = vc.GenerateCode(4);
            opResult.Data = vc.GenerateGraphic(code);
            SetCode(key, code);
            return Task.FromResult(opResult);
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="key">ClientId</param>
        /// <param name="code">验证码</param>
        /// <returns>bool</returns>
        public Task<Model.Core.OperationResult> ValidateCode(string key, string code)
        {
            var opResult = new Model.Core.OperationResult(true, Core.Constants.Ok);
            ValidateCodeViewModel dto = GetCode(key);
            if (dto == null || dto.Code != code)
            {
                opResult = new Model.Core.OperationResult(false, Model.Core.Constants.Error_InvalidCode);
            }
            else if (dto.Expire < DateTime.Now)
            {
                opResult = new Model.Core.OperationResult(false, Model.Core.Constants.Error_ExpiredCode);
            }
            return Task.FromResult(opResult);
        }

        /// <summary>
        /// 保存验证码
        /// </summary>
        /// <param name="key">ClientId</param>
        /// <param name="code">验证码</param>
        /// <param name="expire">失效时间</param>
        private void SetCode(string key, string code)
        {
            XDocument doc = null;
            if (File.Exists(path))
            {
                doc = XDocument.Load(path);
            }
            else
            {
                doc = new XDocument(new XElement("root"));
                var filesDir = new DirectoryInfo(dir);
                if (!filesDir.Exists) { filesDir.Create(); }
            }
            var elements = doc.Root.Descendants("item").Where(p => p.Attribute("key").Value == key || DateTime.Parse(p.Attribute("expire").Value, null, DateTimeStyles.RoundtripKind) < DateTime.Now);
            for (int i = elements.Count() - 1; i >= 0; i--)
            {
                elements.Last().Remove();
            }
            var element = new XElement("item",
                new XAttribute("key", key),
                new XAttribute("code", code),
                new XAttribute("expire", DateTime.Now.AddMinutes(expireDuration))
            );
            doc.Root.Add(element);
            doc.Save(path);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="key">ClientId</param>
        /// <returns>ValidateCodeDto</returns>
        private ValidateCodeViewModel GetCode(string key)
        {
            if (!File.Exists(path)) { return null; }
            XDocument doc = XDocument.Load(path);
            var element = doc.Root.Descendants("item").FirstOrDefault(p => p.Attribute("key").Value == key && DateTime.Parse(p.Attribute("expire").Value, null, DateTimeStyles.RoundtripKind) >= DateTime.Now);
            if (element == null) { return null; }
            var result = new ValidateCodeViewModel()
            {
                Key = key,
                Code = element.Attribute("code").Value,
                Expire = DateTime.Parse(element.Attribute("expire").Value, null, DateTimeStyles.RoundtripKind)
            };
            return result;
        }
    }

    internal class PhoneCodeTool : ICodeTool
    {
        private const string VALIDATE_CODE_DIRECTORY = "ValidateCodeDir";
        private string dir;
        private string path;
        private int expireDuration;
        private int sendInteval;

        public PhoneCodeTool()
        {
            dir = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Model.Core.Constants.RUNTIME_DIRECTORY + "/" + VALIDATE_CODE_DIRECTORY);
            path = dir + "\\phone_code.xml";
            //从配置中读取[验证码发送间隔]
            sendInteval = AppSettingTool.Value(Model.Core.Constants.Key_Api_SendCodeInteval, Model.Core.Constants.Default_Api_SendCodeInteval);
            //从配置中读取[验证码失效间隔]
            expireDuration = AppSettingTool.Value(Model.Core.Constants.Key_Api_CodeExpireDuration, Model.Core.Constants.Default_Api_CodeExpireDuration);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="key">手机号码</param>
        /// <param name="action">验证码用途</param>
        /// <returns></returns>
        public Task<Model.Core.OperationResult<object>> SendCode(string key, string action)
        {
            var opResult = new Model.Core.OperationResult<object>(true, Core.Constants.Ok);
            var dto = GetCode(key);
            if (dto != null && dto.Send.AddMinutes(sendInteval) > DateTime.Now)
            {
                opResult = new Model.Core.OperationResult<object>(false, Model.Core.Constants.Error_FrequencyTooFast);
                return Task.FromResult(opResult);
            }

            string subject = string.Empty;
            string appName = AppSettingTool.Value<string>(Model.Core.Constants.Key_Vender_WebSiteName);
            string venderName = AppSettingTool.Value<string>(Model.Core.Constants.Key_Vender_Name);
            IValidateCode vc = new SimpleValidateCode();
            string code = vc.GenerateCode(4);
            opResult.Data = code;
            switch (action)
            {
                case "ChangePhoneNumber":
                    subject = "更改手机号";
                    break;
                case "GeneratePhoneNumberToken":
                    subject = "生成手机验证码";
                    break;
                case "RegisterByPhoneNumber":
                    subject = "手机注册";
                    break;
                case "ResetPassword":
                    subject = "重置密码";
                    break;
                default:
                    break;
            }
            string content = JsonConvert.SerializeObject(new
            {
                Action = action,
                Code = code,
                App = appName,
                Company = venderName
            }, Formatting.None);
            var message = new IdentityMessage
            {
                Subject = subject,
                Body = content,
                Destination = key
            };
            new SmsService().SendAsync(message).Wait();
            SetCode(key, code);
            return Task.FromResult(opResult);
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="key">手机号码</param>
        /// <param name="code">验证码</param>
        /// <returns>true:验证通过;false:验证失败</returns>
        public Task<Model.Core.OperationResult> ValidateCode(string key, string code)
        {
            var opResult = new Model.Core.OperationResult(true, Core.Constants.Ok);
            ValidateCodeViewModel model = GetCode(key);
            if (model == null || model.Code != code)
            {
                opResult = new Model.Core.OperationResult(false, Model.Core.Constants.Error_InvalidCode);
            }
            else if (model.Expire < DateTime.Now)
            {
                opResult = new Model.Core.OperationResult(false, Model.Core.Constants.Error_ExpiredCode);
            }
            return Task.FromResult(opResult);
        }

        /// <summary>
        /// 保存验证码
        /// </summary>
        /// <param name="key">手机号码</param>
        /// <param name="code">验证码</param>
        /// <param name="expire">失效时间</param>
        private void SetCode(string key, string code)
        {
            XDocument doc = null;
            if (File.Exists(path))
            {
                doc = XDocument.Load(path);
            }
            else
            {
                doc = new XDocument(new XElement("root"));
                var filesDir = new DirectoryInfo(dir);
                if (!filesDir.Exists) { filesDir.Create(); }
            }
            var elements = doc.Root.Descendants("item").Where(p => p.Attribute("key").Value == key || DateTime.Parse(p.Attribute("expire").Value, null, DateTimeStyles.RoundtripKind) < DateTime.Now);
            for (int i = elements.Count() - 1; i >= 0; i--)
            {
                elements.Last().Remove();
            }
            var element = new XElement("item",
                new XAttribute("key", key),
                new XAttribute("code", code),
                new XAttribute("send", DateTime.Now),
                new XAttribute("expire", DateTime.Now.AddMinutes(expireDuration))
            );
            doc.Root.Add(element);
            doc.Save(path);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="key">手机号码</param>
        /// <returns>ValidateCodeDto</returns>
        private ValidateCodeViewModel GetCode(string key)
        {
            if (!File.Exists(path)) { return null; }
            XDocument doc = XDocument.Load(path);
            //var element = doc.Root.Descendants("item").FirstOrDefault(p => p.Attribute("key").Value == key && DateTime.Parse(p.Attribute("expire").Value, null, DateTimeStyles.RoundtripKind) >= DateTime.Now);
            var element = doc.Root.Descendants("item").FirstOrDefault(p => p.Attribute("key").Value == key);
            if (element == null) { return null; }
            var result = new ValidateCodeViewModel()
            {
                Key = key,
                Code = element.Attribute("code").Value,
                Send = DateTime.Parse(element.Attribute("send").Value, null, DateTimeStyles.RoundtripKind),
                Expire = DateTime.Parse(element.Attribute("expire").Value, null, DateTimeStyles.RoundtripKind)
            };
            return result;
        }
    }

    public enum CodeType { Graphic, PhoneNumber, Email }
}