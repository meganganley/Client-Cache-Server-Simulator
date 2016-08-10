using FluentAssertions;
using NUnit.Framework;

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
