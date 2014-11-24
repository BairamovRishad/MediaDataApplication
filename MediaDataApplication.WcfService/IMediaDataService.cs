using System.Collections.Generic;
using System.ServiceModel;
using MediaDataApplication.WcfService.DataContracts;
using MediaDataApplication.WcfService.MessageContracts;

namespace MediaDataApplication.WcfService {

    [ServiceContract]
    public interface IMediaDataService : IMediaService, IUserService { }

    [ServiceContract]
    public interface IMediaService {
        [OperationContract]
        [FaultContract(typeof(UserFault))]
        [FaultContract(typeof(MediaFault))]
        void DeleteMedia(string userName, string[] mediaFilesName);

        [OperationContract]
        [FaultContract(typeof(UserFault))]
        [FaultContract(typeof(MediaFault))]
        DownloadFileInfo DownloadMedia(DownloadChunkRequest request);

        [OperationContract]
        [FaultContract(typeof(UserFault))]
        [FaultContract(typeof(MediaFault))]
        DownloadFileInfo DownloadMediaThumbnail(DownloadRequest request);

        [OperationContract]
        [FaultContract(typeof(UserFault))]
        [FaultContract(typeof(MediaFault))]
        ICollection<string> GetAllUserMediaFilesName(string userName);

        [OperationContract]
        [FaultContract(typeof(UserFault))]
        [FaultContract(typeof(MediaFault))]
        ICollection<MediaMetadata> GetAllUserMediaMetadata(string userName);

        [OperationContract]
        [FaultContract(typeof(UserFault))]
        [FaultContract(typeof(MediaFault))]
        MediaMetadata GetMediaMetadata(string userName, string mediaFileName);

        [OperationContract]
        [FaultContract(typeof(UserFault))]
        [FaultContract(typeof(MediaFault))]
        void UpdateMediaMetadata(MediaMetadata mediaMetadata);

        [OperationContract]
        [FaultContract(typeof(UserFault))]
        [FaultContract(typeof(MediaFault))]
        void UploadMedia(UploadMediaFileInfo file);

        [OperationContract]
        [FaultContract(typeof(UserFault))]
        [FaultContract(typeof(MediaFault))]
        void UploadMediaThumbnail(UploadThumbFileInfo file);
    }

    [ServiceContract]
    public interface IUserService {
        [OperationContract]
        [FaultContract(typeof(UserFault))]
        void LoginUser(string userName, string password);

        [OperationContract]
        [FaultContract(typeof(UserFault))]
        void RegisterUser(User user);
    }

}