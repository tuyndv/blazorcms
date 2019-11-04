using Microsoft.AspNetCore.DataProtection;
using Pl.Core.Exceptions;
using System.IO;

namespace Pl.Core.Security
{
    public static class SystemDataProtection
    {
        private static IDataProtector _dataProtector;

        public static IDataProtector DataProtector
        {
            get
            {
                GuardClausesParameter.Null(_dataProtector, nameof(_dataProtector));
                return _dataProtector;
            }
        }

        /// <summary>
        /// Get data protecter by file system key manager. if your aplication run on multi server you need use redis of azure key manager
        /// </summary>
        /// <param name="basePath">Base folder to save key
        /// ex. in asp.net core set WebHostEnvironment.ContentRootPath
        /// </param>
        /// <param name="applicationName">The application name
        /// ex. in asp.net core set WebHostEnvironment.ApplicationName
        /// </param>
        /// <param name="applicationKey">system key to identity system to read and write protected data
        /// ex. in asp.net core set WebHostEnvironment.EnvironmentName
        /// </param>
        public static IDataProtector InitializeFileSystemProtecter(string basePath, string applicationName, string applicationKey)
        {
            if (_dataProtector != null)
            {
                return _dataProtector;
            }

            GuardClausesParameter.NullOrEmpty(basePath, nameof(basePath));
            GuardClausesParameter.NullOrEmpty(applicationName, nameof(applicationName));
            GuardClausesParameter.NullOrEmpty(applicationKey, nameof(applicationKey));

            var destFolder = Path.Combine(basePath, "datakey");
            var dataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(destFolder), options =>
            {
                options.SetApplicationName(applicationName);
            });
            _dataProtector = dataProtectionProvider.CreateProtector(applicationKey);
            return _dataProtector;
        }
    }
}
