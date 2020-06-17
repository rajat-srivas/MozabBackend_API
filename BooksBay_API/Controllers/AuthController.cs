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
using Taskboard_API.ViewModels;

namespace Taskboard_API.Controllers
{
    [RoutePrefix("api/auth")]
    [EnableCors("*", "*", "*")]
    public class AuthController : ApiController
    {
        IAuthService _authService;
        IUserService _userService;
        public AuthController(IAuthService _serviceAuth, IUserService _serviceUser) : base()
        {
            _authService = _serviceAuth;
            _userService = _serviceUser;
        }

        [Route("login")]
        [HttpGet]
        public async Task<HttpResponseMessage> Login(string email, string password)
        {
            var user = await _authService.Login(email, password);
            if(user != null)
            {
               var token = await _authService.GenerateJWTToken(user);
                return Request.CreateResponse(HttpStatusCode.OK, token);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        [Route("registration")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateNewUser([FromBody]Users userModel)
        {
            var response = await _userService.CreateNewUser(userModel);
            return Request.CreateResponse(HttpStatusCode.Created, response);
        }

        [Route("emailValidation")]
        [HttpGet]
        public async Task<bool> CheckEmailExist(string email)
        {
            return await _userService.CheckEmailExist(email);
        }
    }
}
