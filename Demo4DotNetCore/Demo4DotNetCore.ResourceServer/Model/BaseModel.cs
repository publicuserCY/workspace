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
        public BaseModel() { State = EntityState.Unchanged; }

        public BaseModel(bool autoGenerateId) : base()
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

        private static D Convert<S, D>(S value)
        {
            if (value is D && typeof(D) != typeof(IComparable) && typeof(D) != typeof(IFormattable))
            {
                return (D)(object)value;
            }
            else
            {
                Type targetType = typeof(D);

                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (value == null)
                        return default(D);

                    targetType = Nullable.GetUnderlyingType(targetType);
                }

                return (D)System.Convert.ChangeType(value, targetType, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
}