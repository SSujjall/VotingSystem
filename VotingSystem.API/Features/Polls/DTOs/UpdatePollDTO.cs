namespace VotingSystem.API.Features.Polls.DTOs
{
    public class UpdatePollDTO
    {
        public int PollId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? EndsAt { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string> Options { get; set; } = new();
    }
}
