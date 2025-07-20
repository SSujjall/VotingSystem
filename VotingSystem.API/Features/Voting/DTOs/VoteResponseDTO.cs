namespace VotingSystem.API.Features.Voting.DTOs
{
    public class VoteResponseDTO
    {
        public int VoteId { get; set; }
        public int PollId { get; set; }
        public int PollOptionId { get; set; }
        public DateTime VotedAt { get; set; }
    }
}
