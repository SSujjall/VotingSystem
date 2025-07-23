import { Component } from '@angular/core';
import { HealthResponse } from '../../models/health-response.model';
import { HealthService } from '../../services/health/health.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-health',
  imports: [CommonModule],
  templateUrl: './health.html',
  styleUrl: './health.css',
})
export class Health {
  healthData?: HealthResponse;
  loading = false;
  error = '';

  constructor(private healthService: HealthService) {}

  ngOnInit(): void {
    this.fetchHealth();
  }

  fetchHealth() {
    this.loading = true;
    this.healthService.getHealthStatus().subscribe({
      next: (res) => {
        this.healthData = res;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load health status';
        this.loading = false;
      },
    });
  }
}
