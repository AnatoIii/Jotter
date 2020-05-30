using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JotterAPI.Controllers
{
	[Route("[controller]")]
	public class TestController : Controller
	{
		[HttpGet]
		public string TestMethod()
		{
			return "It works";
		}
	}
}