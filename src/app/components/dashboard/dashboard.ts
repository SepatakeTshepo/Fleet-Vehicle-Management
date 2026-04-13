import { Component, OnInit, inject , ChangeDetectorRef} from '@angular/core'
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
  private VehicleService = inject(VehicleService);
  private cdr = inject(ChangeDetectorRef);

  stats: VehicleStats = {
    Total: 0,
    Critical: 0,
    Pending: 0,
    Cleared: 0
  };

  ngOnInit(): void {
    this.VehicleService.refreshStatsSignal.subscribe(() => {
      this.loadStats();
    });
  }

  loadStats() {
    this.VehicleService.getStats().subscribe({
      next: (data) => {
        this.stats = data;
        // Wake Angular up to draw the numbers instantly!
        this.cdr.detectChanges(); 
      },
      error: (err) => console.error("Failed to load dashboard stats", err)
    });
  }
}