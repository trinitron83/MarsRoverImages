using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarsRoverImages;
using System.Threading.Tasks;

namespace ImageAPITests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestGetImageUrls()
        {
            var controller = new MarsRoverImages.Controllers.RoverImagesController();
            var urls = await controller.GetImageUrls("01/01/2018");
            Assert.IsTrue(urls.Count > 0);
        }

        [TestMethod]
        public async Task TestGetImagesForDate()
        {
            var controller = new MarsRoverImages.Controllers.RoverImagesController();
            var images = await controller.GetImagesForDate("01/01/2018");
            Assert.IsTrue(images.Count > 0);
        }

        [TestMethod]
        public async Task TestGetImagesForDataFile()
        {
            var controller = new MarsRoverImages.Controllers.RoverImagesController();
            var images = await controller.GetImagesForDataFile();
            Assert.IsTrue(images.Count > 0);
        }

    }
}
