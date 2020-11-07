using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using backend.Data;
using backend.Dtos;
using backend.products;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace backend.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class productController: ControllerBase{
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;


        public productController(ICommanderRepo repository,IMapper mapper)
        {
            _repository =repository;
            _mapper = mapper;

        }

        [HttpGet]
        public ActionResult <IEnumerable<ProductsReadDto>> GetAllProducts()
        {
            var allItems =_repository.GetAllProducts();
            return Ok(_mapper.Map<IEnumerable<ProductsReadDto>>(allItems));
        }
        [HttpGet("{id}",Name="GetProductsId1")]
       public ActionResult <ProductsReadDto> GetProductsId1(int id)
       {
             var commandItem = _repository.GetProductsId(id);
             if(commandItem != null){
             return Ok(_mapper.Map<ProductsReadDto>(commandItem));}
             return NotFound();

       }

       //POST api/user

        [HttpPost]
        public ActionResult<ProductsReadDto> CreateCommand(productCreateDto ProductCreate)
        {
            var  ProductModel = _mapper.Map<Products>(ProductCreate);
            _repository.CreateProduct(ProductModel);
            _repository.SaveChanages();
            var CommandReadDto = _mapper.Map<ProductsReadDto>(ProductModel);
            return CreatedAtRoute(nameof(GetProductsId1),new {Id=CommandReadDto.ProductId},CommandReadDto);
             
        }

        //PUT api/user/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id,productUpdateDto ProductUpdate)
        {
            var ProductModelFromRepo = _repository.GetProductsId(id);
            if(ProductModelFromRepo==null)
            {
                return NotFound();
            }
            _mapper.Map(ProductUpdate,ProductModelFromRepo);
            _repository.UpdateProduct(ProductModelFromRepo);
            _repository.SaveChanages(); 
            return NoContent();
        }
        //PATCH api/user/{id}
        [HttpPatch("{id}")]
        public ActionResult  PartialCommandUpdate(int id,JsonPatchDocument<productUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _repository.GetProductsId(id);
            if(commandModelFromRepo==null)
            {
                return NotFound();
            }
            var commandToPatch = _mapper.Map<productUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch,ModelState);
            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(commandToPatch,commandModelFromRepo);
            _repository.UpdateProduct(commandModelFromRepo);
            _repository.SaveChanages(); 
            return NoContent();


        }
       // DELETE api/user/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetProductsId(id);
            if(commandModelFromRepo==null)
            {
                return NotFound();
            }
            _repository.DeleteProduct(commandModelFromRepo);
            _repository.SaveChanages();
            return NoContent();
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Route("api/Users")]
    [ApiController]

     public class UserController:ODataController{
       
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.UserName, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new {
                Id = user.UserId,
                Username = user.UserName,
                FirstName = user.first_name,
                LastName = user.last_name,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try 
            {
                // save 
                _userService.Create(user, userDto.Password);
                return Ok();
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
       [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users =  _userService.GetAll();
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }
[AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user =  _userService.GetById(id);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userDto);
            user.UserId = id;

            try 
            {
                // save 
                _userService.Update(user, userDto.Password);
                return Ok();
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   [Route("api/cart")]
    [ApiController]
    public class cartController: ControllerBase{
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;
        public cartController(ICommanderRepo repository,IMapper mapper)
        {
            _repository =repository;
            _mapper = mapper;
        }
        [HttpGet]
        [EnableQuery()]
        public ActionResult <IEnumerable<cartReadDto>> GetAllCart()
        {
            var allItems =_repository.GetAllCarts();
            return Ok(_mapper.Map<IEnumerable<cartReadDto>>(allItems));
        }
        [HttpGet("{id}",Name="GetCartId")]
       public ActionResult <cartReadDto> GetCartId(int id)
       {
             var commandItem = _repository.GetCartId(id);
             if(commandItem != null){
             return Ok(_mapper.Map<cartReadDto>(commandItem));}
             return NotFound();

       }

       //POST api/user

        [HttpPost]
        public ActionResult<cartReadDto> CreateCommand(cartCreateDto ProductCreate)
        {
            var  ProductModel = _mapper.Map<cart>(ProductCreate);
            _repository.CreateCart(ProductModel);
            _repository.SaveChanages();
            var CommandReadDto = _mapper.Map<cartReadDto>(ProductModel);
            return CreatedAtRoute(nameof(GetCartId),new {Id=CommandReadDto.CartId},CommandReadDto);
             
        }

        //PUT api/user/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id,cartUpdateDto ProductUpdate)
        {
            var ProductModelFromRepo = _repository.GetCartId(id);
            if(ProductModelFromRepo==null)
            {
                return NotFound();
            }
            _mapper.Map(ProductUpdate,ProductModelFromRepo);
            _repository.UpdateCart(ProductModelFromRepo);
            _repository.SaveChanages(); 
            return NoContent();
        }
        //PATCH api/user/{id}
        [HttpPatch("{id}")]
        public ActionResult  PartialCommandUpdate(int id,JsonPatchDocument<cartUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _repository.GetCartId(id);
            if(commandModelFromRepo==null)
            {
                return NotFound();
            }
            var commandToPatch = _mapper.Map<cartUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch,ModelState);
            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(commandToPatch,commandModelFromRepo);
            _repository.UpdateCart(commandModelFromRepo);
            _repository.SaveChanages(); 
            return NoContent();


        }
       // DELETE api/user/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCartId(id);
            if(commandModelFromRepo==null)
            {
                return NotFound();
            }
            _repository.DeleteCart(commandModelFromRepo);
            _repository.SaveChanages();
            return NoContent();
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [Route("api/cartItem")]
    [ApiController]
    public class cartItemController: ControllerBase{
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;
        public cartItemController(ICommanderRepo repository,IMapper mapper)
        {
            _repository =repository;
            _mapper = mapper;
        }
        [HttpGet]
        [EnableQuery()]
        public ActionResult <IEnumerable<cart_itemReadDto>> GetAllCart()
        {
            var allItems =_repository.GetAllCartItem();
            return Ok(_mapper.Map<IEnumerable<cart_itemReadDto>>(allItems));
        }
        [HttpGet("{id}",Name="GetCartItem")]
       public ActionResult <IEnumerable<cart_itemReadDto>> GetCartItem(int id)
       {
             var commandItem = _repository.GetCartItemId(id);
             if(commandItem != null){
             return Ok(_mapper.Map<IEnumerable<cart_itemReadDto>>(commandItem));}
             return NotFound();

       }

       //POST api/user

        [HttpPost]
        public ActionResult<cart_itemReadDto> CreateCommand(cart_itemCreateDto ProductCreate)
        {
            var  ProductModel = _mapper.Map<cart_item>(ProductCreate);
            _repository.CreateCartItem(ProductModel);
            _repository.SaveChanages();
            var CommandReadDto = _mapper.Map<cart_itemReadDto>(ProductModel);
            return CreatedAtRoute(nameof(GetCartItem),new {Id=CommandReadDto.cart_item_id},CommandReadDto);
             
        }

        //PUT api/user/{id}
        // [HttpPut("{id}")]
        // public ActionResult UpdateCommand(int id,cart_itemUpdateDto ProductUpdate)
        // {
        //     var ProductModelFromRepo = _repository.GetCartItemId(id);
        //     if(ProductModelFromRepo==null)
        //     {
        //         return NotFound();
        //     }
        //     _mapper.Map(ProductUpdate,ProductModelFromRepo);
        //     _repository.UpdateCartItem(ProductModelFromRepo);
        //     _repository.SaveChanages(); 
        //     return NoContent();
        // }
        //PATCH api/user/{id}
        // [HttpPatch("{id}")]
        // public ActionResult  PartialCommandUpdate(int id,JsonPatchDocument<cartUpdateDto> patchDoc)
        // {
        //     var commandModelFromRepo = _repository.GetCartId(id);
        //     if(commandModelFromRepo==null)
        //     {
        //         return NotFound();
        //     }
        //     var commandToPatch = _mapper.Map<cartUpdateDto>(commandModelFromRepo);
        //     patchDoc.ApplyTo(commandToPatch,ModelState);
        //     if(!TryValidateModel(commandToPatch))
        //     {
        //         return ValidationProblem(ModelState);
        //     }
        //     _mapper.Map(commandToPatch,commandModelFromRepo);
        //     _repository.UpdateCart(commandModelFromRepo);
        //     _repository.SaveChanages(); 
        //     return NoContent();


        // }
       // DELETE api/user/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCart_ItemId(id);
            if(commandModelFromRepo==null)
            {
                return NotFound();
            }
            _repository.DeleteCartItem(commandModelFromRepo);
            _repository.SaveChanages();
            return NoContent();
        }
    }
}
