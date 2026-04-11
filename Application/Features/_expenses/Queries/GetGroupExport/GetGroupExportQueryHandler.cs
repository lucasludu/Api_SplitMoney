using Application.Interfaces;
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

            var sb = new StringBuilder();

            // Header for info
            sb.AppendLine($"REPORT DE GASTOS PRO: {group.Name}");
            sb.AppendLine($"Fecha de Exportacion: {DateTime.Now:dd/MM/yyyy}");
            sb.AppendLine();

            // Table 1: Expenses
            sb.AppendLine("--- DETALLE DE GASTOS ---");
            sb.AppendLine("Fecha,Categoria,Descripcion,Quien Pago,Monto Total,Como se dividio");
            
            foreach (var exp in expenses)
            {
                var payers = string.Join(" | ", exp.Payments.Select(p => $"{(p.User != null ? string.Concat(p.User.FirstName, " ", p.User.LastName) : p.UserId)} (${p.AmountPaid:N2})"));

                var splitValue = exp.Splits.FirstOrDefault()?.SplitType;
                var splitType = "Equitativo";

                if (splitValue.HasValue)
                {
                    splitType = splitValue.Value switch
                    {
                        SplitTypeEnum.Equal => "Equitativo",
                        SplitTypeEnum.Percentage => "Porcentaje",
                        SplitTypeEnum.Exact => "Exacto",
                        _ => "Equitativo"
                    };
                }

                sb.AppendLine($"{exp.Created:dd/MM/yyyy},{exp.Category?.Name ?? "General"},\"{exp.Title}\",\"{payers}\",{exp.Amount.Amount:N2},{splitType}");
            }

            sb.AppendLine();
            
            // Table 2: Member Balances 
            sb.AppendLine("--- RESUMEN DE BALANCES (AL CIERRE) ---");
            sb.AppendLine("Miembro,Total Pagado (Puso),Total Gastado (Debe),Balance Neto");

            var members = group.Members.Select(m => m.User).Where(u => u != null).ToList();
            foreach (var user in members)
            {
                var totalPaid = expenses.SelectMany(e => e.Payments).Where(p => p.UserId == user.Id).Sum(p => p.AmountPaid);
                var totalOwed = expenses.SelectMany(e => e.Splits).Where(s => s.UserId == user.Id).Sum(s => s.AmountOwed);
                var balance = totalPaid - totalOwed;

                sb.AppendLine($"\"{string.Concat(user.FirstName, " ", user.LastName)}\",{totalPaid:N2},{totalOwed:N2},{balance:N2}");
            }

            return new FileResult
            {
                Content = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray(),
                ContentType = "text/csv",
                FileName = $"Reporte_{group.Name}_{DateTime.Now:yyyyMMdd}.csv"
            };
        }
    }
}
