using System.ComponentModel;
using System.Globalization;
using OfficeOpenXml;

namespace University.Application.Services.Excel.HelperMethods;

public static class SetValuesInCellHelper
{
    public static void SetValueInCell(this object value, DescriptionAttribute? descAttribute, ExcelRange cell,
        bool isSimpleExport)
    {
        var invariantCulture = CultureInfo.InvariantCulture;

        if (descAttribute != null)
        {
            var attributeName = descAttribute.Description;
            if (!string.IsNullOrEmpty(attributeName) && attributeName is "Number" or "SimpleString")
                cell.Value = value.ToString();
            else if (!string.IsNullOrEmpty(attributeName) && attributeName is "TimeSpan")
            {
                cell.Value = TimeSpan.FromSeconds((int)value);
                cell.Style.Numberformat.Format = @"hh\:mm\:ss";
            }
            else if (!string.IsNullOrEmpty(attributeName) && attributeName is "ConvertedTimeSpan")
            {
                cell.Value = value;
                cell.Style.Numberformat.Format = @"HH\:mm\:ss";
            }
            else if (!string.IsNullOrEmpty(attributeName) && attributeName is "double" or "float" or "decimal")
            {
                cell.Value = value;
                cell.Style.Numberformat.Format = "#,##0.00";
            }

            return;
        }

        if (isSimpleExport)
        {
            cell.Value = value.ToString();
            return;
        }

        switch (value)
        {
            case string s when int.TryParse(s, out var intValue):
                cell.Value = intValue;
                cell.Style.Numberformat.Format = "#,##0";
                break;

            case string s when long.TryParse(s, out var longValue):
                cell.Value = longValue;
                cell.Style.Numberformat.Format = "#,##0";
                break;

            case string s when double.TryParse(s, out var doubleValue):
                cell.Value = doubleValue;
                cell.Style.Numberformat.Format = "#,##0.00";
                break;

            case string s when float.TryParse(s, out var floatValue):
                cell.Value = floatValue;
                cell.Style.Numberformat.Format = "#,##0.00";
                break;

            case string s when decimal.TryParse(s, NumberStyles.Any, invariantCulture, out var decimalVal):
                cell.Value = decimalVal;
                cell.Style.Numberformat.Format = "#,##0.00";
                break;

            case DateTime dt:
                cell.Value = dt;
                cell.Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                break;

            default:
                cell.Value = value.ToString();
                break;
        }
    }
}