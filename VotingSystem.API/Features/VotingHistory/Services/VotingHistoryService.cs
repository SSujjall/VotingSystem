using AutoMapper;
using VotingSystem.API.Features.VotingHistory.DTOs;
using VotingSystem.Common.ResponseModel;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.API.Features.VotingHistory.Services
{
    public class VotingHistoryService : IVotingHistoryService
    {
        private readonly IVoteRepository _voteRepository;

        public VotingHistoryService(IVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<ApiResponse<IEnumerable<VotingHistoryResDTO>>> GetVotingHistory(string userId)
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
    }
}
