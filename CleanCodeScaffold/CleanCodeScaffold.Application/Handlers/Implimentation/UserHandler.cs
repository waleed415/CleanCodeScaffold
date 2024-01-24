using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Dtos.Configs;
using CleanCodeScaffold.Application.Handlers.Interface;
using CleanCodeScaffold.Application.Util;
using CleanCodeScaffold.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
        public UserHandler(UserManager<User> userManager, IOptions<JWTConfigs> options, 
            IValidator<LoginVM> loginValidator, IValidator<RegisterVM> registerVmValidator)
        {
            _userManager = userManager;
            _jwtConfigs = options.Value;
            _loginValidator = loginValidator;
            _registerVmValidator = registerVmValidator;
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
            Response<string> response = new Response<string> {  Status = "Error" };
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

        public Task<Response<TokenVM>> GetTokenByRefresh(string refreshToken, int userId)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GetRefreshToken(User user)
        {
            string token = Guid.NewGuid().ToString()+user.Id+DateTime.UtcNow.ToString("dd-MM-yy-H-m-ss");
            user.RefreshToken = token;
            await _userManager.UpdateAsync(user);
            return token;
        }

        private string GenerateToken(User user, List<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfigs.Key);

            var tokenDescriptor = new JwtSecurityToken
            (
                claims: authClaims,
                issuer: _jwtConfigs.Issuer,
                audience: _jwtConfigs.Audience,
                expires: DateTime.UtcNow.AddHours(1), // Adjust expiration as needed
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
             );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
