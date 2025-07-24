interface VoteHistoryResponse {
  voteId: number;
  pollId: number;
  pollTitle: string;
  pollDescription: string;
  pollOptionId: number;
  optionText: string;
  votedAt: string;
}
