namespace Daco.Infrastructure.Persistence.Mappings.Users
{
    public static class UserGenderMapper
    {
        public static UserGender FromDb(string value)
            => value?.ToLower() switch
            {
                "male" => UserGender.Male,
                "female" => UserGender.Female,
                "other" => UserGender.Other,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };

        public static string ToDb(UserGender gender)
            => gender switch
            {
                UserGender.Male => "male",
                UserGender.Female => "female",
                UserGender.Other => "other",
                _ => throw new ArgumentOutOfRangeException(nameof(gender))
            };
    }
}
