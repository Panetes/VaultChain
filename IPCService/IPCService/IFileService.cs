using System.IO;
using System.ServiceModel;

namespace IPCService
{
    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        void SaveFile(SaveFileRequest saveFileRequest);

        [OperationContract]
        DownloadFileResponse DownloadFile();    
    }

    [MessageContract]
    public class DownloadFileResponse
    {
        [MessageHeader]
        public string FileName { get; set; }

        [MessageBodyMember]
        public Stream Stream { get; set; }
    }

    [MessageContract]
    public class SaveFileRequest
    {
        [MessageHeader]
        public string FileName { get; set; }

        [MessageBodyMember]
        public Stream Stream { get; set; }

    }
}
