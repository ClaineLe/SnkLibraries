using System;
using System.Linq;
using System.Reflection;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// ��չ�����࣬�����ж����ͺ������Ƿ����Լ����������
    /// </summary>
    public static class SnkConventionAttributeExtensions
    {
        /// <summary>
        /// �ж�һ�������Ƿ����Լ�������ͷ���Լ�����ҽ�����
        /// - δ��ǲ���������
        /// - ���б�ǵ�����Լ������ true
        /// </summary>
        /// <param name="candidateType">���������͡�</param>
        /// <returns>������ͷ���Լ�������� true�����򷵻� false��</returns>
        public static bool IsConventional(this Type candidateType)
        {
            var unconventionalAttributes = candidateType.GetCustomAttributes(
                typeof(SnkUnconventionalAttribute), true);

            if (unconventionalAttributes.Length > 0)
                return false;

            return candidateType.SatisfiesConditionalConventions();
        }

        /// <summary>
        /// �ж�һ�������Ƿ����Լ�������Է���Լ�����ҽ�����
        /// - δ��ǲ���������
        /// - ���б�ǵ�����Լ������ true
        /// </summary>
        /// <param name="propertyInfo">������������Ϣ��</param>
        /// <returns>������Է���Լ�������� true�����򷵻� false��</returns>
        public static bool IsConventional(this PropertyInfo propertyInfo)
        {
            var unconventionalAttributes = propertyInfo.GetCustomAttributes(
                typeof(SnkUnconventionalAttribute), true);

            if (unconventionalAttributes.Any())
                return false;

            return propertyInfo.SatisfiesConditionalConventions();
        }

        /// <summary>
        /// ��������Ƿ�������������Լ����
        /// </summary>
        /// <param name="candidateType">���������͡�</param>
        /// <returns>�������������������Լ�������� true�����򷵻� false��</returns>
        public static bool SatisfiesConditionalConventions(this Type candidateType)
        {
            var conditionalAttributes =
                candidateType.GetCustomAttributes(typeof(SnkConditionalConventionalAttribute), true);

            return conditionalAttributes.Cast<SnkConditionalConventionalAttribute>()
                .All(attr => attr.IsConditionSatisfied);
        }

        /// <summary>
        /// ��������Ƿ�������������Լ����
        /// </summary>
        /// <param name="propertyInfo">������������Ϣ��</param>
        /// <returns>�������������������Լ�������� true�����򷵻� false��</returns>
        public static bool SatisfiesConditionalConventions(this PropertyInfo propertyInfo)
        {
            var conditionalAttributes =
                propertyInfo.GetCustomAttributes(typeof(SnkConditionalConventionalAttribute), true);

            return conditionalAttributes.Cast<SnkConditionalConventionalAttribute>()
                .All(conditional => conditional.IsConditionSatisfied);
        }
    }
}