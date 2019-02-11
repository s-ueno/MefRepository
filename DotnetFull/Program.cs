using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using Impl;
using MefRepository;

namespace DotnetFull
{

    class Program
    {
        static void Main(string[] args)
        {
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
            catch (Exception ex)
            {
                var message = ex.ToString();
            }
            Console.WriteLine("Please enter some key.");
            Console.ReadKey();
        }
    }
}
