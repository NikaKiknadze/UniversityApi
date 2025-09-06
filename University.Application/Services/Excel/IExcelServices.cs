using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace University.Application.Services.Excel;

public interface IExcelServices
{
    Task<FileContentResult> ExportToExcel<T>(List<string> columnNames, string fileName, List<T> dataList, CancellationToken cancellationToken, bool isSimpleExport = false);
    ExcelPackage ExpandPackage<T>(ExcelPackage package, List<string> columnNames, string sheetName,
        List<T> dataList, bool isSimpleExport);
    Task<FileContentResult> DownloadTemplate(List<string> columnNames, string fileName, CancellationToken cancellationToken);
    Task<List<T>> GetDataFromExcel<T>(IFormFile file, CancellationToken cancellationToken)  where T : new();
}