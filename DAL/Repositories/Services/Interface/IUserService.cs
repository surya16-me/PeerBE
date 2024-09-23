using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interface
{
    public interface IUserService
    {
        Task<ResLoginUserDto> Login(ReqLoginUserDto reqLoginUser);
        Task<string> Register(ReqRegisterUserDto reqRegisterUser);

    }
}
