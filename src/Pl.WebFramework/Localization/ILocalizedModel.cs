using System.Collections.Generic;

namespace Pl.WebFramework.Localization
{
    public interface ILocalizedModel<TLocalizedModel>
    {
        IList<TLocalizedModel> Locales { get; set; }
    }
}