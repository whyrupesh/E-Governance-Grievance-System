import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-landing',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './landing.component.html',
    styleUrls: ['./landing.component.css']
})
export class LandingComponent {

    features = [
        {
            icon: 'report_problem',
            title: 'Easy Grievance Lodging',
            description: 'Submit your grievances in just a few clicks with our intuitive interface. We ensure your voice is heard.'
        },
        {
            icon: 'track_changes',
            title: 'Real-time Tracking',
            description: 'Stay updated with the status of your grievance at every step. Transparency is our priority.'
        },
        {
            icon: 'analytics',
            title: 'Actionable Analytics',
            description: 'Comprehensive dashboards for officials to monitor and expedite the redressal process efficiently.'
        }
    ];

    stats = [
        { value: '10k+', label: 'Citizens Served' },
        { value: '95%', label: 'Resolution Rate' },
        { value: '24/7', label: 'Support Available' }
    ];
}
