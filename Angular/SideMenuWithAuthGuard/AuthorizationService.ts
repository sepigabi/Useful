import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { ApplicationRoles } from '../Enums/ApplicationRoles';
import { Http, Response } from '@angular/http';
import { ConfigService } from './ConfigService';

@Injectable()
export class AuthorizationService {

    constructor(private _http: Http, private _configService: ConfigService) { }

    getLoggedUserRoles(): Observable<ApplicationRoles[]> {
        return this._http.get(this._configService.getConfigValue("BASE_USER_ENDPOINT") + "GetLoggedUserRoles/")
            .map((respone: Response) => (<string[]>respone.json()).map(name => ApplicationRoles[name]))
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}
