using System.Collections.Generic;
using System.Runtime.Serialization;
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

        [OperationContract]
        bool FileIsUpToDate(string filename, byte[] hash);

        [OperationContract]
        IEnumerable<ChunkContent> GetModifiedChunks(string fullPath, List<ChunkHash> cacheChunkHashes);

    }

    [DataContract]
    public class ChunkContent
    {
        [DataMember]
        public byte[] Content { get; set; }

        [DataMember]
        public bool UseUpdatedChunk { get; set; }

        [DataMember]
        public int PreviousLocation { get; set; }

        [DataMember]
        public int UpdatedLocation { get; set; }
    }

    [DataContract]
    public class ChunkHash
    {
        [DataMember]
        public byte[] Hash { get; set; }
        
        [DataMember]
        public int Location { get; set; }
        
    }

}
