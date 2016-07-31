using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Server.Service;

namespace Server.Test
{
    [TestFixture]
    public class ServiceTest
    {
        [Test]
        public void GetFilesTest()
        {
            Service.FileService service = new Service.FileService();
            service.GetFileNames().Should().NotBeEmpty();


        }
    }
}
