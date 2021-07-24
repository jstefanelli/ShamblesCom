using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using System.Collections.Concurrent;

namespace ShamblesCom.Server.GApi {
	public static class GoogleApi {
		private static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
		private static string ApplicationName = "ShamblesCom";
		private static GoogleClientSecrets ServerSecrets { get; set; }
		private static GoogleAuthorizationCodeFlow AuthFlow { get; set; }

		private static readonly ConcurrentDictionary<Guid, UserCredential> LoggedUsers = new ConcurrentDictionary<Guid, UserCredential>();

		public static async Task Init() {
			string secretsFile = "Config/google_auth.json";
			#if DEBUG
			secretsFile = "Config/google_auth_test.json";
			#endif

			if (ServerSecrets == null) {
				if (!File.Exists(secretsFile)) {
					throw new FileNotFoundException("Could not find google API authentication file");
				}

				using (Stream s = new FileStream(secretsFile, FileMode.Open, FileAccess.Read)) {
					ServerSecrets = await GoogleClientSecrets.FromStreamAsync(s);
				}
			}
			
			if (AuthFlow == null) {
				AuthFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer() {
					ClientSecrets = ServerSecrets.Secrets,
					Scopes = Scopes,
					DataStore = new FileDataStore("Config/google_tokens", true)
				});
			}
		}

		private static string GenReidrectUrl(string host) {
			return  Path.Combine(host, "gapi/code");
		}

		public static async Task<Uri> GenLoginURL(string myUrl){
			await Init();			

			return AuthFlow.CreateAuthorizationCodeRequest(GenReidrectUrl(myUrl)).Build();
		}

		public static async Task<Guid> CompleteLogin(string code, string host) {

			TokenResponse token = await AuthFlow.ExchangeCodeForTokenAsync("fuck_you_google", code, GenReidrectUrl(host), CancellationToken.None);
			UserCredential credential = new UserCredential(AuthFlow, "fuck_you_google", token);

			Guid guid;
			do {
				guid = new Guid();
			} while(!LoggedUsers.TryAdd(guid, credential));

			return guid;
		}

		public static SheetsService GetSheetsService(Guid userId) {
			if (!LoggedUsers.TryGetValue(userId, out UserCredential user)) {
				return null;
			}

			if (user.Token.IsExpired(AuthFlow.Clock)) {
				return null;
			}

			SheetsService s = new SheetsService(new BaseClientService.Initializer() {
				HttpClientInitializer = user,
				ApplicationName = ApplicationName
			});
			
			return s;
		}
	}
}