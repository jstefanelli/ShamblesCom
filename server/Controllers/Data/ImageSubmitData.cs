using System.Linq;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.Controllers.Data {
		public struct ImageSubmitData {
			public static readonly string[] SupportedImageTypes = new string[] {
				"image/png",
				"image/jpeg",
				"image/jpg"
			};

			[JsonPropertyName("base64data")]
			public string Base64Data { get; set; }

			public bool Decode(out byte[] data, out string mimeType) {
				int idx = Base64Data.IndexOf(";base64,");
				int dataLength = "data:".Length;
				mimeType = Base64Data.Substring(dataLength, idx - dataLength);
				string dataStr = Base64Data.Substring(idx + ";base64,".Length);

				if (!SupportedImageTypes.Contains(mimeType)) {
					data = null;
					mimeType = null;
					return false;
				}

				data = System.Convert.FromBase64String(dataStr);
				return true;
			}
		}
}