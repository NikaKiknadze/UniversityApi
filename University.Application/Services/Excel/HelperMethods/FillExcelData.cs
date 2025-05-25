using System.Reflection;
using OfficeOpenXml;

namespace University.Application.Services.Excel.HelperMethods;

public static class FillExcelData
{
    public static void FillDataInWorksheet<T>(this ExcelWorksheet worksheet, List<string> columnNames, List<T> dataList)
    {
        for (var i = 0; i < columnNames.Count; i++)
        {
            worksheet.Cells[1, i + 1].Value = columnNames[i];
            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
        }

        var currentRow = 2;
        foreach (var item in dataList.OfType<T>())
            WriteRecursive(item, worksheet, columnNames, ref currentRow, 0);

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
    }

    private static void WriteRecursive(
        object? item,
        ExcelWorksheet worksheet,
        List<string> columnNames,
        ref int row,
        int outlineLevel)
    {
        var props = item?.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        for (var col = 0; col < columnNames.Count && col < props?.Length; col++)
        {
            var prop = props[col];
            var value = prop.GetValue(item);
            if (value == null) continue;

            var cell = worksheet.Cells[row, col + 1];
            cell.Value = value;
            value.SetValueInCell(cell);
        }

        if (outlineLevel > 0)
        {
            worksheet.Row(row).OutlineLevel = outlineLevel;
            worksheet.Row(row).Collapsed = true;
        }

        row++;

        var childProp = props?.FirstOrDefault(p =>
            typeof(IEnumerable<object>).IsAssignableFrom(p.PropertyType) &&
            p.PropertyType != typeof(string) &&
            p.Name.Equals("children", StringComparison.CurrentCultureIgnoreCase));

        if (childProp?.GetValue(item) is not IEnumerable<object> children) return;

        foreach (var child in children)
            WriteRecursive(child, worksheet, columnNames, ref row, outlineLevel + 1);
    }
}