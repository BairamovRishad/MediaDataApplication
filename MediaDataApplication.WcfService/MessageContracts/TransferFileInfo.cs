using System;
using System.IO;
using System.ServiceModel;
using MediaDataApplication.WcfService.DataContracts;

namespace MediaDataApplication.WcfService.MessageContracts {

    [MessageContract]
    public abstract class TransferFileInfo : IDisposable, IDataValidator {
        [MessageBodyMember(Order = 1)]
        public Stream FileByteStream;

        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        #region IDataValidator Members

        public virtual void Validate(string context) {
            if (string.IsNullOrEmpty(this.FileName)) {
                throw new FaultException<MediaFault>(new MediaFault("Filename is required"), context + " Empty Filename");
            }

            if (this.FileByteStream == null) {
                throw new FaultException<MediaFault>(new MediaFault("File file is required"), context + " Empty File");
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            if (this.FileByteStream != null) {
                this.FileByteStream.Close();
                this.FileByteStream = null;
            }
        }

        #endregion
    }

}