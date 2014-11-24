using System.Runtime.Serialization;

namespace MediaDataApplication.WcfService.DataContracts {

    [DataContract]
    public abstract class CommonFault {
        [DataMember]
        public string FaultMessage;

        protected CommonFault(string msg) {
            this.FaultMessage = msg;
        }
    }

    [DataContract]
    public class UserFault : CommonFault {
        public UserFault(string msg) : base(msg) { }
    }

    [DataContract]
    public class MediaFault : CommonFault {
        public MediaFault(string msg) : base(msg) { }
    }

}