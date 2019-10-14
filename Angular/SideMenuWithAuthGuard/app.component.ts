import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from "@angular/core"
import { AppContextService } from "./Services/AppContextService";
import { Subscription } from "rxjs/Subscription";
import { AuthorizationService } from "./Services/AuthorizationService";
import { ApplicationRoles } from "./Enums/ApplicationRoles";

@Component({
    selector: "user-app",
    templateUrl: "./app.component.html",
    styleUrls: [
        "./app.component.css"
    ]
})

export class AppComponent implements OnInit, OnDestroy{
    subscription: Subscription = new Subscription();
    errorMsg: string;
    isUser: boolean;
    isStaff: boolean;
    isManager: boolean;
    isAdministrator: boolean;
    @ViewChild("header") header: ElementRef;
    @ViewChild("title") title: ElementRef;

    constructor(private _appContext: AppContextService, private _authorizationService: AuthorizationService,) { }

    ngOnInit(): void {
        let loggedUserRolesSubscription = this._authorizationService.getLoggedUserRoles().subscribe(loggedUserRoles => {
            this._appContext.loggedUserRolesValue = loggedUserRoles;
            this.setUserRoleProperties(loggedUserRoles);
        }, error => this.errorMsg = <any>error);
        this.subscription.add(loggedUserRolesSubscription);
    }

    ngOnDestroy(): void {
        this._appContext.loggedUserRolesValue = [];
        this.subscription ? this.subscription.unsubscribe() : null;
    }

    handleNavigation(title: string) {
        this.header.nativeElement.focus();
        this.title.nativeElement.innerText = title;
    }

    private setUserRoleProperties(loggedUserRoles: ApplicationRoles[]) {
        if (loggedUserRoles.indexOf(ApplicationRoles.User) !== -1) {
            this.isUser = true;
        }
        else {
            this.isUser = false;
        }
        if (loggedUserRoles.indexOf(ApplicationRoles.Staff) !== -1) {
            this.isStaff = true;
        }
        else {
            this.isStaff = false;
        }
        if (loggedUserRoles.indexOf(ApplicationRoles.Manager) !== -1) {
            this.isManager = true;
        }
        else {
            this.isManager = false;
        }
        if (loggedUserRoles.indexOf(ApplicationRoles.Administrator) !== -1) {
            this.isAdministrator = true;
        }
        else {
            this.isAdministrator = false;
        }
    }
}             
