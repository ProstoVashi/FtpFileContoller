using System;
using System.Linq;
using System.Reflection;

namespace FtpFileController.Extensions {
    public static class ReflectionExtensions {
        private static readonly Type ObjectType = typeof(Object);
        private static readonly BindingFlags PUBLIC_GETTER_FLAGS = BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance;

        private static readonly BindingFlags PUBLIC_SETTER_FLAGS = BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance;

        public static void UpdatePropertiesValue<TSource, TDestination>(this TDestination destination, TSource source) where TDestination : new() where TSource : new() {
            var setProperties = destination.GetType<TDestination>().GetProperties(PUBLIC_SETTER_FLAGS);
            var getPropertiesMap = source.GetType<TSource>().GetProperties(PUBLIC_GETTER_FLAGS)
                                              .ToDictionary(prop => prop.Name, prop => prop.GetValue(source));

            foreach (var setProperty in setProperties) {
                if (getPropertiesMap.TryGetValue(setProperty.Name, out var value)) {
                    setProperty.SetValue(destination, value);
                }
            }
        }

        public static Type GetType<T>(this T obj) {
            var type = typeof(T);

            return type == ObjectType
                ? obj.GetType()
                : type;
        }
    }
}