using System.Collections.Generic;
using System.ServiceModel;

namespace Server.Service
{
    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        IEnumerable<string> GetFileNames();

        [OperationContract]
        byte[] GetFile(string filename);
        
    }
}
