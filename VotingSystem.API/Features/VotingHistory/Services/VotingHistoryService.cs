using AutoMapper;
using VotingSystem.API.Features.VotingHistory.DTOs;
using VotingSystem.Common.ResponseModel;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.API.Features.VotingHistory.Services
{
    public class VotingHistoryService : IVotingHistoryService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly ILogger<VotingHistoryService> _logger;

        public VotingHistoryService(IVoteRepository voteRepository, ILogger<VotingHistoryService> logger)
        {
            _voteRepository = voteRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<VotingHistoryResDTO>>> GetVotingHistory(string userId)
        {
            try
            {
                var votes = await _voteRepository.GetUserVotes(userId);
                if (votes == null || votes.Count == 0)
                {
                    return ApiResponse<IEnumerable<VotingHistoryResDTO>>.Failed(null, "No voting history found");
                }

                #region response mapping
                var mapped = votes.Select(vote => new VotingHistoryResDTO
                {
                    VoteId = vote.VoteId,
                    PollId = vote.PollId,
                    PollTitle = vote.Poll.Title,
                    PollDescription = vote.Poll.Description ?? "",
                    PollOptionId = vote.PollOptionId,
                    OptionText = vote.PollOption.OptionText,
                    VotedAt = vote.VotedAt
                }).ToList();
                #endregion

                return ApiResponse<IEnumerable<VotingHistoryResDTO>>.Success(mapped, "Voting history fetched successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetVotingHistory");
                return ApiResponse<IEnumerable<VotingHistoryResDTO>>.Failed(null, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
