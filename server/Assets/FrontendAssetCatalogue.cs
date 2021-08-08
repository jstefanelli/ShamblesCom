using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace ShamblesCom.Server.Assets {
	public interface IFrontendAssets {
		string GetPublicPath(string asset_name);
		IEnumerable<string> GetAssetNames();
	}

	public class FrontendAssets : IFrontendAssets {
		private readonly Dictionary<string, string> Assets = new Dictionary<string, string>();

		public FrontendAssets(string manifestPath) {
			if (!File.Exists(manifestPath)) {
				throw new FileNotFoundException($"Could not find frontend manifest file at: {manifestPath}");
			}

			using (JsonDocument doc = JsonDocument.Parse(File.ReadAllText(manifestPath))) {
				JsonElement root = doc.RootElement;
				foreach (JsonProperty e in root.EnumerateObject()) {
					string name = e.Name;
					string value = e.Value.GetString();
					if (String.IsNullOrWhiteSpace(value))
						continue;

					Assets.Add(name, value);
				}
			}
		}

		public IEnumerable<string> GetAssetNames()
		{
			return new List<string>(Assets.Keys);
		}

		public string GetPublicPath(string asset_name)
		{
			if (Assets.TryGetValue(asset_name, out string val)) {
				return val;
			}
			return null;
		}
	}
}