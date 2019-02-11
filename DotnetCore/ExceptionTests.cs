using MefRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetCore
{
    public interface IExceptionTests {
        void Do();
    }

    [ExportPriority(typeof(IExceptionTests))]
    public class ExceptionTests : IExceptionTests
    {
        public void Do()
        {
            throw new NotImplementedException();
        }
    }
}
