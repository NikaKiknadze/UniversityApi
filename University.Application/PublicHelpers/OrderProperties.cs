using System.Reflection;

namespace University.Application.PublicHelpers;

public static class OrderProperties
{
    public static List<PropertyInfo> Execute<T>()
    {
        return typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite)
            .ToList();
    }
}