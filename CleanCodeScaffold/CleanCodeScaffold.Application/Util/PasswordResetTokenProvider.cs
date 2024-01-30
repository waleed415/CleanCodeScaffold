using CleanCodeScaffold.Domain.Entities;
using CleanCodeScaffold.Infrastructure.Persistence;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Util
{
    public class PasswordResetTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : User
    {
        private readonly AppDBContext _db;
        public DataProtectionTokenProviderOptions Options { get; }
        public IDataProtector Protector { get; }
        public string Name => Options.Name;
        private const string AllowedChars = "0123456789";
        private readonly Random _random = new Random();

        public PasswordResetTokenProvider(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<PasswordResetTokenProviderOptions> options,
            AppDBContext db)
        {
            if (dataProtectionProvider == null)
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            Options = options?.Value ?? new DataProtectionTokenProviderOptions();
            Protector = dataProtectionProvider.CreateProtector(Name ?? "DataProtectorTokenProvider");

            _db = db;
        }
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            return await GetTokenAsync(purpose, user.Id);
        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            bool response = false;
            var tokenModel = await _db.UserTokens.FirstOrDefaultAsync(x => x.UserId == user.Id && x.Name == purpose);
            if (tokenModel == null)
                response = false;
            else if (tokenModel.Value == ConvertToHash(token) && tokenModel.ExpiredDateTimeUTC >= DateTime.UtcNow)
            {
                _db.UserTokens.Remove(tokenModel);
                response = true;
            }
            else
            {
                tokenModel.TokenTries++;
                if (tokenModel.TokenTries > 2)
                    _db.UserTokens.Remove(tokenModel);
                response = false;
            }
            await _db.SaveChangesAsync();
            return response;
        }

        private async Task<string> GetTokenAsync(string type, long userId)
        {
            char[] chars = new char[6];
            for (int i = 0; i < 6; i++)
            {
                chars[i] = AllowedChars[_random.Next(0, AllowedChars.Length)];
            }
            var token = new string(chars);
            var oldToken = await _db.UserTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.Name == type);
            if (oldToken != null)
                _db.UserTokens.Remove(oldToken);

            await _db.UserTokens.AddAsync(new UserToken
            {
                Name = type,
                UserId = userId,
                Value = ConvertToHash(token),
                LoginProvider = "local",
                ExpiredDateTimeUTC = DateTime.UtcNow.Add(Options.TokenLifespan),
            });
            await _db.SaveChangesAsync();
            return token;
        }

        private string ConvertToHash(string value)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
    public class PasswordResetTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public PasswordResetTokenProviderOptions()
        {
            TokenLifespan = TimeSpan.FromMinutes(30);
            TokenTries = 3;
        }
        public int TokenTries { get; set; }
    }
}
