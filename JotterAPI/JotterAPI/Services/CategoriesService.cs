using JotterAPI.DAL;
using JotterAPI.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Services
{
	public class CategoriesService : BaseService, ICategoriesService
	{
		public CategoriesService(JotterDbContext dbContext) : base(dbContext)
		{
		}
	}
}
