namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// ��������ע����ѡ��Ľӿڡ�
    /// �ṩ��������ע����Ϊ��ѡ�
    /// </summary>
    public interface ISnkPropertyInjectorOptions
    {
        /// <summary>
        /// ��ȡע�����ԵĲ��ԡ�
        /// </summary>
        SnkPropertyInjection InjectIntoProperties { get; }

        /// <summary>
        /// ��ȡһ��ֵ��ָʾ������ע��ʧ��ʱ�Ƿ�Ӧ���׳��쳣��
        /// </summary>
        bool ThrowIfPropertyInjectionFails { get; }
    }
}