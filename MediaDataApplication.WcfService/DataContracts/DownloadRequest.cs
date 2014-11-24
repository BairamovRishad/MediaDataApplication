using System;
using System.ServiceModel;

namespace MediaDataApplication.WcfService.DataContracts {

    [MessageContract]
    public class DownloadRequest : IDataValidator {
        [MessageBodyMember]
        public string FileName;

        [MessageBodyMember]
        public string UserName;

        #region IDataValidator Members

        public virtual void Validate(string context) {
            if (string.IsNullOrEmpty(this.UserName)) {
                throw new FaultException<UserFault>(new UserFault("Username is required"), context + " Empty Username");
            }

            if (string.IsNullOrEmpty(this.FileName)) {
                throw new FaultException<MediaFault>(new MediaFault("Filename is required"), context + " Empty Filename");
            }
        }

        #endregion
    }

    [MessageContract]
    public class DownloadChunkRequest : DownloadRequest {
        [MessageBodyMember]
        public int Length;

        [MessageBodyMember]
        public Int64 Offset;

        public override void Validate(string context) {
            base.Validate(context);

            if (this.Length < 0) {
                throw new FaultException<MediaFault>(new MediaFault("Chunk length must be nonnegative"), context + " Negative Chunk length");
            }

            if (this.Offset < 0) {
                throw new FaultException<MediaFault>(new MediaFault("Chunk offset must be nonnegative"), context + " Negative Chunk offset");
            }
        }
    }

}