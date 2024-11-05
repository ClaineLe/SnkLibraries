using System;
using System.Collections.Generic;
using System.Reflection;

namespace SnkFramework.DependencyInjection
{
    /// <summary>
    /// �������ͻ���ӿڣ����ڴ洢�Ͳ��ҳ����е�������Ϣ��
    /// </summary>
    public interface ISnkTypeCache
    {
        /// <summary>
        /// ��ȡ���ͻ�������Ϊ���͵�ȫ����Сд��ʽ��
        /// </summary>
        Dictionary<string, Type> LowerCaseFullNameCache { get; }

        /// <summary>
        /// ��ȡ���ͻ�������Ϊ���͵�ȫ����
        /// </summary>
        Dictionary<string, Type> FullNameCache { get; }

        /// <summary>
        /// ��ȡ���ͻ�������Ϊ���͵����ơ�
        /// </summary>
        Dictionary<string, Type> NameCache { get; }

        /// <summary>
        /// �򻺴�����ӳ����е��������͡�
        /// </summary>
        /// <param name="assembly">��Ҫ����ĳ��򼯡�</param>
        void AddAssembly(Assembly assembly);
    }
}