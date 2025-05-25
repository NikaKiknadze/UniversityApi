using Microsoft.AspNetCore.Mvc;

namespace University.Application.Services.Excel;

public interface IExcelServices
{
    Task<FileContentResult> ExportToExcel<T>(List<string> columnNames, string fileName, List<T> dataList, CancellationToken cancellationToken);
}