using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Pl.EntityFrameworkCore
{
    public class DataProtectedConverter : ValueConverter<string, string>
    {
        public DataProtectedConverter(IDataProtector dataProtector, ConverterMappingHints mappingHints = default) : 
            base(s => dataProtector.Protect(s), s => dataProtector.Unprotect(s), mappingHints)
        {

        }
    }
}
