using System.Collections.ObjectModel;

namespace ViewerData_MAUI_APP;

public static class Extenstions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableCollection)
        => new(enumerableCollection);

}
