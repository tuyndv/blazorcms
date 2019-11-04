namespace Pl.WebFramework.Localization
{
    public class SqlLocalizationOptions
    {
        public string ConnectionString { get; set; }

        public bool CreateDbResourceIfNotFound { get; set; }

        public void UseSettings(string connectionString, bool createDbResourceIfNotFound)
        {

            ConnectionString = connectionString;
            CreateDbResourceIfNotFound = createDbResourceIfNotFound;
        }
    }
}
