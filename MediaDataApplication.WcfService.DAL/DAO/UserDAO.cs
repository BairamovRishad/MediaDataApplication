using System;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DAL.Entities;
using MediaDataApplication.WcfService.DAL.Repository;

namespace MediaDataApplication.WcfService.DAL.DAO {

    public class UserDAO : BaseDAO {
        public UserDAO() { }

        public UserDAO(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void AddUser(UserBDO newUserBDO) {
            var newUser = Mapper.Map<UserBDO, User>(newUserBDO);
            newUser.CreationDate = DateTime.Now;
            UnitOfWork.UserRepository.Add(newUser);
            UnitOfWork.Commit();
        }

        public UserBDO GetUser(string userName) {
            var foundUser = UnitOfWork.UserRepository.SingleOrDefault(x => x.UserName.Equals(userName));

            UserBDO userBDO = null;
            if (foundUser != null) {
                userBDO = Mapper.Map<User, UserBDO>(foundUser);
            }
            return userBDO;
        }
    }

}