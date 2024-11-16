using System;
using System.Collections.Generic;
using System.Reflection;

namespace SnkDependencyInjection
{
    /// <summary>
    /// 定义类型缓存接口，用于存储和查找程序集中的类型信息。
    /// </summary>
    public interface ISnkTypeCache
    {
        /// <summary>
        /// 获取类型缓存表，其键为类型的全名的小写形式。
        /// </summary>
        Dictionary<string, Type> LowerCaseFullNameCache { get; }

        /// <summary>
        /// 获取类型缓存表，其键为类型的全名。
        /// </summary>
        Dictionary<string, Type> FullNameCache { get; }

        /// <summary>
        /// 获取类型缓存表，其键为类型的名称。
        /// </summary>
        Dictionary<string, Type> NameCache { get; }

        /// <summary>
        /// 向缓存中添加程序集中的所有类型。
        /// </summary>
        /// <param name="assembly">需要缓存的程序集。</param>
        void AddAssembly(Assembly assembly);
    }
}