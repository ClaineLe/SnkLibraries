using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// �����ӳٴ�������ʵ�����ࡣ
    /// </summary>
    public class SnkLazySingletonCreator
    {
        // ����ͬ���߳����Ķ���
        private readonly object _locker = new object();

        // ��Ҫ��������ʵ��������
        private readonly Type _type;

        // ���浥��ʵ�����ֶ�
        private object _instance;

        /// <summary>
        /// ��ȡ����ʵ�������ʵ����δ���������ӳٴ�������
        /// </summary>
        public object Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                // ȷ���̰߳�ȫ��ʵ����
                lock (_locker)
                {
                    // ���_instanceΪnull�����������ע���ṩ�Ĺ��췽������ʵ��
                    _instance = _instance ?? SnkDIProvider.Instance.DIConstruct(_type);
                    return _instance;
                }
            }
        }

        /// <summary>
        /// ��ʼ�� <see cref="SnkLazySingletonCreator"/> �����ʵ����
        /// </summary>
        /// <param name="type">��Ҫ��������ʵ�������͡�</param>
        public SnkLazySingletonCreator(Type type)
        {
            _type = type;
        }
    }
}