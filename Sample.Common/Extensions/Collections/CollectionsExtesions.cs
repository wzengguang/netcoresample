namespace Sample.Common.Extensions.Collections
{
    public static class CollectionsExtesions
    {
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}
