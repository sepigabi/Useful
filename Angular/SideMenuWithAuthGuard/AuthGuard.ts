import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { AppContextService } from "../Services/AppContextService";
import { ApplicationRoles } from "../Enums/ApplicationRoles";
import { Injectable } from "@angular/core";

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private _router: Router, private _appContext: AppContextService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const requiredRoles = (<any>route.data).roles;
        if (requiredRoles) {
            return this._appContext.getLoggedUserRoles().map(loggedUserRoles => {
                return this.checkUserPermission(requiredRoles, loggedUserRoles);
            });
        }
        return true;
    }

    private checkUserPermission(requiredRoles: any, availableRoles: ApplicationRoles[]): boolean {
        for (var i = 0; i < requiredRoles.length; i++) {
            if (availableRoles.indexOf(requiredRoles[i]) !== -1) {
                return true;
            }
        }
        this._router.navigate(['/']);
        return false;
    }
}
