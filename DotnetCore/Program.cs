using Impl;
using MefRepository;
using System;

namespace DotnetCore
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


            var svc2 = Container.GetTransparentProxy<IExceptionTests>();
            try
            {
                svc2.Do();
            }
            catch(Exception ex)
            {
                var message = ex.ToString();
            }
            Console.WriteLine("Please enter some key.");
            Console.ReadKey();
        }
    }
}
