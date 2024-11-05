using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// ����ע��ϵͳ������ѡ�
    /// </summary>
    public class SnkDIOptions : ISnkDIOptions
    {
        /// <summary>
        /// ��ʼ�� <see cref="SnkDIOptions"/> �����ʵ����������Ĭ��ֵ��
        /// </summary>
        public SnkDIOptions()
        {
            // Ĭ������£����Լ�ⵥ���������е�ѭ�����á�
            TryToDetectSingletonCircularReferences = true;

            // Ĭ������£����Լ�⶯̬�������е�ѭ�����á�
            TryToDetectDynamicCircularReferences = true;

            // Ĭ������£��������ע��ʧ�ܣ����鲢�����ͷŶ���
            CheckDisposeIfPropertyInjectionFails = true;

            // Ĭ������£�ʹ�� SnkPropertyInjector ��Ϊ����ע���������͡�
            PropertyInjectorType = typeof(SnkPropertyInjector);

            // Ĭ������£�ʹ���µ� SnkPropertyInjectorOptions ʵ����Ϊ����ע������ѡ�
            PropertyInjectorOptions = new SnkPropertyInjectorOptions();
        }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�Ƿ��Լ�ⵥ���������е�ѭ�����á�(Ĭ��ֵ��true)
        /// </summary>
        public bool TryToDetectSingletonCircularReferences { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�Ƿ��Լ�⶯̬�������е�ѭ�����á�(Ĭ��ֵ��true)
        /// </summary>
        public bool TryToDetectDynamicCircularReferences { get; set; }

        /// <summary>
        /// ��ȡ������һ��ֵ��ָʾ�������ע��ʧ�ܣ��Ƿ��鲢�����ͷŶ���(Ĭ��ֵ��true)
        /// </summary>
        public bool CheckDisposeIfPropertyInjectionFails { get; set; }

        /// <summary>
        /// ��ȡ������Ҫʹ�õ�����ע���������͡�(Ĭ��ֵ��<see cref="SnkDIOptions"/>)
        /// </summary>
        public Type PropertyInjectorType { get; set; }

        /// <summary>
        /// ��ȡ����������ע������ѡ�(Ĭ��ֵ��<see cref="SnkPropertyInjectorOptions"/>)
        /// </summary>
        public ISnkPropertyInjectorOptions PropertyInjectorOptions { get; set; }
    }
}