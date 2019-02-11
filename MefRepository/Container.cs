using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Composition.Hosting;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Composition.Convention;
using System.Composition;
using System.Composition.Hosting.Core;

namespace MefRepository
{
    public static class Container
    {
        static readonly CompositionHost host;
        static Container()
        {
            var conf = new ContainerConfiguration();
            //var conventions = GetConventions();

            var assembliesList = new List<string>();
            var assemblies = ConfigurationManager.GetSection("Container.Assemblies") as NameValueCollection;
            if (assemblies?.AllKeys != null)
                assembliesList.AddRange(assemblies.AllKeys);

            if (Assembly.GetEntryAssembly() != null)
            {
                assembliesList.Add(Assembly.GetEntryAssembly().FullName);
            }

            if (assembliesList.Any())
                conf.WithAssemblies(assembliesList.Select(x => LoadAssembly(x))/*, conventions*/);


            var types = ConfigurationManager.GetSection("Container.Types") as NameValueCollection;
            var typeList = new List<Type>();
            if (types?.AllKeys != null)
                typeList.AddRange(types.AllKeys.Select(x => LoadType(x)));


            if (typeList.Any())
                conf.WithParts(typeList/*, conventions*/);

            host = conf.CreateContainer();
        }
        static Assembly LoadAssembly(string s)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load(s);
            }
            catch
            {
            }
            return assembly;
        }
        static Type LoadType(string s)
        {
            Type type = null;
            try
            {
                type = Type.GetType(s);
            }
            catch
            {
            }
            return type;
        }

        /// <summary>
        /// Get instance with most priority
        /// </summary>
        public static T GetInstance<T>()
        {
            var mostPriority =
                    All<T, PriorityMetadataView>()
                        .OrderBy(x => x.Metadata.Priority)
                        .FirstOrDefault();
            if (mostPriority != null)
                return mostPriority.CreateExport().Value;

            return default(T);
        }

        /// <summary>
        /// Retrieve service via default transparent proxy
        /// </summary>
        /// <remarks>
        /// <seealso cref="ActionProxy"/>
        /// </remarks>
        public static T GetTransparentProxy<T>() 
        {
            var proxy = ActionProxy.Create<T, ActionProxy>();

            (proxy as ActionProxy).Instance = GetInstance<T>();

            return proxy;
        }

        /// <summary>
        /// Retrieve service via arbitrary transparent proxy
        /// </summary>
        /// <remarks>
        /// <seealso cref="ActionProxy"/>
        /// </remarks>
        public static T GetTransparentProxy<T, TProxy>() 
            where TProxy : ActionProxy
        {
            var proxy = ActionProxy.Create<T, TProxy>();

            (proxy as ActionProxy).Instance = GetInstance<T>();

            return proxy;
        }

        public static T GetByContext<T>(string context)
        {
            var firstContext =
                    All<T, ContextMetadataView>()
                        .FirstOrDefault(x => x.Metadata.Context == context);
            if (firstContext != null)
                return firstContext.CreateExport().Value;

            return default(T);
        }

        public static IEnumerable<T> All<T>()
        {
            return host.GetExports<T>();
        }
        public static IEnumerable<ExportFactory<T, TMetadataview>> All<T, TMetadataview>()
        {
            return host.GetExports<ExportFactory<T, TMetadataview>>();
        }

        public static CompositionHost Host { get { return host; } }

    }
}
