using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.Text;

namespace MefRepository
{
    public interface IPriorityMetadata
    {
        int Priority { get; set; }
    }
    internal class PriorityMetadataView : IPriorityMetadata
    {
        public int Priority { get; set; }
    }
}
