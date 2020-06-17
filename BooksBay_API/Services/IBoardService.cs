using BooksBay_API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBay_API.Services
{
	public interface IBoardService
	{
		Task<string> CreateBoard(Board board);

		Task<List<Board>> GetAllBoardByUser(string userEmail);

		Task<Board> GetBoardById(string boardId);

		Task<BoardItem> GetBoardItemById(string boardItem);

		Task<string> CreateNewBoardItem(BoardItem item);

		Task<MongoDB.Driver.DeleteResult> DeleteBoardItem(string Id);

		Task<MongoDB.Driver.DeleteResult> DeleteBoard(string Id);

		Task<List<string>> CreateMultipleBoards(List<Board> boards);

		Task<List<UpdateResult>> SaveBoardChanges(List<Board> boardsList);
	}
}
