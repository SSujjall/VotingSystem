namespace VotingSystem.API.Features.VotingHistory.DTOs
{
    public class VotingHistoryResDTO
    {
        public int VoteId { get; set; }
        public int PollId { get; set; }
        public string PollTitle { get; set; } = string.Empty;
        public string PollDescription { get; set; }
        public int PollOptionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public DateTime VotedAt { get; set; }
    }
}
