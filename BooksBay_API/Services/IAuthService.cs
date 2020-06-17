using Taskboard_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskboard_API.Services
{
	public interface IAuthService
	{
		Task<Users> Login(string email, string password);
		Task<string> GenerateJWTToken(Users authenticatedUser);
		void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
	}
}
