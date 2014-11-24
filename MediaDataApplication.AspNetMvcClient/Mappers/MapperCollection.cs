using AutoMapper;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using MediaDataApplication.AspNetMvcClient.Models;

namespace MediaDataApplication.AspNetMvcClient.Mappers {

    public class MapperCollection {
        #region Nested type: MediaMetadataMapper

        public static class MediaMetadataMapper {
            public static void Init() {
                Mapper.CreateMap<MediaMetadata, MediaMetadataViewModel>().ForMember(x => x.MediaFileName, y => y.MapFrom(src => src.FileName));
                Mapper.CreateMap<MediaMetadataViewModel, MediaMetadata>();
            }
        }

        #endregion

        #region Nested type: UserMapper

        public static class UserMapper {
            public static void Init() {
                Mapper.CreateMap<User, RegisterViewModel>();
                Mapper.CreateMap<RegisterViewModel, User>();
            }
        }

        #endregion
    }

}