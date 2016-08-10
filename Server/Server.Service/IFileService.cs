using System.Collections.Generic;
using System.ServiceModel;

namespace Server.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFileService" in both code and config file together.
    [ServiceContract]
    public interface IFileService
    {

        [OperationContract]
        IEnumerable<string> GetFileNames();

        [OperationContract]
        byte[] GetFile(string filename);

        // TODO: Add your service operations here
    }
}
