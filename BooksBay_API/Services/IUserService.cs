using Taskboard_API.Models;
using Taskboard_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskboard_API.Services
{
	public interface IUserService
	{
		Task<bool> CheckEmailExist(string email);

		Task<List<User_DTO>> GetAllUsers();

		Task<User_DTO> GetUserById(string id);

		Task<string> CreateNewUser(Users userModel);

	}
}
