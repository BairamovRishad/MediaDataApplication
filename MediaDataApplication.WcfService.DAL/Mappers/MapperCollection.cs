using AutoMapper;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DAL.Entities;

namespace MediaDataApplication.WcfService.DAL.Mappers {

    public class MapperCollection {
        #region Nested type: MediaMetadataMapper

        public static class MediaMetadataMapper {
            public static void Init() {
                Mapper.CreateMap<MediaMetadata, MediaMetadataBDO>();
                Mapper.CreateMap<MediaMetadataBDO, MediaMetadata>();
            }
        }

        #endregion

        #region Nested type: UserMapper

        public static class UserMapper {
            public static void Init() {
                Mapper.CreateMap<User, UserBDO>();
                Mapper.CreateMap<UserBDO, User>();
            }
        }

        #endregion
    }

}