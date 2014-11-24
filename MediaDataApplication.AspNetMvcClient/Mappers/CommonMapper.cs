using System;
using AutoMapper;

namespace MediaDataApplication.AspNetMvcClient.Mappers {

    public class CommonMapper : IMapper {
        static CommonMapper() {
            MapperCollection.UserMapper.Init();
            MapperCollection.MediaMetadataMapper.Init();
        }

        #region IMapper Members

        public object Map(object source, Type sourceType, Type destinationType) {
            return Mapper.Map(source, sourceType, destinationType);
        }

        public TDestination Map<TSource, TDestination>(TSource source) {
            return Mapper.Map<TSource, TDestination>(source);
        }

        #endregion
    }

}