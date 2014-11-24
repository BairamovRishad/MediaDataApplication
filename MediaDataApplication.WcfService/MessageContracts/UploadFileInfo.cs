using System.ServiceModel;
using MediaDataApplication.WcfService.DataContracts;

namespace MediaDataApplication.WcfService.MessageContracts {

    [MessageContract]
    public class UploadFileInfo : TransferFileInfo {
        [MessageHeader(MustUnderstand = true)]
        public string UserName;

        public override void Validate(string context) {
            if (string.IsNullOrEmpty(this.UserName)) {
                throw new FaultException<UserFault>(new UserFault("Username is required"), context + " Empty Username");
            }

            base.Validate(context);
        }
    }

    [MessageContract]
    public class UploadThumbFileInfo : UploadFileInfo {
        [MessageHeader(MustUnderstand = true)]
        public string MediaFileName;

        public override void Validate(string context) {
            if (string.IsNullOrEmpty(this.MediaFileName)) {
                throw new FaultException<MediaFault>(new MediaFault("MediaFileName is required"),
                                                     context + " Empty MediaFileName");
            }

            base.Validate(context);
        }
    }

    [MessageContract]
    public class UploadMediaFileInfo : UploadFileInfo { }

}