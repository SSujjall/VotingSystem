namespace VotingSystem.API.Features.Voting.DTOs
{
    public class VoteRequestDTO
    {
        public int PollId { get; set; }
        public int PollOptionId { get; set; }
    }
}
