using System.ServiceModel;

namespace IPCService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FileService : IFileService
    {
        public void SaveFile(SaveFileRequest saveFileRequest)
        {
            // Do your stuff
        }

        public DownloadFileResponse DownloadFile()
        {
            // Do your stuff
            return null;
        }
    }
}
