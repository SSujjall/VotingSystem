using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using VotingSystem.API.Features.Hubs;
using VotingSystem.API.Features.Voting.DTOs;
using VotingSystem.Common.ResponseModel;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.API.Features.Voting.Services
{
    public class VoteService : IVoteService
    {
        private readonly IPollRepository _pollRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<VoteHub> _hubContext;

        public VoteService(IPollRepository pollRepository, IVoteRepository voteRepository,
            IMapper mapper, IHubContext<VoteHub> hubContext)
        {
            _pollRepository = pollRepository;
            _voteRepository = voteRepository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<ApiResponse<VoteResponseDTO>> Vote(string userId, VoteRequestDTO dto)
        {
            var poll = await _pollRepository.GetPollById(dto.PollId);
            if (poll is null || !poll.IsActive)
            {
                return ApiResponse<VoteResponseDTO>.Failed(null, "Poll not found or not active");
            }

            if (poll.EndsAt.HasValue && poll.EndsAt.Value < DateTime.Now)
            {
                return ApiResponse<VoteResponseDTO>.Failed(null, "Poll has already ended");
            }

            var hasVoted = await _voteRepository.HasUserVoted(userId, dto.PollId);
            if (hasVoted is true)
            {
                return ApiResponse<VoteResponseDTO>.Failed(null, "You have already voted.");
            }

            var selectedOption = poll.Options.FirstOrDefault(x => x.PollOptionId == dto.PollOptionId);
            if (selectedOption is null)
            {
                return ApiResponse<VoteResponseDTO>.Failed(null, "Selected option does not exist");
            }

            #region request mapping to entity
            var vote = _mapper.Map<Vote>(dto);
            vote.UserId = userId;
            #endregion

            var result = await _voteRepository.Add(vote);
            if (result is null)
            {
                return ApiResponse<VoteResponseDTO>.Failed(null, "Failed to add vote");
            }

            selectedOption.VoteCount++;
            var updateResult = await _pollRepository.Update(poll);
            if (updateResult is null)
            {
                return ApiResponse<VoteResponseDTO>.Failed(null, "Failed to update poll's voting count");
            }

            #region SignalR Update
            await _hubContext.Clients
                .Group($"poll-{poll.PollId}")
                .SendAsync("ReceiveVoteUpdate", new
                {
                    PollId = poll.PollId,
                    Options = poll.Options.Select(o => new
                    {
                        o.PollOptionId,
                        o.OptionText,
                        o.VoteCount
                    }).ToList()
                });
            #endregion

            var mappedRes = _mapper.Map<VoteResponseDTO>(result);
            return ApiResponse<VoteResponseDTO>.Success(mappedRes, "Vote successfully casted");
        }

        public async Task<ApiResponse<string>> RemoveVote(string userId, int pollId)
        {
            var vote = await _voteRepository.GetVoteByUserAndPoll(userId, pollId);
            if (vote is null)
            {
                return ApiResponse<string>.Failed(null, "You have not voted in this poll.");
            }

            var poll = await _pollRepository.GetPollById(pollId);
            if (poll is null)
            {
                return ApiResponse<string>.Failed(null, "Poll not found.");
            }

            var pollOption = poll.Options.FirstOrDefault(x => x.PollOptionId == vote.PollOptionId);
            if (pollOption != null && pollOption.VoteCount > 0)
            {
                pollOption.VoteCount--;
            }

            var updatePoll = await _pollRepository.Update(poll);
            if (updatePoll is null)
            {
                return ApiResponse<string>.Failed(null, "Failed to update poll vote count.");
            }

            var result = await _voteRepository.Delete(vote);
            if (result is false)
            {
                return ApiResponse<string>.Failed(null, "Failed to remove vote.");
            }

            #region SignalR Update
            await _hubContext.Clients
                .Group($"poll-{poll.PollId}")
                .SendAsync("ReceiveVoteUpdate", new
                {
                    PollId = poll.PollId,
                    Options = poll.Options.Select(o => new
                    {
                        o.PollOptionId,
                        o.OptionText,
                        o.VoteCount
                    }).ToList()
                });
            #endregion

            return ApiResponse<string>.Success(null, "Vote removed successfully.");
        }

        public async Task<ApiResponse<object?>> GetUserVote(string userId, int pollId)
        {
            var result = await _voteRepository.GetVoteByUserAndPoll(userId, pollId);
            if (result == null)
            {
                return ApiResponse<object?>.Success(null, "User has not voted in this poll.");
            }

            var resObj = new
            {
                PollOptionId = result.PollOptionId
            };
            return ApiResponse<object?>.Success(resObj, "User vote fetched successfully.");
        }
    }
}
