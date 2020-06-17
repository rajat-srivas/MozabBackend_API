using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Taskboard_API.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using BooksBay_API.Helpers;

namespace Taskboard_API.Services
{
	public class AuthService: IAuthService
	{
		IMongoCollection<Users> users;

		public static string secretKey = "My super secret key used for encoding the token";
		public AuthService()
		{
			//var client = new MongoClient("mongodb://127.0.0.1:27017");
			var client = new MongoClient(ConfigConstants.MongoDBClient);
			var database = client.GetDatabase("Mozab");
			users = database.GetCollection<Users>("Users");
		}
		public async Task<Users> Login(string email, string password)
		{
			var userFromDB = await users.Find(x => x.Email == email).FirstOrDefaultAsync();
			if (userFromDB == null) return null;

			if (!VerifyPassword(password, userFromDB.PasswordHash, userFromDB.Salt))
				return null;

			return userFromDB;
		}

		public async Task<string> GenerateJWTToken(Users authenticatedUser)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, authenticatedUser.Id),
				new Claim(ClaimTypes.Email, authenticatedUser.Email),
				new Claim(ClaimTypes.Name, $"{authenticatedUser.Name}")

			};

			//hashed key for the token
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

			//create the signing credential using the key
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			//create the token sescriptor which contains the token header and everything
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = System.DateTime.Now.AddDays(1),
				SigningCredentials = creds
			};

			// use the token handler to create the token
			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);

		}
		private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				for (int i = 0; i < computedHash.Length; i++)
				{
					if (computedHash[i] != passwordHash[i])
						return false;
				}
			}

			return true;
		}

		public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}
	}
}