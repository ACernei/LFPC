using System.Text;

namespace Lab5.Extensions;

public static class HashSetExtensions
{
    public static string? ToString<T>(this HashSet<T> hashSet, string delimiter) =>
        string.Join(delimiter, hashSet.OrderBy(e => e));
}