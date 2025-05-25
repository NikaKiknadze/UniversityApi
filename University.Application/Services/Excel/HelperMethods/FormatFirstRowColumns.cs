using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace University.Application.Services.Excel.HelperMethods;

public static class FormatFirstRowColumns
{
    public static void FormatFirstRow(this ExcelWorksheet worksheet, List<string> columnNames)
    {
        for (var i = 0; i < columnNames.Count; i++)
        {
            worksheet.Cells[1, i + 1].Value = columnNames[i];
            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }
    }
}