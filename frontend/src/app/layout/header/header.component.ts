import { Component, Output, EventEmitter, OnInit, HostListener, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { NotificationService } from '../../core/services/notification.service';
import { SearchService } from '../../core/services/search.service';
import { UserProfile } from '../../core/models/user.model';
import { NotificationDto } from '../../core/models/notification.model';
import { SearchResultItemDto } from '../../core/models/search.model';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  @Output() toggleSidebar = new EventEmitter<void>();

  currentUser: UserProfile = {
    name: 'System Admin',
    email: 'admin@raras.com',
    role: 'Administrator',
    initials: 'SA'
  };

  isProfileMenuOpen: boolean = false;

  // Notifications state
  isNotificationsOpen: boolean = false;
  notifications: NotificationDto[] = [];
  unreadCount: number = 0;

  // Search state
  searchQuery: string = '';
  isSearchOpen: boolean = false;
  isSearching: boolean = false;
  searchResults: SearchResultItemDto[] = [];

  constructor(
    private authService: AuthService,
    private notificationService: NotificationService,
    private searchService: SearchService,
    private router: Router,
    private elementRef: ElementRef
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.currentUser = user;
      }
    });

    this.notificationService.unreadCount$.subscribe(count => {
      this.unreadCount = count;
    });

    this.loadNotifications();
  }

  onToggleSidebar(): void {
    this.toggleSidebar.emit();
  }

  toggleProfileMenu(): void {
    this.isProfileMenuOpen = !this.isProfileMenuOpen;
    this.isNotificationsOpen = false;
    this.isSearchOpen = false;
  }

  toggleNotifications(): void {
    this.isNotificationsOpen = !this.isNotificationsOpen;
    this.isProfileMenuOpen = false;
    this.isSearchOpen = false;
    if (this.isNotificationsOpen) {
      this.loadNotifications();
    }
  }

  loadNotifications(): void {
    this.notificationService.getNotifications().subscribe(data => {
      this.notifications = data;
    });
  }

  markNotificationAsRead(id: number, event: Event): void {
    event.stopPropagation();
    this.notificationService.markAsRead(id).subscribe(() => {
      const item = this.notifications.find(n => n.id === id);
      if (item) item.isRead = true;
    });
  }

  markAllNotificationsAsRead(): void {
    this.notificationService.markAllAsRead().subscribe(() => {
      this.notifications.forEach(n => n.isRead = true);
    });
  }

  onSearchInput(): void {
    if (!this.searchQuery.trim()) {
      this.isSearchOpen = false;
      this.searchResults = [];
      return;
    }

    this.isSearching = true;
    this.isSearchOpen = true;

    this.searchService.search(this.searchQuery).subscribe(res => {
      this.searchResults = res.results || [];
      this.isSearching = false;
    });
  }

  navigateToResult(item: SearchResultItemDto): void {
    this.isSearchOpen = false;
    this.searchQuery = '';
    if (item.linkUrl) {
      this.router.navigateByUrl(item.linkUrl);
    }
  }

  onLogout(): void {
    this.isProfileMenuOpen = false;
    this.authService.logout();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isProfileMenuOpen = false;
      this.isNotificationsOpen = false;
      this.isSearchOpen = false;
    }
  }
}
