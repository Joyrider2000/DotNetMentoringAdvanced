using AutoMapper;
using System.Data.SqlTypes;

namespace CartingService.BLL.Mappers.Builders
{
    public class BaseMapperBuilder
    {
        protected BaseMapperBuilder() { }

        protected static void CreateTwoWayMap<TSource, TDest>(IMapperConfigurationExpression config)
        {
            config.CreateMap<TSource, TDest>();
            config.CreateMap<TDest, TSource>();
        }
    }
}
