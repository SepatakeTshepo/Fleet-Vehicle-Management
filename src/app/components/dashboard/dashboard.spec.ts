import { Component, OnInit, inject } from '@angular/core'
import { CommonModule } from '@angular/common'
import { RouterLink } from '@angular/router'
import { VehicleService } from '../../Service/Vehicle.service'
import { VehicleStats } from '../../Models/vehicle.models'

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  private vehicleService = inject(VehicleService)

  stats: VehicleStats = {
    Total: 0,
    Critical: 0,
    Pending: 0,
    Cleared: 0
  }

  loading = true

  ngOnInit() {
    this.vehicleService.getStats().subscribe({
      next: (data) => {
        this.stats = data
        this.loading = false
      },
      error: (err) => {
        console.error('Failed to load stats', err)
        this.loading = false
      }
    })
  }
}