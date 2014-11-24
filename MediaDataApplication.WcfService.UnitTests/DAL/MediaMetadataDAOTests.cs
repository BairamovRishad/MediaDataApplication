using System.Linq;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DAL.DAO;
using MediaDataApplication.WcfService.DAL.Repository;
using MediaDataApplication.WcfService.Tests.Fake.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Модульными тестами это сложно назвать, скорее быстрые точки входа.

namespace MediaDataApplication.WcfService.Tests.DAL {

    [TestClass]
    public class MediaMetadataDAOTests {
        private MediaMetadataDAO mediaMetadataDAO;
        private string userName;

        private IUnitOfWork UnitOfWork { get; set; }

        [TestMethod]
        public void GetMediaMetadata_ExistentMedia_UpdatesThumb() {
            // Arrange
            var mediaMetadataBDO = new MediaMetadataBDO {
                                                            FileName = "someName",
                                                            Description = "this is something",
                                                            FileLength = 1000L
                                                        };

            // Act 
            var filesName = this.mediaMetadataDAO.GetAllUserMediaFileNames(this.userName);

            // Assert
            int expectedCount = this.UnitOfWork.UserRepository.Single(x => x.UserName.Equals(this.userName)).Media.Count();
            Assert.AreEqual(expectedCount, filesName.Count());
        }

        [TestInitialize]
        public void TestStartup() {
            this.UnitOfWork = new FakeUnitOfWork();
            this.mediaMetadataDAO = new MediaMetadataDAO(this.UnitOfWork);

            this.userName = this.UnitOfWork.UserRepository.GetById(1).UserName;
        }
    }

}