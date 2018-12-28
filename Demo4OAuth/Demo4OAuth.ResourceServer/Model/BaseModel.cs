using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Demo4OAuth.ResourceServer.Model
{
    /// <summary>
    /// Model基类
    /// </summary>
    public class BaseModel
    {
        public BaseModel() { }

        public BaseModel(bool autoGenerateId)
        {
            if (autoGenerateId) { Id = Guid.NewGuid().ToString("D"); }
        }

        /// <summary>
        /// 标识
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// 实体状态
        /// </summary>
        [NotMapped]
        public EntityState EntityState { get; set; }
    }
}