namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// ��������ע�����ӿڡ�
    /// </summary>
    public interface ISnkPropertyInjector
    {
        /// <summary>
        /// ��Ŀ�����ע�����ԡ�
        /// </summary>
        /// <param name="target">Ŀ�����ע�뽫Ӧ���ڸö�������ԡ�</param>
        /// <param name="options">��ѡ������ע����ѡ���������ע����Ϊ��</param>
        void Inject(object target, ISnkPropertyInjectorOptions options = null);
    }
}