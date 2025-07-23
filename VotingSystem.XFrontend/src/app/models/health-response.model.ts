export interface HealthCheck {
  component: string;
  status: string;
  description: string;
}

export interface HealthResponse {
  overallStatus: string;
  message: string;
  timestamp: string;
  checks: HealthCheck[];
}