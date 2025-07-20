using VotingSystem.API.Features.VotingHistory.DTOs;
using VotingSystem.Common.ResponseModel;

namespace VotingSystem.API.Features.VotingHistory.Services
{
    public interface IVotingHistoryService
    {
        Task<ApiResponse<IEnumerable<VotingHistoryResDTO>>> GetVotingHistory(string userId);
    }
}
