namespace Daco.Shared.Converters
{
    public class EmptyStringGuidConverter : JsonConverter<Guid>
    {
        public override Guid Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str))
                    return Guid.Empty;

                if (Guid.TryParse(str, out var guid))
                    return guid;

                return Guid.Empty;
            }

            if (reader.TokenType == JsonTokenType.Null)
                return Guid.Empty;

            return reader.GetGuid();
        }

        public override void Write(
            Utf8JsonWriter writer,
            Guid value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
