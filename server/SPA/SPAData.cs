using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.SPA {
	public class SPAData {
		[JsonPropertyName("view")]
		public string View { get; set; } = null;

		[JsonPropertyName("redraw")]
		public bool Redraw { get; set; } = false;

		[JsonPropertyName("data")]
		public object Data { get; set; } = null;

		[JsonPropertyName("title")]
		public string Title { get; set; } = null;
		[JsonPropertyName("url")]
		public string Url { get; set; } = null;
	}
}