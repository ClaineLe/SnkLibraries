namespace SnkDependencyInjection
{
    /// <summary>
    /// ��ʾ����ע��ѡ����ࡣ
    /// </summary>
    public class SnkPropertyInjectorOptions : ISnkPropertyInjectorOptions
    {
        /// <summary>
        /// ��ʼ�� <see cref="SnkPropertyInjectorOptions"/> �����ʵ��������Ĭ��ѡ�
        /// </summary>
        public SnkPropertyInjectorOptions()
        {
            // Ĭ��Ϊ�������κ�����ע��
            InjectIntoProperties = SnkPropertyInjection.None;
            // Ĭ��Ϊ����ע��ʧ��ʱ���׳��쳣
            ThrowIfPropertyInjectionFails = false;
        }

        /// <summary>
        /// ��ȡ����������ע������͡�
        /// </summary>
        public SnkPropertyInjection InjectIntoProperties { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ����ֵָʾ�Ƿ�������ע��ʧ��ʱ�׳��쳣��
        /// </summary>
        public bool ThrowIfPropertyInjectionFails { get; set; }

        /// <summary>
        /// ��̬�ֶΣ����ڴ洢ע������ѡ���ʵ����
        /// </summary>
        private static ISnkPropertyInjectorOptions _injectProperties;

        /// <summary>
        /// ��ȡ����ע��ӿ����Ե�ѡ��ʵ����
        /// </summary>
        public static ISnkPropertyInjectorOptions Inject
        {
            get
            {
                // �����δ��ʼ�����򴴽�һ����ʵ��������Ϊע��ӿ�����
                _injectProperties = _injectProperties ?? new SnkPropertyInjectorOptions()
                {
                    InjectIntoProperties = SnkPropertyInjection.InjectInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _injectProperties;
            }
        }

        /// <summary>
        /// ��̬�ֶΣ����ڴ洢ȫ������ע��ѡ���ʵ����
        /// </summary>
        private static ISnkPropertyInjectorOptions _allProperties;

        /// <summary>
        /// ��ȡ����ע�����нӿ����Ե�ѡ��ʵ����
        /// </summary>
        public static ISnkPropertyInjectorOptions All
        {
            get
            {
                // �����δ��ʼ�����򴴽�һ����ʵ��������Ϊע�����нӿ�����
                _allProperties = _allProperties ?? new SnkPropertyInjectorOptions()
                {
                    InjectIntoProperties = SnkPropertyInjection.AllInterfaceProperties,
                    ThrowIfPropertyInjectionFails = false
                };
                return _allProperties;
            }
        }
    }
}