using CleanCodeScaffold.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Handlers.Interface
{
    public interface IUserHandler
    {
        Task<Response<TokenVM>> GetToken(LoginVM Model);
        Task<Response<TokenVM>> GetTokenByRefresh(string refreshToken);
        Task<Response<string>> Register(RegisterVM model);
        Task<Response<string>> ChangePassword(long userId, ChangePasswordVM model);
        Task<Response<string>> GetForgotPasswordToken(string emailorPhone);
        Task<Response<string>> ResetPassword(ResetPasswordVM model);
    }
}
