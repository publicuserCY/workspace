using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo4DotNetCore.ResourceServer.Model
{
    /// <summary>
    /// Model基类
    /// </summary>
    public abstract class BaseModel<T>
    {
        public BaseModel() { }

        public BaseModel(bool autoGenerateId)
        {
            if (Id is string)
            {
                Id = Convert<string, T>(Guid.NewGuid().ToString("D"));
            }
        }


        public virtual T Id { get; set; }

        /// <summary>
        /// 实体状态
        /// </summary>
        [NotMapped]
        public EntityState State { get; set; }

        private static U Convert<T, U>(T value)
        {
            if (value is U && typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable))
            {
                return (U)(object)value;
            }
            else
            {
                Type targetType = typeof(U);

                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (value == null)
                        return default(U);

                    targetType = Nullable.GetUnderlyingType(targetType);
                }

                return (U)System.Convert.ChangeType(value, targetType, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}