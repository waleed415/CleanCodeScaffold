using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Dtos.Configs;
using CleanCodeScaffold.Application.Handlers.Interface;
using CleanCodeScaffold.Application.Util;
using CleanCodeScaffold.Application.Validators;
using CleanCodeScaffold.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace CleanCodeScaffold.Application.Handlers.Implimentation
{
    public class UserHandler : IUserHandler
    {
        private readonly UserManager<User> _userManager;
        private readonly JWTConfigs _jwtConfigs;
        private readonly IValidator<LoginVM> _loginValidator;
        private readonly IValidator<RegisterVM> _registerVmValidator;
        private readonly IValidator<ChangePasswordVM> _changePasswordVmValidator;
        private readonly IValidator<ResetPasswordVM> _resetPasswordVMValidator;
        public UserHandler(UserManager<User> userManager, IOptions<JWTConfigs> options,
            IValidator<LoginVM> loginValidator, IValidator<RegisterVM> registerVmValidator,
            IValidator<ChangePasswordVM> changePasswordVmValidator, IValidator<ResetPasswordVM> resetPasswordVMValidator)
        {
            _userManager = userManager;
            _jwtConfigs = options.Value;
            _loginValidator = loginValidator;
            _registerVmValidator = registerVmValidator;
            _changePasswordVmValidator = changePasswordVmValidator;
            _resetPasswordVMValidator = resetPasswordVMValidator;
        }
        public async Task<Response<TokenVM>> GetToken(LoginVM Model)
        {
            Response<TokenVM> response = new Response<TokenVM> { Status = "Error" };
            var validationResult = await _loginValidator.ValidateAsync(Model);
            if (!validationResult.IsValid)
                response.Message = validationResult.ToErrorMessage();
            else
            {
                var user = await _userManager.FindByNameAsync(Model.UserName);
                if (user == null)
                    response.Message.Add("Invalid User name or password");
                else if (!await _userManager.CheckPasswordAsync(user, Model.Password))
                    response.Message.Add("Invalid User name or password");
                else
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    response.Status = "Success";
                    response.Data = new TokenVM
                    {
                        Name = user.UserName,
                        Roles = roles.ToList(),
                        Token = GenerateToken(user, roles.ToList()),
                        RefreshToken = await GetRefreshToken(user),
                    };
                }
            }
            return response;
        }

        public async Task<Response<string>> Register(RegisterVM model)
        {
            Response<string> response = new Response<string> { Status = "Error" };
            var validatorResult = await _registerVmValidator.ValidateAsync(model);
            if (!validatorResult.IsValid)
                response.Message = validatorResult.ToErrorMessage();
            else
            {
                var user = new User { UserName = model.UserName, Email = model.Email };
                var userResult = await _userManager.CreateAsync(user, model.Password);
                if (userResult.Succeeded)
                {
                    response.Status = "Success";
                    response.Data = "User Created Successfully!";
                }
                else
                {
                    response.Message = userResult.Errors.Select(x => x.Description).ToList();
                }

            }
            return response;
        }

        public async Task<Response<TokenVM>> GetTokenByRefresh(string refreshToken, long userId)
        {
            Response<TokenVM> response = new Response<TokenVM> { Status = "Error" };
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                response.Message.Add("Invalid user!");
            else if (!user.RefreshToken.Equals(refreshToken))
                response.Message.Add("Invalid refresh token!");
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                response.Status = "Success";
                response.Data = new TokenVM
                {
                    Name = user.UserName,
                    Roles = roles.ToList(),
                    Token = GenerateToken(user, roles.ToList()),
                    RefreshToken = await GetRefreshToken(user),
                };
            }
            return response;
        }

        public async Task<Response<string>> ChangePassword(long userId, ChangePasswordVM model)
        {
            Response<string> response = new Response<string> { Status = "Error" };
            var validationResult = await _changePasswordVmValidator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                    response.Message.Add("Invalid User!");
                else
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        response.Status = "Success";
                        response.Message.Add("Password changed successfully!");
                        response.Data = "Password changed successfully!";
                    }
                    else
                        response.Message = result.Errors.Select(x => x.Description).ToList();
                }
            }
            else
                response.Message = validationResult.ToErrorMessage();
            return response;
        }

        public async Task<Response<string>> GetForgotPasswordToken(string emailorPhone)
        {
            Response<string> response = new Response<string> { Status = "Error" };
            var user = await _userManager.FindByEmailAsync(emailorPhone);
            if (user == null)
                user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == emailorPhone);
            if (user == null)
                response.Message.Add("User not found!");
            else
            {
                await _userManager.GeneratePasswordResetTokenAsync(user);
                response.Status = "Success";
                response.Message.Add("Token genrated successfully");
                response.Data = "Token genrated successfully";
            }
            return response;
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordVM model)
        {
            Response<string> response = new Response<string> { Status = "Error" };
            var validationResult = await _resetPasswordVMValidator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailorPhone);
                if (user == null)
                    user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.EmailorPhone);
                if (user == null)
                    response.Message.Add("User not found.");
                else
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        response.Status = "Success";
                        response.Message.Add("Password reset successfully.");
                    }
                    else
                        response.Message = result.Errors.Select(x => x.Description).ToList();
                }
            }
            return response;
        }

        private async Task<string> GetRefreshToken(User user)
        {
            string token = Guid.NewGuid().ToString() + user.Id + DateTime.UtcNow.ToString("dd-MM-yy-H-m-ss");
            user.RefreshToken = token;
            await _userManager.UpdateAsync(user);
            return token;
        }

        private string GenerateToken(User user, List<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("id", user.Id.ToString())
            };

            foreach (var userRole in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigs.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfigs.Issuer,
                audience: _jwtConfigs.Audience,
                claims: authClaims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }


    }
}
