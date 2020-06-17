using Taskboard_API.CustomModules;
using Taskboard_API.Models;
using Taskboard_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Taskboard_API.Controllers
{
	[RoutePrefix("api/users")]
	[EnableCors("*", "*", "*")]
	public class UsersController : ApiController
	{
		IUserService _userService;
		public UsersController(IUserService userService) : base()
		{
			_userService = userService;
		}
		
		[AuthorizeJwt]
		public async Task<HttpResponseMessage> Get()
		{
			var response = await _userService.GetAllUsers();
			return Request.CreateResponse(HttpStatusCode.OK, response);

		}

		[Route("userById")]
		[AuthorizeJwt]
		public async Task<HttpResponseMessage> Get(string id)
		{
			var response = await _userService.GetUserById(id);
			return Request.CreateResponse(HttpStatusCode.OK, response);
		}


		[HttpPut]
		[Route("updateUser")]
		public void Put(int id, [FromBody]string value)
		{
		}

		[HttpDelete]
		[Route("deleteUser")]
		public void Delete(int id)
		{
		}
	}
}
