using AutoMapper;
using Taskboard_API.Models;
using Taskboard_API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taskboard_API.Helpers
{
	public class MapperProvider: IMapperProvider
	{
		public IMapper GetMapper()
		{
			var config = new MapperConfiguration(cfg => {
				cfg.CreateMap<Users, User_DTO>();
				cfg.CreateMap<User_DTO, Users>();
			});

			IMapper mapper = config.CreateMapper();
			return mapper;

		}
	}
}