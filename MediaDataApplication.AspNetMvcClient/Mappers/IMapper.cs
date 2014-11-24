using System;

namespace MediaDataApplication.AspNetMvcClient.Mappers {

    public interface IMapper {
        object Map(object source, Type sourceType, Type destinationType);

        TDestination Map<TSource, TDestination>(TSource source);
    }

}