using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using University.Application.PublicHelpers;
using University.Application.Services.Excel.HelperMethods;

namespace University.Application.Services.Excel;

public class ExcelServices : IExcelServices
{
    public async Task<FileContentResult> ExportToExcel<T>(List<string> columnNames, string fileName,
        List<T> dataList, CancellationToken cancellationToken, bool isSimpleExport = false)
    {
        var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("Sheet1");

        worksheet.FormatFirstRow(columnNames);

        FillExcelData.Execute(worksheet, columnNames, dataList, isSimpleExport);

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var exportedFileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        var result = await package.GetAsByteArrayAsync(cancellationToken);

        return new FileContentResult(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = exportedFileName
        };
    }

    public ExcelPackage ExpandPackage<T>(ExcelPackage package, List<string> columnNames, string sheetName,
        List<T> dataList, bool isSimpleExport)
    {
        var worksheet = package.Workbook.Worksheets.Add($"{sheetName}");

        worksheet.FormatFirstRow(columnNames);

        FillExcelData.Execute(worksheet, columnNames, dataList, isSimpleExport);

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return package;
    }

    public async Task<FileContentResult> DownloadTemplate(List<string> columnNames, string fileName,
        CancellationToken cancellationToken)
    {
        var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("კამპანიის შაბლონი");

        for (var i = 0; i < columnNames.Count; i++)
        {
            worksheet.Cells[1, i + 1].Value = columnNames[i];
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var exportedFileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        var result = await package.GetAsByteArrayAsync(cancellationToken);

        return new FileContentResult(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = exportedFileName
        };
    }

    public async Task<List<T>> GetDataFromExcel<T>(IFormFile file, CancellationToken cancellationToken) where T : new()
    {
        var result = new List<T>();

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        using var package = new ExcelPackage(memoryStream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        if (worksheet == null)
            throw new InvalidOperationException("No worksheet found in the Excel file.");

        var properties = OrderProperties.Execute<T>();
        var rowCount = worksheet.Dimension.Rows;
        var colCount = worksheet.Dimension.Columns;

        for (var row = 2; row <= rowCount; row++)
        {
            var obj = new T();

            for (var col = 1; col <= colCount && col <= properties.Count; col++)
            {
                var prop = properties[col - 1];
                var cellValue = worksheet.Cells[row, col].Text?.Trim();

                if (string.IsNullOrEmpty(cellValue))
                    continue;
                var value = cellValue.ConvertToPropertyType(prop.PropertyType);
                prop.SetValue(obj, value);
            }

            result.Add(obj);
        }

        return result;
    }
}