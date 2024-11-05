using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnkFramework.Logging;

namespace SnkDependencyInjection
{
    /// <summary>
    /// ����ע������ʵ�֡�
    /// </summary>
    public class SnkPropertyInjector : ISnkPropertyInjector
    {
        /// <summary>
        /// ��ȡ�����ÿ�ע�����Եİ󶨱�־��Ĭ��ֵΪ����ʵ�����Բ�չƽ��νṹ��
        /// </summary>
        protected virtual BindingFlags InjectablePropertyBindingFlags { get; } = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        /// <summary>
        /// ��������ע�뵽Ŀ�����������С�
        /// </summary>
        /// <param name="target">Ҫע���Ŀ�����</param>
        /// <param name="options">ע��ѡ����Ϊ null����ʹ��Ĭ��ѡ�</param>
        public virtual void Inject(object target, ISnkPropertyInjectorOptions options = null)
        {
            // ���δ�ṩѡ���ʹ��Ĭ��ѡ�
            options = options ?? SnkPropertyInjectorOptions.All;

            // ���û��Ҫע������ԣ���ֱ�ӷ��ء�
            if (options.InjectIntoProperties == SnkPropertyInjection.None)
                return;

            // Ŀ�������Ϊ�ա�
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            // ����Ŀ����������п�ע������ԡ�
            var injectableProperties = FindInjectableProperties(target.GetType(), options);

            // �������п�ע������Բ�����ע�롣
            foreach (var injectableProperty in injectableProperties)
            {
                InjectProperty(target, injectableProperty, options);
            }
        }

        /// <summary>
        /// ��������ע�뵽ָ���������С�
        /// </summary>
        /// <param name="toReturn">Ŀ�����</param>
        /// <param name="injectableProperty">Ҫע������ԡ�</param>
        /// <param name="options">ע��ѡ�</param>
        protected virtual void InjectProperty(object toReturn, PropertyInfo injectableProperty, ISnkPropertyInjectorOptions options)
        {
            object propertyValue;
            // ���Դ�����ע��ϵͳ�н�������ֵ��
            if (SnkDIProvider.Instance.TryResolve(injectableProperty.PropertyType, out propertyValue) == true)
            {
                try
                {
                    // ��������ֵ���õ������С�
                    injectableProperty.SetValue(toReturn, propertyValue, null);
                }
                catch (TargetInvocationException invocation)
                {
                    // ���ע��ʧ�ܣ��׳��쳣��
                    throw new SnkDIResolveException(invocation, "Failed to inject into {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                }
            }
            else
            {
                // �������ע��ʧ�ܣ�����ѡ������Ƿ��׳��쳣���¼���档
                if (options.ThrowIfPropertyInjectionFails)
                {
                    throw new SnkDIResolveException("DependencyInjection property injection failed for {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                }
                else
                {
                    SnkLogHost.Default?.Warning("DependencyInjection property injection skipped for {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                }
            }
        }

        /// <summary>
        /// ����ָ�����������п�ע������ԡ�
        /// </summary>
        /// <param name="type">Ŀ���������͡�</param>
        /// <param name="options">ע��ѡ�</param>
        /// <returns>��ע�����Եļ��ϡ�</returns>
        protected virtual IEnumerable<PropertyInfo> FindInjectableProperties(Type type, ISnkPropertyInjectorOptions options)
        {
            // �������з��ϰ󶨱�־�Ĺ���ʵ�����ԡ�
            var injectableProperties = type
                .GetProperties(InjectablePropertyBindingFlags)
                .Where(p => p.PropertyType.GetTypeInfo().IsInterface)
                .Where(p => p.IsConventional())
                .Where(p => p.CanWrite);

            // ����ע��ѡ��������ԡ�
            switch (options.InjectIntoProperties)
            {
                case SnkPropertyInjection.InjectInterfaceProperties:
                    injectableProperties = injectableProperties
                        .Where(p => p.GetCustomAttributes(typeof(SnkInjectAttribute), false).Any());
                    break;

                case SnkPropertyInjection.AllInterfaceProperties:
                    break;

                case SnkPropertyInjection.None:
                    SnkLogHost.Default?.Error("Internal error - should not call FindInjectableProperties with SnkPropertyInjection.None");
                    injectableProperties = new PropertyInfo[0];
                    break;

                default:
                    throw new System.Exception($"unknown option for InjectIntoProperties {options.InjectIntoProperties}");
            }
            return injectableProperties;
        }
    }
}