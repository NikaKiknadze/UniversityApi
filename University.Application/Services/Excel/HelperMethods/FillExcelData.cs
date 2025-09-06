using OfficeOpenXml;

namespace University.Application.Services.Excel.HelperMethods;

public static class FillExcelData
{
    public static void Execute<T>(ExcelWorksheet worksheet, List<string> columnNames, List<T> dataList, bool isSimpleExport)
    {
        for (var i = 0; i < columnNames.Count; i++)
        {
            worksheet.Cells[1, i + 1].Value = columnNames[i];
            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
        }

        var currentRow = 2;
            
        foreach (var item in dataList.OfType<T>())
            worksheet.WriteRecursive(item, columnNames, ref currentRow, 0, isSimpleExport);

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
    }
}