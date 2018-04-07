using System.ServiceModel;

namespace ConsoleApp1
{
    [ServiceContract]
    interface IServiceContract
    {
        [OperationContract]
        int ShareFile(string path, string address, string walletAddress);
    }
}
