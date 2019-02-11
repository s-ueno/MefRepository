using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace MefRepository
{
    /// <summary>
    /// dotnet core compatible version of RealProxy
    /// </summary>
    /// <remarks>
    /// RealProxy needed MarshalByRefObject for base class.
    /// Instead, all the actions of the acquired service have been proxied.
    /// 
    /// DispathProxy implements an arbitrary interface in the base class and acquires the service via that interface.
    /// The object to be proxied is the signature of the interface
    /// </remarks>
    public class ActionProxy : DispatchProxy
    {
        public virtual object Instance { get; set; }
        protected string Identity { get; set; }
        public ActionProxy()
        {
            this.Identity = Guid.NewGuid().ToString();
        }

        protected TraceLevel TraceLevel = AppSettings("FacadeProxy.TraceLevel", TraceLevel.Info);
        static T AppSettings<T>(string key, T defaultValue = default(T))
        {
            var result = defaultValue;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var value = appSettings.Get(key);
                if (!string.IsNullOrEmpty(value))
                {
                    result = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
                }
            }
            catch
            {
                //error free
            }
            return result;
        }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod == null)
                throw new ArgumentNullException(nameof(targetMethod));

            object result;


            Begin(targetMethod, args);
            try
            {
                result = targetMethod.Invoke(Instance, args);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException tex)
                {
                    Error(targetMethod, tex ?? ex);
                    Finally(targetMethod);
                    throw tex ?? ex;
                }
                else
                {
                    Error(targetMethod, ex);
                    Finally(targetMethod);
                    throw;
                }
            }


            if (result is Task task)
            {
                task.ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {
                        Error(targetMethod, t.Exception);
                    }
                    else
                    {
                        Complate(targetMethod, result);
                    }
                    Finally(targetMethod);
                });

            }
            else
            {
                Complate(targetMethod, result);
                Finally(targetMethod);
            }
            return result;
        }

        protected virtual void Begin(MethodInfo targetMethod, object[] args)
        {
            if (TraceLevel.Info <= this.TraceLevel)
            {
                Trace.TraceInformation($"{Identity} ---Begin--- {Instance?.GetType()?.FullName}.{targetMethod.Name}");
            }

            if (TraceLevel.Verbose <= this.TraceLevel)
            {
                var sb = new StringBuilder();
                var ps = targetMethod.GetParameters();
                if (ps != null && ps.Any())
                {
                    for (int i = 0; i < ps.Length; i++)
                    {
                        var p = ps[i];
                        var s = $"Type:{p.ParameterType.FullName}\tName:{p.Name}\tValue:";
                        if (args != null && i <= args.Length)
                        {
                            s += $"{args[1]}";
                        }
                        sb.AppendLine(s);
                    }
                }
                Trace.TraceInformation($"Parameters\n{sb.ToString()}");
            }
        }
        protected virtual void Complate(MethodInfo targetMethod, object result)
        {
            if (TraceLevel.Info <= this.TraceLevel)
            {
                Trace.TraceInformation($"{Identity} ---Complate--- {Instance?.GetType()?.FullName}.{targetMethod.Name}");
            }

            if (result is Task task)
            {
                if (TraceLevel.Verbose <= this.TraceLevel)
                {
                    Trace.TraceInformation("result is async task");
                }
            }
            else
            {
                if (TraceLevel.Verbose <= this.TraceLevel)
                {
                    using (var mem = new MemoryStream())
                    {
                        var sw = new StreamWriter(mem, Encoding.UTF8);
                        ObjectDumper.Write(result, 1, sw);
                        sw.Flush();

                        Trace.TraceInformation(Encoding.UTF8.GetString(mem.ToArray()));
                    }
                }
            }
        }
        protected virtual void Error(MethodInfo targetMethod, Exception ex)
        {
            if (TraceLevel.Info <= this.TraceLevel)
            {
                Trace.TraceInformation($"{Identity} ---Error--- {Instance?.GetType()?.FullName}.{targetMethod.Name}\t{ex.ToString()}");
            }
        }
        protected virtual void Finally(MethodInfo targetMethod)
        {
            if (TraceLevel.Info <= this.TraceLevel)
            {
                Trace.TraceInformation($"{Identity} ---Finally--- {Instance?.GetType()?.FullName}.{targetMethod.Name}");
            }
        }
    }
}
