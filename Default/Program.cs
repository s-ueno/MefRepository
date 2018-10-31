using Impl;
using MefRepository;
using System;

namespace Default
{
    [ExportPriority(typeof(ISayHello))]
    public class DefaultClass : ISayHello
    {
        public string Do()
        {
            return "Default";
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            // Rewriting app.config will change the service you get
            var svc = Container.GetInstance<ISayHello>();
            Console.WriteLine(svc.Do());
            Console.WriteLine("Please enter some key.");
            Console.ReadKey();
        }
    }
}
