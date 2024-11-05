using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// һ����������������������࣬���������Ա�ǣ���ʾ��������ʱ�Ž���ĳЩ������������ע�룩��
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class SnkConditionalConventionalAttribute : Attribute
    {
        /// <summary>
        /// ��ȡһ������ֵ����ʾ��ǰ�����Ƿ����㡣
        /// �����Ա����ǳ���ģ��Ա�ÿ������ʵ�ֿ��Զ����Լ�����������߼���
        /// </summary>
        public abstract bool IsConditionSatisfied { get; }
    }
}