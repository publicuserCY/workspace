using System;
using System.Configuration;

namespace Demo4DotNetCore.Tools
{
    /// <summary>
    /// AppSetting工具
    /// </summary>
    public class AppSettingTool
    {
        /// <summary>
        /// 从AppSetting获取值
        /// </summary>
        /// <typeparam name="T">期待得到的类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>返回指定类型的结果</returns>
        public static T Value<T>(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? default(T) : Convert<string, T>(value);
        }

        /// <summary>
        /// 从AppSetting获取值
        /// </summary>
        /// <typeparam name="T">期待得到的类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回指定类型的结果。如果配置不存在，返回默认值。</returns>
        public static T Value<T>(string key, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? defaultValue : Convert<string, T>(value);
        }

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