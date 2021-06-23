using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.SPA {
	public class SPAData {
		public string View { get; set; } = null;

		public bool Redraw { get; set; } = true;

		public object Data { get; set; } = null;

		public string Title { get; set; } = null;
		public string Url { get; set; } = null;
	}
}