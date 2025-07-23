interface VoteData {
  voteId: number;
  pollId: number;
  pollOptionId: number;
  votedAt: string; // ISO datetime string
}

interface UserVoteData {
  pollOptionId: number;
}