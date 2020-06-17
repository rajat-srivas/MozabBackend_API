using Taskboard_API.Helpers;
using Taskboard_API.Services;
using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BooksBay_API.Services;

namespace Taskboard_API.CustomModules
{
	public static class DiContainerExtension
	{
		public	static IContainer AddDependencies(this IContainer container)
		{
			container.Register<IAuthService, AuthService>(Reuse.Singleton);
			container.Register<IUserService, UserService>(Reuse.Singleton);
			container.Register<IBoardService, BoardService>(Reuse.Singleton);
			container.Register<IMapperProvider, MapperProvider>(Reuse.Singleton);
			return container;
		}
	}
}