using Application.Wrappers;
using MediatR;
using Models.Response._expenses;

namespace Application.Features.Expenses.Queries
{
    public class GetUserDashboardQuery : IRequest<Response<DashboardResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
