using AutoMapper;
using MediaDataApplication.WcfService.BDO;
using MediaDataApplication.WcfService.DataContracts;

namespace MediaDataApplication.WcfService.Mappers {

    public class MapperCollection {
        #region Nested type: ChunkFileMapper

        public static class ChunkFileMapper {
            public static void Init() {
                Mapper.CreateMap<DownloadChunkRequest, FileChunkBDO>();
            }
        }

        #endregion

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