using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taskboard_API.ViewModels
{
	public class User_DTO
	{
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

    }
}