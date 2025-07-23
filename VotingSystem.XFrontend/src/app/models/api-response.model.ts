export interface ApiResponse<T> {
  status: boolean;
  message: string;
  data: T | null;
  statusCode: number;
  errors?: {
    [key: string]: string;
  };
}