using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ShamblesCom.Server.GApi;

namespace ShamblesCom.Server.Controllers {
	[Route("/gapi")]
	public class GapiController : ControllerBase {
		public const string GapiUserUuidSessionKey = "gapi_usr";
		public const string GapiRequestIdSessionKey = "gapi_req";

		private static readonly ConcurrentDictionary<Guid, string> requests = new ConcurrentDictionary<Guid, string>();

		private static string GetHost(HttpContext ctx) {
			Uri myUri = new Uri(ctx.Request.GetDisplayUrl());
			return $"{myUri.Scheme}://{myUri.Host}" + (myUri.IsDefaultPort ? string.Empty : $":{myUri.Port}");
		}

		[HttpGet("test")]
		public async Task<ActionResult> Test() {
			string host = GetHost(HttpContext);

			GapiRequest? req = await Generate(host, host);

			if (req != null) {
				HttpContext.Session.SetString(GapiRequestIdSessionKey, req.Value.requestGuid.ToString());
				return Redirect(req.Value.requestUri.ToString());
			} else {
				return NotFound();
			}
		}

		[HttpGet("code")]
		public async Task<ActionResult> ReceiveGapiCode(string code, string scope) {
			string host = GetHost(HttpContext);
			Guid gapiUid = await GoogleApi.CompleteLogin(code, host);
			
			HttpContext.Session.SetString(GapiUserUuidSessionKey, gapiUid.ToString());

			string reqIdStr = HttpContext.Session.GetString(GapiRequestIdSessionKey);
			if (!string.IsNullOrWhiteSpace(reqIdStr))
			{
				HttpContext.Session.Remove(GapiRequestIdSessionKey);
				Guid reqId = new Guid(reqIdStr);

				if (requests.TryRemove(reqId, out string redirect)) {
					return new SeeOtherResult(redirect);
				}
			}

			return new SeeOtherResult(GetHost(HttpContext));
		}

		public static async Task<GapiRequest?> Generate(string host, string redirectUri) {
			Uri requestUri = await GoogleApi.GenLoginURL(host);
			if (requestUri == null) {
				return null;
			}

			Guid requestGuid;
			do {
				requestGuid = new Guid();
			} while (!requests.TryAdd(requestGuid, redirectUri));

			return new GapiRequest {
				requestGuid = requestGuid,
				requestUri = requestUri
			};
		}
	}

	public struct GapiRequest {
		public Guid requestGuid { get; set; }
		public Uri requestUri { get; set; }
	}
}