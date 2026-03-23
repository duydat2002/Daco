namespace Daco.Shared.Converters
{
    public class EmptyStringToNullDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str))
                    return null;

                if (DateTime.TryParse(str, out var date))
                    return date;

                return null;
            }

            if (reader.TokenType == JsonTokenType.Null)
                return null;

            return reader.GetDateTime();
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime? value,
            JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.Value.ToString("O"));
        }
    }
}
