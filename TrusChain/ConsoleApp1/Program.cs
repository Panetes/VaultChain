using System;
using System.ServiceModel;
using TrusChain.Storage;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            IpfsWrapper.Init();

            string address = "net.pipe://localhost/ShareFileService";
 
            ServiceHost serviceHost = new ServiceHost(typeof(ShareFileService));
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            serviceHost.AddServiceEndpoint(typeof(IServiceContract), binding, address);
            serviceHost.Open();

            Console.WriteLine("ServiceHost running. Press Return to Exit");
            Console.ReadLine();

            IpfsWrapper.CloseDaemon();
        }
    }
}
