using System.Runtime.Serialization;
using System.ServiceModel;

namespace MediaDataApplication.WcfService.DataContracts {

    [DataContract]
    public class MediaMetadata : IDataValidator {
        [DataMember]
        public int MediaId { get; set; }

        [DataMember]
        public long FileLength { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string Description { get; set; }

        #region IDataValidator Members

        public void Validate(string context) {
            if (string.IsNullOrEmpty(this.FileName)) {
                throw new FaultException<MediaFault>(new MediaFault("Filename is required"), context + " Empty Filename");
            }
        }

        #endregion
    }

}