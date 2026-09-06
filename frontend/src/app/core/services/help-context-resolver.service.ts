import { Injectable } from '@angular/core';
import { Router, NavigationEnd, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, BehaviorSubject, filter } from 'rxjs';

export interface HelpContextKeys {
  module: string;
  page: string;
  functionality?: string;
}

@Injectable({
  providedIn: 'root'
})
export class HelpContextResolverService {
  private readonly currentKeysSubject = new BehaviorSubject<HelpContextKeys>(this.resolveRouteKeys());
  public readonly currentKeys$: Observable<HelpContextKeys> = this.currentKeysSubject.asObservable();

  constructor(private readonly router: Router) {
    this.router.events.pipe(
      filter((event): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.currentKeysSubject.next(this.resolveRouteKeys());
    });
  }

  public getCurrentKeys(): HelpContextKeys {
    return this.resolveRouteKeys();
  }

  private resolveRouteKeys(): HelpContextKeys {
    let route: ActivatedRouteSnapshot | null = this.router.routerState?.snapshot?.root || null;
    const mergedData: Record<string, any> = {};

    while (route) {
      if (route.data) {
        Object.assign(mergedData, route.data);
      }
      route = route.firstChild;
    }

    let moduleKey = mergedData['module'] || '';
    let pageKey = mergedData['page'] || '';
    let functionalityKey = mergedData['functionality'] || undefined;

    // Fallback: If route data was not present during initial render, deduce from URL path
    if (!moduleKey || !pageKey) {
      const urlPath = this.router.url || (typeof window !== 'undefined' ? window.location.pathname : '');
      const cleanPath = urlPath.toLowerCase().split('?')[0].split('#')[0];

      if (cleanPath.includes('/dashboard')) {
        moduleKey = moduleKey || 'dashboard';
        pageKey = pageKey || 'overview';
      } else if (cleanPath.includes('/employees')) {
        moduleKey = moduleKey || 'employees';
        pageKey = pageKey || 'employee-list';
      } else if (cleanPath.includes('/departments')) {
        moduleKey = moduleKey || 'departments';
        pageKey = pageKey || 'department-list';
      } else if (cleanPath.includes('/attendance')) {
        moduleKey = moduleKey || 'attendance';
        pageKey = pageKey || 'attendance-list';
      } else if (cleanPath.includes('/leave')) {
        moduleKey = moduleKey || 'leave';
        pageKey = pageKey || 'leave-list';
      } else if (cleanPath.includes('/payroll')) {
        moduleKey = moduleKey || 'payroll';
        pageKey = pageKey || 'payroll-list';
      } else if (cleanPath.includes('/login')) {
        moduleKey = moduleKey || 'auth';
        pageKey = pageKey || 'login';
        functionalityKey = functionalityKey || 'login-form';
      }
    }

    return {
      module: moduleKey,
      page: pageKey,
      functionality: functionalityKey
    };
  }
}
