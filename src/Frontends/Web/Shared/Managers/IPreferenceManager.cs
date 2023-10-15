using BlazorApp.Shared.Settings;
using System.Threading.Tasks;
using BlazorApp.Shared.Wrapper;

namespace BlazorApp.Shared.Managers;

public interface IPreferenceManager
{
    Task SetPreference(IPreference preference);

    Task<IPreference> GetPreference();

    Task<IResult> ChangeLanguageAsync(string languageCode);
}