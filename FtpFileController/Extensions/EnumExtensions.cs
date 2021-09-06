using System;
using System.ComponentModel;

namespace FtpFileController.Extensions {
    public static class EnumExtensions {
        /// <summary>
        /// Возвращает описание перечисления по значению
        /// </summary>
        public static string GetDescription(this Enum value) {
            var fieldInfo = value.GetType().GetField(value.ToString());

            return Attribute.IsDefined(fieldInfo!, typeof(DescriptionAttribute))
                ? ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))?.Description ?? fieldInfo.Name
                : fieldInfo.Name;
        }
    }
}