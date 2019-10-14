import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs/ReplaySubject';
import { Observable } from 'rxjs/Observable';
import { ApplicationRoles } from '../Enums/ApplicationRoles';

@Injectable()
export class AppContextService {

    private _loggedUserRoles: ReplaySubject<ApplicationRoles[]> = new ReplaySubject(1);
    constructor() { }

    getLoggedUserRoles(): Observable<ApplicationRoles[]> { return this._loggedUserRoles; }
    set loggedUserRolesValue(value: ApplicationRoles[]) { this._loggedUserRoles.next(value); }
}
