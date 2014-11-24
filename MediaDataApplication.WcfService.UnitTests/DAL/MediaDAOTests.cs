using System;
using System.Globalization;
using System.IO;
using System.Linq;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DAL.DAO;
using MediaDataApplication.WcfService.DAL.Mappers;
using MediaDataApplication.WcfService.DAL.Repository;
using MediaDataApplication.WcfService.Tests.Fake.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

// Модульными тестами это сложно назвать, скорее быстрые точки входа.

namespace MediaDataApplication.WcfService.Tests.DAL {

    [TestClass]
    public class MediaDAOTests {
        private MediaDAO mediaDAO;
        private string userName;

        private IUnitOfWork UnitOfWork { get; set; }

        [TestMethod]
        public void AddMediaThumbnail_ExistentMedia_UpdatesThumb() {
            // Arrange
            var media = this.UnitOfWork.MediaRepository.GetById(1);
            string mediaFileName = media.MediaMetadata.FileName;

            int countBefore = this.UnitOfWork.MediaThumbnailsRepository.AsQueryable().Count();
            string thumbFileNewName = new Random().Next().ToString(CultureInfo.InvariantCulture);

            // Act 
            this.AddMediaThumbnailAct(mediaFileName, thumbFileNewName);

            // Assert
            int countAfter = this.UnitOfWork.MediaThumbnailsRepository.GetAll().Count();
            Assert.AreEqual(countBefore, countAfter);

            Assert.AreEqual(thumbFileNewName, media.MediaThumbnail.FileName);
        }

        [TestMethod]
        public void AddMediaThumbnail_NewMedia_AddsThumb() {
            // Arrange
            var mediaFileName = new Random().Next().ToString(CultureInfo.InvariantCulture);
            this.AddMediaAct(mediaFileName);

            int countBefore = this.UnitOfWork.MediaThumbnailsRepository.AsQueryable().Count();
            string thumbFileNewName = new Random().Next().ToString(CultureInfo.InvariantCulture);

            // Act
            this.AddMediaThumbnailAct(mediaFileName, thumbFileNewName);

            // Assert
            var userMedia = this.UnitOfWork.UserRepository.SingleOrDefault(x => x.UserName.Equals(this.userName)).Media;
            Assert.AreEqual(countBefore + 1, userMedia.Count());

            var addedThumb = userMedia.Where(x => x.MediaThumbnail.FileName.Equals(thumbFileNewName)).Select(x => x.MediaThumbnail);
            Assert.IsNotNull(addedThumb);
        }

        [TestMethod]
        public void AddMedia_ExistentMedia_UpdatesMedia() {
            // Arrange
            var media = this.UnitOfWork.MediaRepository.GetById(1);
            string mediaFileName = media.MediaMetadata.FileName;

            int countBefore = this.UnitOfWork.MediaRepository.AsQueryable().Count();

            // Act 
            this.AddMediaAct(mediaFileName);

            // Assert
            var userMedia = this.UnitOfWork.UserRepository.SingleOrDefault(x => x.UserName.Equals(this.userName)).Media;
            Assert.AreEqual(countBefore, userMedia.Count());

            var updatedMedia = userMedia.SingleOrDefault(x => x.MediaMetadata.FileName.Equals(mediaFileName));
            Assert.IsNotNull(updatedMedia);
        }

        [TestMethod]
        public void AddMedia_NewMedia_AddsNewMedia() {
            // Arrange
            var mediaFileName = new Random().Next().ToString(CultureInfo.InvariantCulture);
            int countBefore = this.UnitOfWork.MediaRepository.GetAll().Count();

            // Act
            this.AddMediaAct(mediaFileName);

            // Assert
            int countAfter = this.UnitOfWork.UserRepository.SingleOrDefault(x => x.UserName.Equals(this.userName)).Media.Count();
            Assert.AreEqual(countBefore + 1, countAfter);
        }

        [TestMethod]
        public void DeleteMedia_ExistentMedia_DeletesMedia() {
            // Arrange
            var mediaFileName = new Random().Next().ToString(CultureInfo.InvariantCulture);
            this.AddMediaAct(mediaFileName);
            string thumbFileNewName = new Random().Next().ToString(CultureInfo.InvariantCulture);
            this.AddMediaThumbnailAct(mediaFileName, thumbFileNewName);

            // Act
            this.mediaDAO.DeleteMediaAndThumbnail(this.userName, mediaFileName);

            // Assert
        }

        [TestMethod]
        public void GetMedia() {
            // Arrange
            var mediaFileName = new Random().Next().ToString(CultureInfo.InvariantCulture);
            this.AddMediaAct(mediaFileName);
            var fileChunk = new FileChunkBDO {
                                                 FileName = mediaFileName,
                                                 Length = 100000000,
                                                 Offset = 0
                                             };

            // Act
            var media = this.mediaDAO.GetMedia(this.userName, fileChunk);

            // Assert
            Assert.AreEqual(100000000, media.Length);

            media.Dispose();
        }

        [TestMethod]
        public void GetMediaThumbnail() {
            // Arrange
            var media = this.UnitOfWork.MediaRepository.GetById(1);
            string mediaFileName = media.MediaMetadata.FileName;

            string thumbFileNewName = new Random().Next().ToString(CultureInfo.InvariantCulture);
            this.AddMediaThumbnailAct(mediaFileName, thumbFileNewName);

            // Act
            var names = this.mediaDAO.GetMediaThumbnail(this.userName, thumbFileNewName);

            // Assert
            Assert.AreEqual(10000, names.Length);
            names.Dispose();
        }

        [TestInitialize]
        public void TestStartup() {
            this.UnitOfWork = new FakeUnitOfWork();

            var kernel = new StandardKernel();
            kernel.Bind<IUnitOfWork>().ToMethod(x => this.UnitOfWork);
            kernel.Bind<IMapper>().To<CommonMapper>();

            this.mediaDAO = kernel.Get<MediaDAO>();
            this.userName = this.UnitOfWork.UserRepository.GetById(1).UserName;
        }

        #region Private Helpers

        private void AddMediaAct(string mediaFileName) {
            var testMediaFile = new byte[100000000];
            using (var stream = new MemoryStream(testMediaFile)) {
                this.mediaDAO.AddMedia(this.userName, mediaFileName, stream);
            }
        }

        private void AddMediaThumbnailAct(string mediaFileName, string thumbFileName) {
            var testThumbFile = new byte[10000];
            using (var stream = new MemoryStream(testThumbFile)) {
                this.mediaDAO.AddMediaThumbnail(this.userName, mediaFileName, thumbFileName, stream);
            }
        }

        #endregion
    }

}