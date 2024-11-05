using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SnkFramework.Logging;

namespace SnkDependencyInjection
{
    /// <summary>
    /// 属性注入器的实现。
    /// </summary>
    public class SnkPropertyInjector : ISnkPropertyInjector
    {
        /// <summary>
        /// 获取或设置可注入属性的绑定标志。默认值为公共实例属性并展平层次结构。
        /// </summary>
        protected virtual BindingFlags InjectablePropertyBindingFlags { get; } = BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        /// <summary>
        /// 将依赖项注入到目标对象的属性中。
        /// </summary>
        /// <param name="target">要注入的目标对象。</param>
        /// <param name="options">注入选项。如果为 null，将使用默认选项。</param>
        public virtual void Inject(object target, ISnkPropertyInjectorOptions options = null)
        {
            // 如果未提供选项，则使用默认选项。
            options = options ?? SnkPropertyInjectorOptions.All;

            // 如果没有要注入的属性，则直接返回。
            if (options.InjectIntoProperties == SnkPropertyInjection.None)
                return;

            // 目标对象不能为空。
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            // 查找目标对象中所有可注入的属性。
            var injectableProperties = FindInjectableProperties(target.GetType(), options);

            // 遍历所有可注入的属性并进行注入。
            foreach (var injectableProperty in injectableProperties)
            {
                InjectProperty(target, injectableProperty, options);
            }
        }

        /// <summary>
        /// 将依赖项注入到指定的属性中。
        /// </summary>
        /// <param name="toReturn">目标对象。</param>
        /// <param name="injectableProperty">要注入的属性。</param>
        /// <param name="options">注入选项。</param>
        protected virtual void InjectProperty(object toReturn, PropertyInfo injectableProperty, ISnkPropertyInjectorOptions options)
        {
            object propertyValue;
            // 尝试从依赖注入系统中解析属性值。
            if (SnkDIProvider.Instance.TryResolve(injectableProperty.PropertyType, out propertyValue) == true)
            {
                try
                {
                    // 将解析的值设置到属性中。
                    injectableProperty.SetValue(toReturn, propertyValue, null);
                }
                catch (TargetInvocationException invocation)
                {
                    // 如果注入失败，抛出异常。
                    throw new SnkDIResolveException(invocation, "Failed to inject into {0} on {1}", injectableProperty.Name, toReturn.GetType().Name);
                }
            }
            else
            {
                // 如果属性注入失败，根据选项决定是否抛出异常或记录警告。
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
        /// 查找指定类型中所有可注入的属性。
        /// </summary>
        /// <param name="type">目标对象的类型。</param>
        /// <param name="options">注入选项。</param>
        /// <returns>可注入属性的集合。</returns>
        protected virtual IEnumerable<PropertyInfo> FindInjectableProperties(Type type, ISnkPropertyInjectorOptions options)
        {
            // 查找所有符合绑定标志的公共实例属性。
            var injectableProperties = type
                .GetProperties(InjectablePropertyBindingFlags)
                .Where(p => p.PropertyType.GetTypeInfo().IsInterface)
                .Where(p => p.IsConventional())
                .Where(p => p.CanWrite);

            // 根据注入选项过滤属性。
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