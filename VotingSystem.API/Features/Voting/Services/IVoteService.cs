using VotingSystem.API.Features.Voting.DTOs;
using VotingSystem.Common.ResponseModel;

namespace VotingSystem.API.Features.Voting.Services
{
    public interface IVoteService
    {
        Task<ApiResponse<VoteResponseDTO>> Vote(string userId, VoteRequestDTO dto);
        Task<ApiResponse<string>> RemoveVote(string userId, int pollId);
    }
}
