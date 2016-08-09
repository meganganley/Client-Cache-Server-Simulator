using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Cache.Service
{
    public class CacheFileService : ICacheFileService
    {
        public string GetFileNames(int value)
        {
            throw new NotImplementedException();
        }

        public byte[] GetFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
