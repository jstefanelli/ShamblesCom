using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.DB.DTO {
	public class DTORaceResult {
		public int Id { get; set; }
		public int DriverId { get; set; }
		public DTODriver Driver { get; set; }
		public int TeamId { get; set; }
		public DTOTeam Team { get; set; }
		public int RaceId { get; set; }
		public DTORace Race { get; set; }
		public int Position { get; set; }
		public bool Finished { get; set; } = true;
		public int Penalties { get; set; } = 0;
		public int StartPosition { get; set;}
		public TimeSpan FastestLap { get; set; }
		public int Points { get; set; }

		public DTORaceResult(RaceResult rr) {
			Id = rr.Id;
			DriverId = rr.DriverId;
			TeamId = rr.TeamId;
			RaceId = rr.RaceId;
			Position = rr.Position;
			Finished = rr.Finished;
			Penalties = rr.Penalties;
			StartPosition = rr.StartPosition;
			FastestLap = rr.FastestLap;
			Points = rr.Points;
		}
	}
}