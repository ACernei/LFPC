namespace Lab2.Extensions;

public static class ListExtensions
{
    public static string ConcatSorted<T>(this List<T> list)
    {
        list.Sort();
        return string.Join(string.Empty, list);
    } 
}