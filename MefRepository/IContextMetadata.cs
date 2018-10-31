using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace MefRepository
{
    public interface IContextMetadata
    {
        string Context { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false), MetadataAttribute]
    public class ContextMetadataAttribute : Attribute, IContextMetadata
    {
        public ContextMetadataAttribute(string context)
        {
            this.Context = context;
        }
        public string Context { get; set; }
    }

    internal class ContextMetadataView : IContextMetadata
    {
        public string Context { get; set; }
    }

}
