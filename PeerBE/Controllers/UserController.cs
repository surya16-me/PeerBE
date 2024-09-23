using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services;
using DAL.Repositories.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Internal.Postgres;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PeerBE.Controllers
{
    [Route("rest/v1/user/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Register(ReqRegisterUserDto register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select( x => new
                        {
                            Field = x.Key,
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        }).ToList();
                    var errorMessage = new StringBuilder("Validation errors occured");

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                }

                var res = await _userService.Register(register);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = res
                });
            }catch (Exception ex)
            {
                if(ex.Message == "Email already used")
                {
                    return BadRequest(new ResBaseDto<string>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(ReqLoginUserDto login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new
                        {
                            Field = x.Key,
                            Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        })
                        .ToList();

                    var errorMessage = new StringBuilder("Validation errors occurred");

                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                }

                var res = await _userService.Login(login);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User login success",
                    Data = res
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid email or password")
                {
                    return BadRequest(new ResBaseDto<string>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
    }
}
