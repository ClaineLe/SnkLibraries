using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnkFramework.Base
{
    /// <summary>
    /// 提供对字典进行操作的扩展方法。
    /// </summary>
    public static class SnkDictionaryExtensions
    {
        /// <summary>
        /// 定义了用于反射获取属性的绑定标志。
        /// </summary>
        public static BindingFlags s_PropertyBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        /// <summary>
        /// 将一个对象的公共属性转换为字典。
        /// </summary>
        /// <param name="input">需要转换的对象。</param>
        /// <returns>包含对象公共属性的字典，键为属性名，值为属性值。</returns>
        public static IDictionary<string, object> ToPropertyDictionary(this object input)
        {
            // 如果输入对象为空，返回一个空字典
            if (input == null)
                return new Dictionary<string, object>();

            // 如果输入对象已经是字典，直接返回
            if (input is IDictionary<string, object> dict)
                return dict;

            var dictionary = new Dictionary<string, object>();

            // 获取对象的公共实例属性，并筛选出可读的属性
            var propertyInfos = input.GetType().GetProperties(s_PropertyBindingFlags).Where(p => p.CanRead);

            foreach (var propertyInfo in propertyInfos)
            {
                var value = propertyInfo.GetValue(input);
                if (value != null)
                    dictionary[propertyInfo.Name] = value;
            }

            return dictionary;
        }
    }
}