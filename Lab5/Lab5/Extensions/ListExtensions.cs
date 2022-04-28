namespace Lab5.Extensions;

public static class ListExtensions
{
    public static string ToString<T>(this IEnumerable<T> list, string delimiter) => string.Join(delimiter, list);
}