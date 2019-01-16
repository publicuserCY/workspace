namespace Demo4DotNetCore.ResourceServer.Model
{
    public enum Operational
    {
        Delete = -1,
        Origin = 0,
        Update = 1,
        Insert = 2
    }

    /// <summary>
    /// 查询基类
    /// </summary>
    public class BaseRequestModel
    {
        public Operational Flag { get; set; }

        /// <summary>
        /// 通用查询参数
        /// </summary>
        public string Criteria { get; set; }
    }
}