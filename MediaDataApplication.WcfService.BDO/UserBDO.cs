using System;

namespace MediaDataApplication.WcfService.BDO {

    public class UserBDO {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreationDate { get; set; }
    }

}