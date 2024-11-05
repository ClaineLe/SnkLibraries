using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// ��������ע��ѡ��Ľӿڡ�
    /// </summary>
    public interface ISnkDIOptions
    {
        /// <summary>
        /// ��ȡ�Ƿ��ⵥ��ģʽ�µ�ѭ�����ã���ֹ��ѭ���������µ��ڴ�й©��������
        /// </summary>
        bool TryToDetectSingletonCircularReferences { get; }

        /// <summary>
        /// ��ȡ�Ƿ��⶯̬�����ѭ�����ã�ȷ����̬ע��Ķ��󲻻��γ�ѭ��������
        /// </summary>
        bool TryToDetectDynamicCircularReferences { get; }

        /// <summary>
        /// ��ȡ������ע��ʧ��ʱ���Ƿ��鲢�����ͷ��ѷ������Դ����ֹ��Դй©��
        /// </summary>
        bool CheckDisposeIfPropertyInjectionFails { get; }

        /// <summary>
        /// ��ȡָ����������ע���ע�������ͣ������Զ�������ע���߼���
        /// </summary>
        Type PropertyInjectorType { get; }

        /// <summary>
        /// ��ȡ����ע����������ѡ����ھ�ϸ����������ע�����Ϊ��
        /// </summary>
        ISnkPropertyInjectorOptions PropertyInjectorOptions { get; }
    }
}