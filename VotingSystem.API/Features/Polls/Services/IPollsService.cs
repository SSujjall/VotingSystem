using VotingSystem.API.Features.Polls.DTOs;
using VotingSystem.Common.ResponseModel;
using VotingSystem.Domain.Entities;

namespace VotingSystem.API.Features.Polls.Services
{
    public interface IPollsService
    {
        Task<ApiResponse<PollResponseDTO>> CreatePoll(string userId, CreatePollDTO dto);
        Task<ApiResponse<PollResponseDTO>> UpdatePoll(string userId, UpdatePollDTO dto);
        Task<ApiResponse<List<PollResponseDTO>>> GetAllPolls();
        Task<ApiResponse<PollResponseDTO>> GetPollById(int pollId);
        Task<ApiResponse<string>> DeletePoll(string userId, int pollId);
        Task<ApiResponse<PollResponseDTO>> DisablePoll(int pollId);
        Task<ApiResponse<PollResponseDTO>> EnablePoll(int pollId);
    }
}
