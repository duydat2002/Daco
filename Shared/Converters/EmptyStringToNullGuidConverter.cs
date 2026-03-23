namespace Daco.Shared.Converters
{
    public class EmptyStringToNullGuidConverter : JsonConverter<Guid?>
    {
        public override Guid? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str))
                    return null;

                if (Guid.TryParse(str, out var guid))
                    return guid;

                return null;
            }

            if (reader.TokenType == JsonTokenType.Null)
                return null;

            return reader.GetGuid();
        }

        public override void Write(
            Utf8JsonWriter writer,
            Guid? value,
            JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.Value.ToString());
        }
    }
}
