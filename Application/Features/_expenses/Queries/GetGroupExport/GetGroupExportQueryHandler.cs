using Application.Interfaces;
using ClosedXML.Excel;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Features.Expenses.Queries.GetGroupExport
{
    public class GetGroupExportQueryHandler : IRequestHandler<GetGroupExportQuery, FileResult>
    {
        private readonly IApplicationDbContext _context;

        public GetGroupExportQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FileResult> Handle(GetGroupExportQuery request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups
                .Include(g => g.Members)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

            if (group == null) throw new KeyNotFoundException("Círculo no encontrado");

            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.Payments)
                .ThenInclude(p => p.User)
                .Include(e => e.Splits)
                .ThenInclude(s => s.User)
                .Where(e => e.GroupId == request.GroupId)
                .OrderBy(e => e.Created)
                .ToListAsync(cancellationToken);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte de Gastos");
                var currentRow = 1;

                // --- TÍTULO Y ENCABEZADO ---
                var titleCell = worksheet.Cell(currentRow, 1);
                titleCell.Value = $"REPORTE DE GASTOS: {group.Name}";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                titleCell.Style.Font.FontColor = XLColor.FromHtml("#1a73e8");
                currentRow++;

                worksheet.Cell(currentRow, 1).Value = $"Fecha de Exportación: {DateTime.Now:dd/MM/yyyy HH:mm}";
                worksheet.Cell(currentRow, 1).Style.Font.Italic = true;
                currentRow += 2;

                // --- TABLA 1: DISTRIBUCIÓN POR CATEGORÍA (Resumen para gráfico) ---
                worksheet.Cell(currentRow, 1).Value = "RESUMEN POR CATEGORÍA";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Range(currentRow, 1, currentRow, 2).Merge().Style.Fill.BackgroundColor = XLColor.FromHtml("#f8f9fa");
                currentRow++;

                var catHeader = worksheet.Range(currentRow, 1, currentRow, 2);
                catHeader.Cell(1, 1).Value = "Categoría";
                catHeader.Cell(1, 2).Value = "Total Gastado";
                catHeader.Style.Font.Bold = true;
                catHeader.Style.Fill.BackgroundColor = XLColor.FromHtml("#e8f0fe");
                currentRow++;

                var categoryData = expenses
                    .GroupBy(e => e.Category?.Name ?? "General")
                    .Select(g => new { Name = g.Key, Total = g.Sum(e => (decimal)e.Amount.Amount) })
                    .OrderByDescending(x => x.Total);

                foreach (var cat in categoryData)
                {
                    worksheet.Cell(currentRow, 1).Value = cat.Name;
                    worksheet.Cell(currentRow, 2).Value = cat.Total;
                    worksheet.Cell(currentRow, 2).Style.NumberFormat.Format = "$ #,##0.00";
                    currentRow++;
                }
                currentRow += 2;

                // --- TABLA 2: DETALLE DE GASTOS ---
                worksheet.Cell(currentRow, 1).Value = "DETALLE DE MOVIMIENTOS";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Range(currentRow, 1, currentRow, 6).Merge().Style.Fill.BackgroundColor = XLColor.FromHtml("#f8f9fa");
                currentRow++;

                var headerRange = worksheet.Range(currentRow, 1, currentRow, 6);
                headerRange.Cell(1, 1).Value = "Fecha";
                headerRange.Cell(1, 2).Value = "Categoría";
                headerRange.Cell(1, 3).Value = "Descripción";
                headerRange.Cell(1, 4).Value = "Pagado por";
                headerRange.Cell(1, 5).Value = "Monto Total";
                headerRange.Cell(1, 6).Value = "División";
                
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a73e8");
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                currentRow++;

                foreach (var exp in expenses)
                {
                    var payers = string.Join(", ", exp.Payments.Select(p => $"{(p.User != null ? p.User.FirstName : "Anon")} (${p.AmountPaid:N0})"));
                    var splitType = exp.Splits.FirstOrDefault()?.SplitType switch
                    {
                        SplitTypeEnum.Equal => "Equitativo",
                        SplitTypeEnum.Percentage => "Porcentaje",
                        SplitTypeEnum.Exact => "Exacto",
                        _ => "Equitativo"
                    };

                    worksheet.Cell(currentRow, 1).Value = exp.Created;
                    worksheet.Cell(currentRow, 2).Value = exp.Category?.Name ?? "General";
                    worksheet.Cell(currentRow, 3).Value = exp.Title;
                    worksheet.Cell(currentRow, 4).Value = payers;
                    worksheet.Cell(currentRow, 5).Value = exp.Amount.Amount;
                    worksheet.Cell(currentRow, 6).Value = splitType;

                    // Estilos de celda
                    worksheet.Cell(currentRow, 1).Style.NumberFormat.Format = "dd/mm/yyyy";
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "$ #,##0.00";
                    
                    if (currentRow % 2 == 0) worksheet.Range(currentRow, 1, currentRow, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#f1f3f4");
                    
                    currentRow++;
                }
                currentRow += 2;

                // --- TABLA 3: RESUMEN DE BALANCES ---
                worksheet.Cell(currentRow, 1).Value = "ESTADO DE CUENTAS POR MIEMBRO";
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Range(currentRow, 1, currentRow, 4).Merge().Style.Fill.BackgroundColor = XLColor.FromHtml("#f8f9fa");
                currentRow++;

                var balanceHeader = worksheet.Range(currentRow, 1, currentRow, 4);
                balanceHeader.Cell(1, 1).Value = "Miembro";
                balanceHeader.Cell(1, 2).Value = "Total Pagado";
                balanceHeader.Cell(1, 3).Value = "Total Gastado";
                balanceHeader.Cell(1, 4).Value = "Balance Neto";
                
                balanceHeader.Style.Font.Bold = true;
                balanceHeader.Style.Fill.BackgroundColor = XLColor.FromHtml("#e8f0fe");
                currentRow++;

                var members = group.Members.Select(m => m.User).Where(u => u != null).ToList();
                foreach (var user in members)
                {
                    var totalPaid = expenses.SelectMany(e => e.Payments).Where(p => p.UserId == user.Id).Sum(p => p.AmountPaid);
                    var totalOwed = expenses.SelectMany(e => e.Splits).Where(s => s.UserId == user.Id).Sum(s => s.AmountOwed);
                    var balance = totalPaid - totalOwed;

                    worksheet.Cell(currentRow, 1).Value = $"{user.FirstName} {user.LastName}";
                    worksheet.Cell(currentRow, 2).Value = totalPaid;
                    worksheet.Cell(currentRow, 3).Value = totalOwed;
                    worksheet.Cell(currentRow, 4).Value = balance;

                    worksheet.Range(currentRow, 2, currentRow, 4).Style.NumberFormat.Format = "$ #,##0.00";
                    if (balance < 0) worksheet.Cell(currentRow, 4).Style.Font.FontColor = XLColor.Red;
                    else if (balance > 0) worksheet.Cell(currentRow, 4).Style.Font.FontColor = XLColor.Green;

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return new FileResult
                    {
                        Content = content,
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        FileName = $"Reporte_{group.Name}_{DateTime.Now:yyyyMMdd}.xlsx"
                    };
                }
            }
        }
    }
}
