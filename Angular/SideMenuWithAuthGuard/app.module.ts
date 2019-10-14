import { NgModule, APP_INITIALIZER } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { AppComponent } from './app.component';
import { routing } from './app.routing';
import { HomeComponent } from './components/home.component';
import { NewLComponent } from './Components/newL/newL.component';
import { ConfigService, configFactory } from './Services/ConfigService';
import { LDetailsComponent } from './Components/lDetails/lDetails.component';
import { AssignmentComponent } from './Components/assignment/assignment.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DependentLComponent } from './Components/dependentL/dependentL.component';
import { ClosedLComponent } from './Components/closedL/closedL.component';
import { AppContextService } from './Services/AppContextService';
import { AuthorizationService } from './Services/AuthorizationService';
import { AuthGuard } from './Helpers/AuthGuard';

@NgModule({
    imports: [BrowserModule, ReactiveFormsModule, FormsModule, HttpModule, routing,  NgbModule.forRoot()],
    declarations: [AppComponent, HomeComponent, NewLComponent, LDetailsComponent, AssignmentComponent, DependentLComponent, ClosedLComponent],
    providers: [
        {
            provide: APP_BASE_HREF,
            useValue: '/'
        },
        AppContextService,
        AuthorizationService,
        AuthGuard,
        {
            provide: APP_INITIALIZER,
            useFactory: configFactory,
            deps: [ConfigService],
            multi: true
        }
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
