using System;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// ��ʾһ������Ӧͨ������ע����и�ֵ�����ԡ�
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SnkInjectAttribute : Attribute
    {
    }
}