using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using University.Application.Services.Excel.HelperMethods;

namespace University.Application.Services.Excel;

public class ExcelServices : IExcelServices
{
    public async Task<FileContentResult> ExportToExcel<T>(List<string> columnNames, string fileName, List<T> dataList,
        CancellationToken cancellationToken)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        worksheet.FormatFirstRow(columnNames);
        
        worksheet.FillDataInWorksheet(columnNames, dataList);

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var exportedFileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        var result = await package.GetAsByteArrayAsync(cancellationToken);

        return new FileContentResult(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = exportedFileName
        };
    }
}