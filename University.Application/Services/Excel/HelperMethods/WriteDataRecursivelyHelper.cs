using System.Collections;
using System.ComponentModel;
using System.Reflection;
using OfficeOpenXml;

namespace University.Application.Services.Excel.HelperMethods;

public static class WriteDataRecursivelyHelper
{
    public static void WriteRecursive(
        this ExcelWorksheet worksheet,
        object? item,
        List<string> columnNames,
        ref int row,
        int outlineLevel,
        bool isSimpleExport)
    {
        var props = item?.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (props == null || columnNames.Count == 0) return;

        var excelCol = 1;
        var processedColumns = 0;

        foreach (var prop in props)
        {
            if (processedColumns >= columnNames.Count) break;

            var descriptionAttr = prop.GetCustomAttributes(typeof(DescriptionAttribute), true)
                .FirstOrDefault() as DescriptionAttribute;

            if (descriptionAttr is { Description: "DoNotIncludeExcel" })
                continue;

            var value = prop.GetValue(item);
            if (value == null)
            {
                excelCol++;
                continue;
            }

            if (prop.Name.Contains("FillWithKeys", StringComparison.OrdinalIgnoreCase) &&
                value is IDictionary dictionary)
                foreach (DictionaryEntry kvp in dictionary)
                {
                    if (kvp.Key is not string key) continue;

                    var columnIndex = columnNames.FindIndex(name =>
                        name.Equals(key, StringComparison.OrdinalIgnoreCase));

                    if (columnIndex < 0)
                        continue;

                    var dictCell = worksheet.Cells[row, columnIndex + 1];

                    dictCell.Value = kvp.Value;

                    kvp.Value?.SetValueInCell(descriptionAttr, dictCell, isSimpleExport: true);
                    processedColumns++;
                }
            else
            {
                var cell = worksheet.Cells[row, excelCol];
                cell.Value = value;
                value.SetValueInCell(descriptionAttr, cell, isSimpleExport);
                processedColumns++;
                excelCol++;
            }
        }

        if (outlineLevel > 0)
        {
            worksheet.Row(row).OutlineLevel = outlineLevel;
            worksheet.Row(row).Collapsed = true;
        }

        row++;

        props.ExecuteForChildrenProperty(worksheet, item, columnNames, ref row, outlineLevel, isSimpleExport);
    }
}