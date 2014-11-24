using System;
using System.ServiceModel;
using System.Threading.Tasks;
using MediaDataApplication.AspNetMvcClient.Helpers;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using NLog;

namespace MediaDataApplication.AspNetMvcClient.Models.MediaManager {

    // Remarque: It's 'Manipulator' because implements data manipulations (as in DMLs). 
    public class MediaManipulator : MediaDataProcessor {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public MediaManipulator(string userName) : base(userName) { }

        public async Task DeleteMediaAsync(string[] filesName) {
            try {
                await MediaDataService.DeleteMediaAsync(UserName, filesName);
            }
            catch (Exception e) {
                new ServiceExceptionHandler(_logger).Handle(e);
                throw new ServiceActivationException();
            }
        }

        public MediaMetadata[] GetAllUserMediaMetadata() {
            try {
                return MediaDataService.GetAllUserMediaMetadata(UserName);
            }
            catch (Exception e) {
                new ServiceExceptionHandler(_logger).Handle(e);
                throw new ServiceActivationException();
            }
        }

        public MediaMetadata GetMediaMetadata(string mediaFileName) {
            try {
                return MediaDataService.GetMediaMetadata(UserName, mediaFileName);
            }
            catch (Exception e) {
                new ServiceExceptionHandler(_logger).Handle(e);
                throw new ServiceActivationException();
            }
        }

        public async Task UpdateMediaMetadata(MediaMetadata mediaMetadata) {
            try {
                await MediaDataService.UpdateMediaMetadataAsync(mediaMetadata);
            }
            catch (Exception e) {
                new ServiceExceptionHandler(_logger).Handle(e);
                throw new ServiceActivationException();
            }
        }
    }

}