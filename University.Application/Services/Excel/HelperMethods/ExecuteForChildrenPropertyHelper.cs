using System.Reflection;
using OfficeOpenXml;

namespace University.Application.Services.Excel.HelperMethods;

public static class ExecuteForChildrenPropertyHelper
{
    public static void ExecuteForChildrenProperty(this PropertyInfo[] props,
        ExcelWorksheet worksheet,
        object? item,
        List<string> columnNames,
        ref int row,
        int outlineLevel,
        bool isSimpleExport)
    {
        var childProp = props.FirstOrDefault(p =>
            typeof(IEnumerable<object>).IsAssignableFrom(p.PropertyType) &&
            p.PropertyType != typeof(string) &&
            p.Name.Equals("children", StringComparison.CurrentCultureIgnoreCase));

        if (childProp?.GetValue(item) is not IEnumerable<object> children) return;

        foreach (var child in children)
            worksheet.WriteRecursive(child, columnNames, ref row, outlineLevel + 1, isSimpleExport);
    }
}