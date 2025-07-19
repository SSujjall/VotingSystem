namespace VotingSystem.API.Features.Polls.DTOs
{
    public class PollResponseDTO
    {
        public int PollId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndsAt { get; set; }

        public List<PollOptionResponseDTO> Options { get; set; } = new();
    }

    public class PollOptionResponseDTO
    {
        public int PollOptionId { get; set; }
        public string OptionText { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public int VoteCount { get; set; }
    }
}
