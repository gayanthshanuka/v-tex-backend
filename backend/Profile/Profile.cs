using AutoMapper;
using backend.Dtos;
using backend.products;

namespace backend.Profiles{
    public class APIProfile:Profile
    {
        public APIProfile(){
           CreateMap<User,UserDto>();
           CreateMap<UserDto,User>();

           CreateMap<Products,ProductsReadDto>();
           CreateMap<productCreateDto,Products>();
           CreateMap<productUpdateDto,Products>();
           CreateMap<Products,productUpdateDto>();

           CreateMap<cart,cartReadDto>();
           CreateMap<cartCreateDto,cart>();
           CreateMap<cartUpdateDto,cart>();
           CreateMap<cart,cartUpdateDto>();

           CreateMap<cart_item,cart_itemReadDto>();
           CreateMap<cart_itemCreateDto,cart_item>();
           CreateMap<cart_itemUpdateDto,cart_item>();
           CreateMap<cart_item,cart_itemUpdateDto>();

           
        }
    }
}