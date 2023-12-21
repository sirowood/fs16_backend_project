using AutoMapper;

using Shopify.Core.src.Entity;
using Shopify.Service.src.DTO;

namespace Shopify.Service.src.Shared;

public class MapperProfile : Profile
{
  public MapperProfile()
  {
    CreateMap<User, UserReadDTO>();
    CreateMap<UserCreateDTO, User>();
    CreateMap<UserUpdateDTO, User>();

    CreateMap<Address, AddressReadDTO>();
    CreateMap<AddressCreateDTO, Address>();
    CreateMap<AddressUpdateDTO, Address>();

    CreateMap<Category, CategoryReadDTO>();
    CreateMap<CategoryCreateDTO, Category>();
    CreateMap<CategoryUpdateDTO, Category>();

    CreateMap<Product, ProductReadDTO>();
    CreateMap<ProductCreateDTO, Product>();
    CreateMap<ProductUpdateDTO, Product>();

    CreateMap<Image, ImageReadDTO>();
    CreateMap<ImageCreateDTO, Image>();
    CreateMap<ImageUpdateDTO, Image>();
  }
}