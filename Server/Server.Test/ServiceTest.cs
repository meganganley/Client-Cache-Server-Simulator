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
            Service.Service service = new Service.Service();
            service.GetFileNames().Should().NotBeEmpty();


        }
    }
}
