using BooksBay_API.Models;
using BooksBay_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Taskboard_API.CustomModules;

namespace BooksBay_API.Controllers
{
    [RoutePrefix("api/board")]
    [EnableCors("*", "*", "*")]
    //[AuthorizeJwt(Roles ="Admin,Manager")]

    public class BoardController : ApiController
    {
        IBoardService _boardService;

        public BoardController(IBoardService _service) : base()
        {
            _boardService = _service;
        }

        [HttpPost]
        [Route("createBoard")]
        public async Task<HttpResponseMessage> CreateBoard(Board board)
        {
            board.Id = "";
            var response = await _boardService.CreateBoard(board);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [HttpGet]
        [Route("getAllBoardsByUser")]
        public async Task<HttpResponseMessage> GetAllBoardByUser(string userEmail)
        {
            var response = await _boardService.GetAllBoardByUser(userEmail);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("getBoardById")]
        public async Task<HttpResponseMessage> GetBoardById(string boardId)
        {
            var response = await _boardService.GetBoardById(boardId);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("getBoardItemById")]
        public async Task<HttpResponseMessage> GetBoardItemById(string boardItem)
        {
            var response = await _boardService.GetBoardItemById(boardItem);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("createBoardItem")]
        public async Task<HttpResponseMessage> CreateNewBoardItem(BoardItem item)
        {
            item.Id = "";
            var response = await _boardService.CreateNewBoardItem(item);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [HttpDelete]
        [Route("deleteBoardItem")]
        public async Task<HttpResponseMessage> DeleteBoardItem(string Id)
        {
            var response = await _boardService.DeleteBoardItem(Id);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [HttpDelete]
        [Route("deleteBoard")]
        public async Task<HttpResponseMessage> DeleteBoard(string Id)
        {
            var response = await _boardService.DeleteBoard(Id);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [HttpPost]
        [Route("createMultipleBoards")]
        public async Task<HttpResponseMessage> CreateMultipleBoards(List<Board> boards)
        {
            var response = await CreateMultipleBoards(boards);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [HttpPut]
        [Route("saveBoards")]
        public async Task<HttpResponseMessage> SaveMultipleBoards(List<Board> boards)
        {
            var response = await _boardService.SaveBoardChanges(boards);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }


    }
}
