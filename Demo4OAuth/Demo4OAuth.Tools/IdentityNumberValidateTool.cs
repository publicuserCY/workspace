using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Demo4OAuth.Tools
{
    public class IdentityNumberValidateTool
    {
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="identityNumber">身份证号码</param>
        /// <param name="realName">姓名(需utf8编码的urlencode)</param>
        /// <returns></returns>
        public Task<IndentityNumberValidateResult> ValidateIdentityNumber(string identityNumber, string realName)
        {
            IndentityNumberValidateResult result = new IndentityNumberValidateResult();
            string validate = AppSettingTool.Value<string>(Model.Core.Constants.Key_Validate_IndentityNumber);

            var requestUri = string.Format("http://apis.juhe.cn/idcard/index?key=1fc718e27ee6c14df71ab3740fad4dc9&cardno={0}", identityNumber);
            
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(requestUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonTask = response.Content.ReadAsStringAsync().Result;
                    JObject jsonObject = (JObject)JsonConvert.DeserializeObject(jsonTask);

                   
                    result.Sex = jsonObject["result"]["sex"].ToString();
                    result.Address = jsonObject["result"]["area"].ToString();
                    result.Birthday = jsonObject["result"]["birthday"].ToString();                   

                    if ((int)jsonObject["error_code"] == 0)
                    {
                        if (validate.ToLower() == "true")
                        {
                            var validateResult = RealNameAuthentication(identityNumber, realName).Result;

                            JObject jo = JObject.Parse(validateResult);
                            var errMsg= jo.Value<string>("ErrorMsg");
                            var res = jo.Value<string>("Res");

                            if (!string.IsNullOrWhiteSpace(errMsg))
                            {
                                result.ErrorMsg = errMsg;
                            }
                            else
                            {
                                if (Convert.ToInt32(res) != 1) // "res" 1：匹配 2：不匹配
                                {
                                    result.ErrorMsg = "姓名与身份证号码不匹配";
                                }
                            }
                        }                           
                    }
                    else
                    {
                        result.ErrorMsg = jsonObject["reason"].ToString();
                    }
                }
                else
                {
                    throw new HttpRequestException(string.Format("验证失败，状态码：", response.StatusCode));
                }
                
            }
            return Task.FromResult(result);
        }

        /// <summary>
        /// 身份证实名认证
        /// </summary>
        /// <param name="identityNumber">身份证号码</param>
        /// <param name="realName">姓名(需utf8编码的urlencode)</param>
        /// <returns></returns>
        private Task<string> RealNameAuthentication(string identityNumber, string realName)
        {
            string baseUri = "http://op.juhe.cn/idcard/query";
            string key = AppSettingTool.Value<string>("Validate.JuHeIndentityNumber.ApiKey");
            string requestUri = string.Format("{0}?key={1}&idcard={2}&realname={3}",baseUri,key,identityNumber,realName);
            string res = "";
            string errorMsg = "";

            using (var client = new HttpClient())
            {
                var response = client.GetAsync(requestUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonTask = response.Content.ReadAsStringAsync().Result;
                    JObject jsonObject = (JObject)JsonConvert.DeserializeObject(jsonTask);

                    if (!string.IsNullOrEmpty(jsonObject["result"].ToString()))
                    {
                        res = jsonObject["result"]["res"].ToString(); // "res" 1：匹配 2：不匹配
                    }
                    else
                    {
                        errorMsg = jsonObject["reason"].ToString();
                    }
                }
                else
                {
                    throw new HttpRequestException(string.Format("验证失败，状态码：", response.StatusCode));
                }
            }

            string content = JsonConvert.SerializeObject(new
            {
                ErrorMsg = errorMsg,
                Res = res
            }, Formatting.None);

            return Task.FromResult(content);
        } 

    }

    public class IndentityNumberValidateResult
    {
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 出身年月日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
