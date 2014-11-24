using System.Runtime.Serialization;
using System.ServiceModel;

namespace MediaDataApplication.WcfService.DataContracts {

    [DataContract]
    public class User : IDataValidator {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        #region IDataValidator Members

        public void Validate(string context) {
            if (string.IsNullOrEmpty(this.UserName)) {
                throw new FaultException<UserFault>(new UserFault("UserName is required"), context + " Empty UserName");
            }

            if (string.IsNullOrEmpty(this.Password)) {
                throw new FaultException<UserFault>(new UserFault("Password is required"), context + " Empty Password");
            }
        }

        #endregion
    }

}