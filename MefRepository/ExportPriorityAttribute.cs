using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.Composition.Hosting.Core;
using System.Text;

namespace MefRepository
{
    /// <summary>
    /// Customize <see cref="ExportAttribute"/> and consider the priority of "class" registered more than once in the same contract
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false), MetadataAttribute]
    public class ExportPriorityAttribute : ExportAttribute, IPriorityMetadata
    {
        public ExportPriorityAttribute(Type type, int priority = int.MaxValue) : base(type)
        {
            this.Priority = priority;
        }
        public int Priority { get; set; }
    }
}
