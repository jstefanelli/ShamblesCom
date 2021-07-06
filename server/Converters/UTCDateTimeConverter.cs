using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShamblesCom.Server.Converters {
	public class UTCDateTimeConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			string str = reader.GetString();
			DateTime t = DateTime.Parse(str, null, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
			return t;
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			string str = DateTime.SpecifyKind(value, DateTimeKind.Utc).ToString("o", CultureInfo.InvariantCulture);
			writer.WriteStringValue(str);
		}
	}
}