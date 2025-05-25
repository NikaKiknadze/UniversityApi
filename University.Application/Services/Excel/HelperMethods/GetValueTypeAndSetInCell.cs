using OfficeOpenXml;

namespace University.Application.Services.Excel.HelperMethods;

public static class GetValueTypeAndSetInCell
{
    public static void SetValueInCell(this object value, ExcelRange cell)
    {
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

            case string s when decimal.TryParse(s, out var decimalValue):
                cell.Value = decimalValue;
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