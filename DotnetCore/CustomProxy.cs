using MefRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace DotnetCore
{
    /// <summary>
    /// customization of the class that proxies the processing you perform.
    /// </summary>
    /// <remarks>
    ///  In general, proxying classes are adopted at the top level of requests like facade components and services.
    ///  The proxy is responsible for AOP such as transaction control, exception handling, logging etc.    
    /// </remarks>
    public class CustomProxy : ActionProxy
    {
        protected override void Begin(MethodInfo targetMethod, object[] args)
        {
            Trace.TraceInformation("CustomProxy Begin");

            base.Begin(targetMethod, args);
        }
        protected override void Complate(MethodInfo targetMethod, object result)
        {
            Trace.TraceInformation("CustomProxy Complate");

            base.Complate(targetMethod, result);
        }
        protected override void Finally(MethodInfo targetMethod)
        {
            Trace.TraceInformation("CustomProxy Finally");

            base.Finally(targetMethod);
        }
        protected override void Error(MethodInfo targetMethod, Exception ex)
        {
            Trace.TraceInformation("CustomProxy Error");

            base.Error(targetMethod, ex);
        }
    }
}
