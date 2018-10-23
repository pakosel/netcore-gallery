
using NUnit.Framework;

namespace aspnettcoreapp.Tests
{
    public class FilesystemHelperTests
    {
        [Test]
        public void GetDirContentCorrectPaths()
        {
            var path = "/";
            var ret = FilesyStemHelper.GetDirContent(path, out var errorMsg);
            Assert.IsEmpty(errorMsg);
            Assert.IsNotNull(ret);
        }
    }
}