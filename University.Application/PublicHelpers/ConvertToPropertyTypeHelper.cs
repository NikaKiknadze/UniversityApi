namespace University.Application.PublicHelpers;

public static class ConvertToPropertyTypeHelper
{
    public static object? ConvertToPropertyType(this string? value, Type type)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Nullable.GetUnderlyingType(type) != null ? null : Activator.CreateInstance(type);

        if (type == typeof(string)) return value;
        if (type == typeof(int)) return int.TryParse(value, out var i) ? i : 0;
        if (type == typeof(double)) return double.TryParse(value, out var d) ? d : 0.0;
        if (type == typeof(decimal)) return decimal.TryParse(value, out var dec) ? dec : 0m;
        if (type == typeof(bool)) return bool.TryParse(value, out var b) && b;
        if (type == typeof(DateTime)) return DateTime.TryParse(value, out var dt) ? dt : DateTime.MinValue;

        var underlying = Nullable.GetUnderlyingType(type);
        return Convert.ChangeType(value, underlying ?? type);
    }
}