using Application.Wrappers;
using MediatR;
using Application.Features._expenses.DTOs;

namespace Application.Features.Expenses.Queries
{
    public class GetUserDashboardQuery : IRequest<Response<DashboardResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
