using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShamblesCom.Server.Controllers {
	public class SeeOtherResult : ActionResult {
		protected string Location { get; set; }
		protected bool AddSPAQuery { get; set; }

		public SeeOtherResult(string location, bool addSPAQuery = true) {
			Location = location;
			AddSPAQuery = addSPAQuery;
		}

		public override Task ExecuteResultAsync(ActionContext context)
		{
			context.HttpContext.Response.Headers.Add("Location", Location + (AddSPAQuery ? "?spa=data" : ""));
			context.HttpContext.Response.StatusCode = (int)HttpStatusCode.SeeOther;
			return Task.CompletedTask;
		}
	}
}