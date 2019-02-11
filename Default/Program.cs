using Impl;
using MefRepository;
using System;
using System.Reflection;

namespace Default
{
    class Program
    {

        static void Main(string[] args)
        {
            // Rewriting app.config will change the service you get
            var svc = Container.GetInstance<ISayHello>();
            Console.WriteLine(svc.Do());

            Console.WriteLine("Next we will run the proxied service.\n\n");
            svc = Container.GetTransparentProxy<ISayHello>();
            Console.WriteLine(svc.Do());


            Console.WriteLine("Finally run the service with the custom proxy class applied.\n\n");
            svc = Container.GetTransparentProxy<ISayHello, CustomProxy>();
            Console.WriteLine($"result => {svc.Do()}");


            var svc2 = Container.GetTransparentProxy<ISampleClass>();
            try
            {
                svc2.Do(true);
            }
            catch 
            {
            }
            Console.WriteLine("Please enter some key.");
            Console.ReadKey();
        }
    }


    /// <summary>
    /// It is not executed because there is a higher priority service than this service.
    /// </summary>
    [ExportPriority(typeof(ISayHello))]
    public class DefaultClass : ISayHello
    {
        public int Index { get; set; } = 0;
        public string Do()
        {
            if (3 <= ++Index)
            {
                throw new Exception("Sample Exception");
            }
            return "Default";
        }
    }

    /// <summary>
    /// customization of the class that proxies the processing you perform.
    /// </summary>
    public class CustomProxy : MefRepository.ActionProxy
    {
        protected override void Begin(MethodInfo targetMethod, object[] args)
        {
            base.Begin(targetMethod, args);
        }
        protected override void Complate(MethodInfo targetMethod, object result)
        {
            base.Complate(targetMethod, result);
        }
        protected override void Finally(MethodInfo targetMethod)
        {
            base.Finally(targetMethod);
        }
        protected override void Error(MethodInfo targetMethod, Exception ex)
        {
            base.Error(targetMethod, ex);
        }
    }



    public interface ISampleClass
    {
        void Do(bool isThrowException);
    }

    [ExportPriority(typeof(ISampleClass))]
    public class SampleClass : ISampleClass
    {
        public void Do(bool isThrowException)
        {
            Console.WriteLine($"isThrowException:{isThrowException}");
            if (isThrowException)
                throw new Exception(" throw Hi!");
        }
    }
}

