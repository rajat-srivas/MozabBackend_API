using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Taskboard_API.Helpers;
using Taskboard_API.Models;
using Taskboard_API.ViewModels;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using BooksBay_API.Services;
using BooksBay_API.Models;
using BooksBay_API.Helpers;

namespace Taskboard_API.Services
{
	public class UserService : IUserService
	{
		IMongoCollection<Users> users;
		IAuthService _authService ;
		IMapperProvider _mapper;
		IBoardService _boardService;
		
		public UserService(IAuthService authService, IMapperProvider provider, IBoardService board)
		{
			//var client = new MongoClient("mongodb://127.0.0.1:27017");
			var client = new MongoClient(ConfigConstants.MongoDBClient);
			var database = client.GetDatabase("Mozab");
			users = database.GetCollection<Users>("Users");
			_authService = authService;
			_mapper = provider;
			_boardService = board;
		}

		public async Task<bool> CheckEmailExist(string email)
		{
			if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
			var response = await users.FindAsync(x => x.Email == email);
			if (response.ToList().Count > 0)
				return true;
			return false;
				
		}
		
		public async Task<List<User_DTO>> GetAllUsers()
		{
			var response = await users.Find(users => true).ToListAsync();


			var user = _mapper.GetMapper().Map<List<User_DTO>>(response);
			return user;

		}

		public async Task<User_DTO> GetUserById(string id)
		{
			var response = await users.Find(users => users.Id == id).FirstOrDefaultAsync();
			var user = _mapper.GetMapper().Map<User_DTO>(response);
			return user;
		}

		public async Task<string> CreateNewUser(Users userModel)
		{
			if (userModel != null)
			{
				byte[] passwordHash, passwordSalt;
				_authService.CreatePasswordHash(userModel.Password, out passwordHash, out passwordSalt);
				userModel.PasswordHash = passwordHash;
				userModel.Salt = passwordSalt;
				await users.InsertOneAsync(userModel);

				Board newBoard = new Board()
				{
					BoardTitle = "Lets Board",
					LinkedUser = userModel.Email
				};
				
				var boardId = await _boardService.CreateBoard(newBoard);

				BoardItem newBoardItem = new BoardItem()
				{
					BoardId = boardId,
					Item = "Hello!"
				};

				await _boardService.CreateNewBoardItem(newBoardItem);
				return userModel.Id;
			}

			throw new ArgumentNullException(nameof(userModel));
		}
	}
}