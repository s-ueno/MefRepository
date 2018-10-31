using MefRepository;
using System;

namespace Ext
{
    [ExportPriority(typeof(Impl.ISayHello), 0 /* most priority */)]
    public class ExtensionImpl : Impl.ISayHello
    {
        public string Do()
        {
            return "Hello World!!";
        }
    }
}
