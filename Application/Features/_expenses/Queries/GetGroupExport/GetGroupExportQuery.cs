using MediatR;

namespace Application.Features.Expenses.Queries.GetGroupExport
{
    public class GetGroupExportQuery : IRequest<FileResult>
    {
        public Guid GroupId { get; set; }
    }

    public class FileResult
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "text/csv";
        public string FileName { get; set; } = string.Empty;
    }
}
