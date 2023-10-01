using System.Collections.ObjectModel;

namespace HH_ASP_APP;

public static class Extenstions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableCollection)
        => new(enumerableCollection);

}
