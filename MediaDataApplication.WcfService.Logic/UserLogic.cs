using System;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DAL.DAO;

namespace MediaDataApplication.WcfService.Logic {

    public class UserLogic {
        private readonly UserDAO userDAO = new UserDAO();

        public void LoginUser(string userName, string password) {
            var userInDB = this.userDAO.GetUser(userName);

            if (userInDB == null) {
                throw new Exception("Unknown userName");
            }

            if (!Security.Match(userInDB.Password, password)) {
                throw new Exception("Invalid password");
            }
        }

        public void RegisterUser(UserBDO newUser) {
            var userInDB = this.userDAO.GetUser(newUser.UserName);

            if (userInDB == null) {
                newUser.Password = Security.Encrypt(newUser.Password);
                this.userDAO.AddUser(newUser);
            }
            else {
                throw new Exception("Username \'" + newUser.UserName + "\' + already registered");
            }
        }
    }

}