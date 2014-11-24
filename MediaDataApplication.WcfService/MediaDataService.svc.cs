using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.ServiceModel;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DataContracts;
using MediaDataApplication.WcfService.Logic;
using MediaDataApplication.WcfService.Mappers;
using MediaDataApplication.WcfService.MessageContracts;
using NLog;

namespace MediaDataApplication.WcfService {

    public class MediaDataService : IMediaDataService {
        private readonly CommonMapper mapper = new CommonMapper();
        private readonly MediaLogic mediaLogic = new MediaLogic();
        private readonly UserLogic userLogic = new UserLogic();

        #region IMediaDataService Members

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public void LoginUser(string userName, string password) {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) {
                throw new FaultException<UserFault>(new UserFault("UserName and Password are required"),
                                                    "LoginUser Empty User");
            }

            try {
                this.userLogic.LoginUser(userName, password);
            }
            catch (Exception e) {
                throw new FaultException<UserFault>(new UserFault(e.Message), "LoginUser Exception");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public void RegisterUser(User user) {
            if (user == null) {
                throw new ArgumentNullException("user", "RegisterUser null User");
            }
            user.Validate("RegisterUser");

            try {
                var newUserBDO = this.mapper.Map<User, UserBDO>(user);
                this.userLogic.RegisterUser(newUserBDO);
            }
            catch (Exception e) {
                string msg = e.Message;
                throw new FaultException<UserFault>(new UserFault(msg), "RegisterUser Exception");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public DownloadFileInfo DownloadMedia(DownloadChunkRequest request) {
            if (request == null) {
                throw new ArgumentNullException("request", "DownloadMedia null DownloadRequest");
            }
            request.Validate("DownloadMedia");

            var result = new DownloadFileInfo();
            try {
                var chunkFileBDO = this.mapper.Map<DownloadChunkRequest, FileChunkBDO>(request);
                result.FileByteStream = this.mediaLogic.GetMedia(request.UserName, chunkFileBDO);
                result.FileName = request.FileName;
            }
            catch (Exception e) {
                throw new FaultException<MediaFault>(new MediaFault(e.Message), "DownloadMedia Exception");
            }

            return result;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public DownloadFileInfo DownloadMediaThumbnail(DownloadRequest request) {
            if (request == null) {
                throw new ArgumentNullException("request", "DownloadMediaThumbnail null DownloadRequest");
            }
            request.Validate("DownloadMediaThumbnail");

            var result = new DownloadFileInfo();
            try {
                result.FileByteStream = this.mediaLogic.GetMediaThumbnail(request.UserName, request.FileName);
                result.FileName = request.FileName;
            }
            catch (Exception e) {
                throw new FaultException<MediaFault>(new MediaFault(e.Message), "DownloadMediaThumbnail Exception");
            }

            return result;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public ICollection<MediaMetadata> GetAllUserMediaMetadata(string userName) {
            if (string.IsNullOrEmpty(userName)) {
                throw new FaultException<UserFault>(new UserFault("Username is required"),
                                                    "GetAllUserMediaMetadata Empty Username");
            }

            try {
                var allMediaMetadata = this.mediaLogic.GetAllUserMediaMetadata(userName);
                return this.mapper.Map<ICollection<MediaMetadataBDO>, ICollection<MediaMetadata>>(allMediaMetadata);
            }
            catch (Exception e) {
                throw new FaultException<MediaFault>(new MediaFault(e.Message), "GetAllUserMediaMetadata Exception");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public ICollection<string> GetAllUserMediaFilesName(string userName) {
            if (string.IsNullOrEmpty(userName)) {
                throw new FaultException<UserFault>(new UserFault("Username is required"),
                                                    "GetAllUserMediaFilesName Empty Username");
            }

            try {
                return this.mediaLogic.GetAllUserMediaFilesName(userName);
            }
            catch (Exception e) {
                throw new FaultException<MediaFault>(new MediaFault(e.Message), "GetAllUserMediaFilesName Exception");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public void UploadMediaThumbnail(UploadThumbFileInfo uploadFileInfo) {
            if (uploadFileInfo == null) {
                throw new ArgumentNullException("uploadFileInfo", "UploadMediaThumbnail null UploadFileInfo");
            }
            uploadFileInfo.Validate("UploadMedia");

            try {
                this.mediaLogic.UploadMediaThumbnail(uploadFileInfo.UserName,
                                                     uploadFileInfo.MediaFileName,
                                                     uploadFileInfo.FileName,
                                                     uploadFileInfo.FileByteStream);
            }
            catch (Exception e) {
                throw new FaultException<MediaFault>(new MediaFault(e.Message), "UploadMediaThumbnail Exception");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public void DeleteMedia(string userName, string[] mediaFilesName) {
            if (mediaFilesName == null) {
                throw new ArgumentNullException("mediaFilesName", "DeleteMedia null MediaFilesName");
            }
            if (string.IsNullOrWhiteSpace(userName)) {
                throw new FaultException("DeleteMedia Empty UserName");
            }

            try {
                this.mediaLogic.DeleteMedia(userName, mediaFilesName);
            }
            catch (Exception e) {
                throw new FaultException<MediaFault>(new MediaFault(e.Message), "DeleteMedia Exception");
            }
        }

        public void UpdateMediaMetadata(MediaMetadata mediaMetadata) {
            mediaMetadata.Validate("UpdateMediaMetadata");

            try {
                var mediaMetadataBDO = this.mapper.Map<MediaMetadata, MediaMetadataBDO>(mediaMetadata);
                this.mediaLogic.UpdateMediaMetadata(mediaMetadataBDO);
            }
            catch (Exception e) {
                throw new FaultException<MediaFault>(new MediaFault(e.Message), "UpdateMediaMetadata Exception");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public void UploadMedia(UploadMediaFileInfo uploadFileInfo) {
            if (uploadFileInfo == null) {
                throw new ArgumentNullException("uploadFileInfo", "UploadMedia null UploadFileInfo");
            }
            uploadFileInfo.Validate("UploadMedia");

            try {
                this.mediaLogic.UploadMedia(uploadFileInfo.UserName, uploadFileInfo.FileName, uploadFileInfo.FileByteStream);
            }
            catch (Exception e) {
                LogManager.GetCurrentClassLogger().Error(e);
                throw new FaultException<MediaFault>(new MediaFault(e.Message), "UploadMedia Exception");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Member")]
        public MediaMetadata GetMediaMetadata(string userName, string mediaFileName) {
            if (string.IsNullOrEmpty(userName)) {
                throw new FaultException<UserFault>(new UserFault("UserName is required"),
                                                    "GetMediaMetadata Empty UserName");
            }
            if (string.IsNullOrWhiteSpace(mediaFileName)) {
                throw new FaultException<MediaFault>(new MediaFault("MediaFilename is required"),
                                                     "GetMediaMetadata Empty MediaFileName");
            }

            try {
                var mediaMetadataBDO = this.mediaLogic.GetMediaMetadata(userName, mediaFileName);
                return this.mapper.Map<MediaMetadataBDO, MediaMetadata>(mediaMetadataBDO);
            }
            catch (Exception e) {
                throw new FaultException<MediaFault>(new MediaFault(e.Message), "GetMediaMetadata Exception");
            }
        }

        #endregion
    }

}