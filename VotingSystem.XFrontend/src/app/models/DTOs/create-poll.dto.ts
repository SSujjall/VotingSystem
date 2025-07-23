export interface CreatePollDto {
  title: string;
  description: string;
  endsAt: string;
  isActive: boolean;
  options: string[];
}
