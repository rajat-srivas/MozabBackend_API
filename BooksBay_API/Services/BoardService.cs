using BooksBay_API.Helpers;
using BooksBay_API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Taskboard_API.Helpers;
using Taskboard_API.Models;
using Taskboard_API.Services;

namespace BooksBay_API.Services
{
	public class BoardService : IBoardService
	{
		IMongoCollection<Board> boards;
		IMongoCollection<BoardItem> boardItem;
		IMongoCollection<Users> users;
		IAuthService _authService;
		IMapperProvider _mapper;

		public BoardService(IAuthService authService, IMapperProvider provider)
		{
			//var client = new MongoClient("mongodb://127.0.0.1:27017");
			var client = new MongoClient(ConfigConstants.MongoDBClient);
			var database = client.GetDatabase("Mozab");
			users = database.GetCollection<Users>("Users");
			boards = database.GetCollection<Board>("Boards");
			boardItem = database.GetCollection<BoardItem>("BoardItem");
			_authService = authService;
			_mapper = provider;
		}

		public async Task<string> CreateBoard(Board board)
		{
			//if (string.IsNullOrWhiteSpace(board.BoardTitle)) throw new ArgumentNullException(nameof(board.BoardTitle));
			await boards.InsertOneAsync(board);
			return board.Id;
		}

		public async Task<string> CreateNewBoardItem(BoardItem item)
		{
			//if (string.IsNullOrWhiteSpace(item.Item)) throw new ArgumentNullException("BoardItem");
			//if (string.IsNullOrWhiteSpace(item.BoardId)) throw new ArgumentNullException("BoardId");
			await boardItem.InsertOneAsync(item);
			return item.Id;
		}

		public async Task<DeleteResult> DeleteBoard(string id)
		{
			var itemToDelete = Builders<Board>.Filter.Eq("Id", id);
			return boards.DeleteOne(itemToDelete);
		}

		public async Task<DeleteResult> DeleteBoardItem(string id)
		{
			var itemToDelete = Builders<BoardItem>.Filter.Eq("Id", id);
			return boardItem.DeleteOne(itemToDelete);
		}

		public async Task<List<Board>> GetAllBoardByUser(string userEmail)
		{
			var userBoards = await boards.Find(board => board.LinkedUser.Equals(userEmail)).ToListAsync();
			if (userBoards.Count > 0)
			{
				foreach (var board in userBoards)
				{
					var items = await boardItem.Find(item => item.BoardId.Equals(board.Id)).ToListAsync();
					board.BoardItems = items;
				}

			}
			return userBoards;
		}

		public async Task<Board> GetBoardById(string boardId)
		{
			var boardById = await boards.Find(board => board.Id == boardId).FirstOrDefaultAsync();
			var items = await boardItem.Find(item => item.BoardId.Equals(boardById.Id)).ToListAsync();
			boardById.BoardItems = items;
			return boardById;
		}

		public async Task<BoardItem> GetBoardItemById(string boardItemId)
		{
			var boardItemById = await boardItem.Find(board => board.Id == boardItemId).FirstOrDefaultAsync();
			return boardItemById;
		}

		public async Task<List<string>> CreateMultipleBoards(List<Board> boards)
		{
			var newBoards = new List<string>();
			foreach (var board in boards)
			{
				var id = await CreateBoard(board);
				newBoards.Add(id);
				board.BoardItems.ForEach(x => x.BoardId = id);
				board.BoardItems.ForEach(x => CreateNewBoardItem(x));
			}
			return newBoards;
		}

		public async Task<List<UpdateResult>> SaveBoardChanges(List<Board> boardsList)
		{
			var updateResult = new List<UpdateResult>();
			foreach (var board in boardsList)
			{
				var filter = Builders<Board>.Filter.Eq("Id", board.Id);
				var update = Builders<Board>.Update.Set("BoardTitle", board.BoardTitle);
				var result = boards.UpdateOne(filter, update);
				updateResult.Add(result);
				foreach (var bItem in board.BoardItems)
				{
					bItem.BoardId = board.Id;
					if (!string.IsNullOrWhiteSpace(bItem.Id) || bItem.Id.Equals("1"))
					{
						var itemFilter = Builders<BoardItem>.Filter.Eq("Id", bItem.Id);
						var itemUpdate = Builders<BoardItem>.Update.Set("Item", bItem.Item);
						boardItem.UpdateOne(itemFilter, itemUpdate);
					}
					else
					{
						await CreateNewBoardItem(bItem);
					}
				}
			}

			return updateResult;
		}
	}
}
	