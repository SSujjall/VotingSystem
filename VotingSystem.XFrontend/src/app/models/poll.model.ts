export interface PollOption {
  pollOptionId: number;
  optionText: string;
  imagePath?: string | null;
  voteCount: number;
}

export interface Poll {
  pollId: number;
  createdBy: string;
  title: string;
  description: string;
  isActive: boolean;
  createdAt: string; // ISO date string
  endsAt: string; // ISO date string
  options: PollOption[];
}
