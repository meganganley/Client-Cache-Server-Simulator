﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Server.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service.svc or Service.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        private static readonly string ServerFilesLocation = Path.Combine(Environment.CurrentDirectory, @"ServerFiles");

        public IEnumerable<string> GetFileNames()
        {
            throw new NotImplementedException();
        }

        public Stream GetFile(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
