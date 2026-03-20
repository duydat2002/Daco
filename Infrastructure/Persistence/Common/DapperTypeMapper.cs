namespace Daco.Infrastructure.Persistence.Common
{
    public static class DapperTypeMapper
    {
        public static void RegisterMappings()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName!.StartsWith("Daco.")); ;

            var typesWithColumnMapping = assemblies
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); }  // bỏ qua assembly không load được
                })
                .Where(t => t.IsClass && !t.IsAbstract &&
                            t.GetProperties()
                             .Any(p => p.GetCustomAttributes<ColumnMappingAttribute>(false).Any()));

            foreach (var type in typesWithColumnMapping)
            {
                SqlMapper.SetTypeMap(
                    type,
                    new CustomPropertyTypeMap(
                        type,
                        (t, columnName) =>
                            t.GetProperties().FirstOrDefault(p =>
                                p.GetCustomAttributes<ColumnMappingAttribute>(false)
                                 .Any(a => a.Name == columnName.ToUpperInvariant())
                                || p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)
                            )
                    )
                );
            }
        }
    }
}
