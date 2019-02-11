using Impl;
using MefRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetFull
{
    /// <summary>
    /// Since there is a class that implements ISayHello with higher priority than this class, this class will not be executed.
    /// </summary>
    [ExportPriority(typeof(ISayHello))]
    public class LowPrioritySayHello : ISayHello
    {
        public string Do()
        {
            return "LowPrioritySayHello";
        }
    }

}
