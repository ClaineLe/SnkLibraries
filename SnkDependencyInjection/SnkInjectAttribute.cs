using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// ��ʾһ������Ӧͨ������ע����и�ֵ�����ԡ�
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SnkInjectAttribute : Attribute
    {
    }
}