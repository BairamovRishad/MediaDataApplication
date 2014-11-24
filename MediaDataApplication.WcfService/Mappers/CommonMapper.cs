using System;
using AutoMapper;

namespace MediaDataApplication.WcfService.Mappers {

    public class CommonMapper {
        static CommonMapper() {
            MapperCollection.UserMapper.Init();
            MapperCollection.MediaMetadataMapper.Init();
            MapperCollection.ChunkFileMapper.Init();
        }

        public object Map(object source, Type sourceType, Type destinationType) {
            return Mapper.Map(source, sourceType, destinationType);
        }

        public TDestination Map<TSource, TDestination>(TSource source) {
            return Mapper.Map<TSource, TDestination>(source);
        }
    }

}