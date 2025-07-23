using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using VotingSystem.API.Features.Hubs;
using VotingSystem.API.Features.Voting.DTOs;
using VotingSystem.API.Features.Voting.Services;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.Tests.Services
{
    public class VoteServiceTests
    {
        private readonly IPollRepository _pollRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<VoteHub> _hubContext;
        private readonly ILogger<VoteService> _logger;
        private readonly VoteService _voteService;

        public VoteServiceTests()
        {
            _pollRepository = A.Fake<IPollRepository>();
            _voteRepository = A.Fake<IVoteRepository>();
            _mapper = A.Fake<IMapper>();
            _hubContext = A.Fake<IHubContext<VoteHub>>();
            _logger = A.Fake<ILogger<VoteService>>();

            _voteService = new VoteService(
                _pollRepository,
                _voteRepository,
                _mapper,
                _logger,
                _hubContext
            );
        }

        [Fact]
        public async Task Vote_PollNotFoundOrInactive_ReturnsFailed()
        {
            // Arrange
            var dto = new VoteRequestDTO { PollId = 1, PollOptionId = 1 };
            A.CallTo(() => _pollRepository.GetPollById(dto.PollId)).Returns(Task.FromResult<Poll>(null));

            // Act
            var result = await _voteService.Vote("user1", dto);

            // Assert
            Assert.False(result.Status);
            Assert.Equal("Poll not found or not active", result.Message);
        }

        [Fact]
        public async Task Vote_PollEnded_ReturnsFailed()
        {
            // Arrange
            var poll = new Poll { PollId = 1, IsActive = true, EndsAt = DateTime.Now.AddMinutes(-1) };
            A.CallTo(() => _pollRepository.GetPollById(poll.PollId)).Returns(poll);

            var dto = new VoteRequestDTO { PollId = 1, PollOptionId = 1 };

            // Act
            var result = await _voteService.Vote("user1", dto);

            // Assert
            Assert.False(result.Status);
            Assert.Equal("Poll has already ended", result.Message);
        }

        [Fact]
        public async Task Vote_UserAlreadyVoted_ReturnsFailed()
        {
            // Arrange
            var poll = new Poll
            {
                PollId = 1,
                IsActive = true,
                EndsAt = DateTime.Now.AddMinutes(10),
                Options = new List<PollOption> { new PollOption { PollOptionId = 1 } }
            };
            A.CallTo(() => _pollRepository.GetPollById(1)).Returns(poll);
            A.CallTo(() => _voteRepository.HasUserVoted("user1", 1)).Returns(true);

            var dto = new VoteRequestDTO { PollId = 1, PollOptionId = 1 };

            // Act
            var result = await _voteService.Vote("user1", dto);

            // Assert
            Assert.False(result.Status);
            Assert.Equal("You have already voted.", result.Message);
        }

        [Fact]
        public async Task Vote_SuccessfulVote_ReturnsSuccess()
        {
            // Arrange
            var option = new PollOption { PollOptionId = 1, OptionText = "Yes", VoteCount = 0 };
            var poll = new Poll
            {
                PollId = 1,
                IsActive = true,
                EndsAt = DateTime.Now.AddMinutes(10),
                Options = new List<PollOption> { option }
            };
            var voteEntity = new Vote { PollId = 1, PollOptionId = 1, UserId = "user1" };

            A.CallTo(() => _pollRepository.GetPollById(1)).Returns(poll);
            A.CallTo(() => _voteRepository.HasUserVoted("user1", 1)).Returns(false);
            A.CallTo(() => _mapper.Map<Vote>(A<VoteRequestDTO>.Ignored)).Returns(voteEntity);
            A.CallTo(() => _voteRepository.Add(voteEntity)).Returns(voteEntity);
            A.CallTo(() => _pollRepository.Update(poll)).Returns(poll);
            A.CallTo(() => _mapper.Map<VoteResponseDTO>(voteEntity)).Returns(new VoteResponseDTO());

            // Stub HubContext.Clients.Group.SendAsync
            var clients = A.Fake<IHubClients>();
            var clientProxy = A.Fake<IClientProxy>();
            A.CallTo(() => _hubContext.Clients).Returns(clients);
            A.CallTo(() => clients.Group("poll-1")).Returns(clientProxy);

            var dto = new VoteRequestDTO { PollId = 1, PollOptionId = 1 };

            // Act
            var result = await _voteService.Vote("user1", dto);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Vote successfully casted", result.Message);
        }
    }
}
