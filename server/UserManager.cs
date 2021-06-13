using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server {
	public static class UserManager {
		public static readonly string RootEmail = Environment.GetEnvironmentVariable("shamblescom_rootEmail") ?? "john@jstefanelli.com";
		public static readonly string RootPassword = Environment.GetEnvironmentVariable("shamblescom_rootPsw") ?? "This_is_unsafe_49";

		public const int MinPasswordLength = 12;
		public const int BCryptWorkFactor = 10;

		public static async Task<ShamblesUser> Login(HttpContext context, ShamblesDBContext db, string email, string password) {
			ShamblesUser user = db.Users.FirstOrDefault(s => s.Email == email);

			if (user == null) {
				return null;
			}

			if (await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, user.HashedPassword)).ConfigureAwait(false)) {
				ClaimsIdentity identity = new ClaimsIdentity(new [] {
					new Claim(ClaimTypes.Email, user.Email),
					new Claim("UserId", user.Id.ToString())
				}, CookieAuthenticationDefaults.AuthenticationScheme);

				await context.SignInAsync(new ClaimsPrincipal(identity));

				return user;
			}

			return null;
		}

		public static async Task<RegisterResult> Register(ShamblesDBContext db, string email, string password) {
			bool hasLowercase = false;
			bool hasUppercase = false;
			bool hasDigit = false;

			password = password.Trim();

			if (password.Length < MinPasswordLength) {
				return RegisterResult.PasswordTooShort;
			}

			foreach(char c in password) {
				if (Char.IsLower(c)) {
					hasLowercase = true;
				}

				if (Char.IsUpper(c)) {
					hasUppercase = true;
				}

				if (Char.IsDigit(c)) {
					hasDigit = true;
				}
			}

			if (!hasLowercase || !hasUppercase || !hasDigit) {
				return RegisterResult.PasswordTooWeak;
			}

			if (db.Users.Any((s => s.Email == email))) {
				return RegisterResult.EmailTaken;
			}

			string hash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password, BCryptWorkFactor)).ConfigureAwait(false);

			var change = await db.Users.AddAsync(new ShamblesUser() {
				Email = email,
				HashedPassword = hash
			}).ConfigureAwait(false);

			return RegisterResult.Ok;
		}

		public static async Task Seed(ShamblesDBContext db) {
			if (!db.Users.Any()) {
				Console.WriteLine($"Registering user: {RootEmail}, {RootPassword}");
				await Register(db, RootEmail, RootPassword);
				await db.SaveChangesAsync().ConfigureAwait(false);
				Console.WriteLine($"Any users: {db.Users.Any()}");
			}
		}

		public enum RegisterResult {
			Ok,
			EmailTaken,
			PasswordTooShort,
			PasswordTooWeak
		}
	}
}