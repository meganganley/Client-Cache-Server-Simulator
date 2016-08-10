using System.Collections.Generic;
using System.ServiceModel;

namespace Cache.Service
{
    [ServiceContract]
    public interface ICacheFileService
    {
        [OperationContract]
        IEnumerable<string> GetFileNames();

        [OperationContract]
        byte[] GetFile(string path);
        
    }

}
