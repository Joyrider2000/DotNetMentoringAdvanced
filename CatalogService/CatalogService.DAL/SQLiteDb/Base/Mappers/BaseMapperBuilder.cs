using AutoMapper;

namespace CatalogService.DAL.SQLiteDb.Base.Mappers
{
    public class BaseMapperBuilder
    {
        protected static void CreateTwoWayMap<TSource, TDest>(IMapperConfigurationExpression config)
        {
            CreateTwoWayMap(config, new Dictionary<string, Action<IMemberConfigurationExpression<TSource, TDest, object>>>(),
                new Dictionary<string, Action<IMemberConfigurationExpression<TDest, TSource, object>>>());
        }
        protected static void CreateTwoWayMap<TSource, TDest>(
            IMapperConfigurationExpression config,
            IDictionary<string, Action<IMemberConfigurationExpression<TSource, TDest, object>>> memberOptionsFwd,
            IDictionary<string, Action<IMemberConfigurationExpression<TDest, TSource, object>>> memberOptionsBwd)
        {
            var expressionFwd = config.CreateMap<TSource, TDest>();
            foreach (var memberOption in memberOptionsFwd)
            {
                expressionFwd.ForMember(memberOption.Key, memberOption.Value);
            }

            var expressionBwd = config.CreateMap<TDest, TSource>();
            foreach (var memberOption in memberOptionsBwd)
            {
                expressionBwd.ForMember(memberOption.Key, memberOption.Value);
            }
        }
    }
}
