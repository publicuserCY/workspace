using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo4DotNetCore.AuthorizationServer.Model
{
    /// <summary>
    /// Model基类
    /// </summary>
    public class BaseModel
    {
        public BaseModel() { }

        /// <summary>
        /// 实体状态
        /// </summary>
        [NotMapped]
        public EntityState State { get; set; }
    }
}