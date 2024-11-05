using System;
using System.Linq;
using System.Reflection;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// 扩展方法类，用于判断类型和属性是否符合约定的条件。
    /// </summary>
    public static class SnkConventionAttributeExtensions
    {
        /// <summary>
        /// 判断一个类型是否符合约定。类型符合约定当且仅当：
        /// - 未标记不常规属性
        /// - 所有标记的条件约定返回 true
        /// </summary>
        /// <param name="candidateType">待检查的类型。</param>
        /// <returns>如果类型符合约定，返回 true；否则返回 false。</returns>
        public static bool IsConventional(this Type candidateType)
        {
            var unconventionalAttributes = candidateType.GetCustomAttributes(
                typeof(SnkUnconventionalAttribute), true);

            if (unconventionalAttributes.Length > 0)
                return false;

            return candidateType.SatisfiesConditionalConventions();
        }

        /// <summary>
        /// 判断一个属性是否符合约定。属性符合约定当且仅当：
        /// - 未标记不常规属性
        /// - 所有标记的条件约定返回 true
        /// </summary>
        /// <param name="propertyInfo">待检查的属性信息。</param>
        /// <returns>如果属性符合约定，返回 true；否则返回 false。</returns>
        public static bool IsConventional(this PropertyInfo propertyInfo)
        {
            var unconventionalAttributes = propertyInfo.GetCustomAttributes(
                typeof(SnkUnconventionalAttribute), true);

            if (unconventionalAttributes.Any())
                return false;

            return propertyInfo.SatisfiesConditionalConventions();
        }

        /// <summary>
        /// 检查类型是否满足所有条件约定。
        /// </summary>
        /// <param name="candidateType">待检查的类型。</param>
        /// <returns>如果类型满足所有条件约定，返回 true；否则返回 false。</returns>
        public static bool SatisfiesConditionalConventions(this Type candidateType)
        {
            var conditionalAttributes =
                candidateType.GetCustomAttributes(typeof(SnkConditionalConventionalAttribute), true);

            return conditionalAttributes.Cast<SnkConditionalConventionalAttribute>()
                .All(attr => attr.IsConditionSatisfied);
        }

        /// <summary>
        /// 检查属性是否满足所有条件约定。
        /// </summary>
        /// <param name="propertyInfo">待检查的属性信息。</param>
        /// <returns>如果属性满足所有条件约定，返回 true；否则返回 false。</returns>
        public static bool SatisfiesConditionalConventions(this PropertyInfo propertyInfo)
        {
            var conditionalAttributes =
                propertyInfo.GetCustomAttributes(typeof(SnkConditionalConventionalAttribute), true);

            return conditionalAttributes.Cast<SnkConditionalConventionalAttribute>()
                .All(conditional => conditional.IsConditionSatisfied);
        }
    }
}