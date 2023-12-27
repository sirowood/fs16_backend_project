using AutoMapper;

using Shopify.Service.src.Shared;

namespace Shopify.Test.Shared;

public static class MapperHelper
{
  public static IMapper GetMapper()
  {
    MapperConfiguration mappingConfig = new(m =>
    {
      m.AddProfile(new MapperProfile());
    });

    IMapper mapper = mappingConfig.CreateMapper();

    return mapper;
  }
}
