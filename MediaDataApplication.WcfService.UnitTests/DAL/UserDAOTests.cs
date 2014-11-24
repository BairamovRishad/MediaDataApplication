using System;
using System.Linq;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DAL.DAO;
using MediaDataApplication.WcfService.DAL.Repository;
using MediaDataApplication.WcfService.Tests.Fake.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Модульными тестами это сложно назвать, скорее быстрые точки входа.

namespace MediaDataApplication.WcfService.Tests.DAL {

    [TestClass]
    public class UserDAOTests {
        private UserDAO userDAO;

        private IUnitOfWork UnitOfWork { get; set; }

        [TestMethod]
        public void AddUser_NewUser_AddsNewUser() {
            // Arrange
            var userBDO = new UserBDO {
                                          UserID = 2,
                                          UserName = "Bill",
                                          FirstName = "Bill",
                                          LastName = "Bill",
                                          Password = "bill",
                                          CreationDate = DateTime.Now
                                      };
            int countBefore = this.UnitOfWork.UserRepository.AsQueryable().Count();

            // Act 
            this.userDAO.AddUser(userBDO);

            // Assert
            int countAfter = this.UnitOfWork.UserRepository.AsQueryable().Count();
            Assert.AreEqual(countBefore + 1, countAfter);
        }

        [TestMethod]
        public void GetUser_ExistentUser_ReturnsRequestedUser() {
            // Arrange
            var user = this.UnitOfWork.UserRepository.GetById(1);
            var userName = user.UserName;

            // Act 
            var userBDO = this.userDAO.GetUser(userName);

            // Assert
            Assert.AreEqual(user.FirstName, userBDO.FirstName);
        }

        [TestInitialize]
        public void TestStartup() {
            this.UnitOfWork = new FakeUnitOfWork();
            this.userDAO = new UserDAO(this.UnitOfWork);
        }
    }

}