using System;
using Newtonsoft.Json;

namespace Demo4OAuth.Tools
{
    public class OperationResult
    {
        [JsonConstructor]
        public OperationResult(bool isSuccess)
        {
            Date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            IsSuccess = isSuccess;
        }

        public OperationResult(bool isSuccess, string code)
            : this(isSuccess)
        {
            Code = code;
        }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 操作相关信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回操作结果的时间
        /// </summary>
        public DateTime Date { get; private set; }

        private string code;
        /// <summary>
        /// 操作相关代码
        /// </summary>
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                var tool = new JsonConifgManager();
                Message = tool.Read(code);
            }
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult(bool isSuccess) : base(isSuccess) { }
        public OperationResult(bool isSuccess, string code) : base(isSuccess, code) { }
        public OperationResult(string message) : base(false) { Message = message; }
        [JsonConstructor]
        public OperationResult(bool isSuccess, string code, T entity) : base(isSuccess, code) { Data = entity; }
        public T Data { get; set; }
    }
}