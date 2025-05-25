namespace University.Data.LogHelpers;

public static class ActionTypeHelper
{
    public static string GetActionType(this string action, string propertyName, string? originalValue)
    {
        return propertyName.Equals("isactive", StringComparison.CurrentCultureIgnoreCase) &&
               !string.IsNullOrEmpty(originalValue) &&
               originalValue.Equals("true", StringComparison.CurrentCultureIgnoreCase)
            ? "Delete"
            : action;
    }
}