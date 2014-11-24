using System;

namespace MediaDataApplication.WcfService.DAL.Mappers {

    public interface IMapper {
        object Map(object source, Type sourceType, Type destinationType);

        TDestination Map<TSource, TDestination>(TSource source);
    }

}