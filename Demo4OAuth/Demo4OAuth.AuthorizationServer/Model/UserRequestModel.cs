namespace Demo4OAuth.AuthorizationServer.Model
{
    /// <summary>
    /// 查询基类
    /// </summary>
    public class UserRequestModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}