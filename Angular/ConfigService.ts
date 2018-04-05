import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';

export function configFactory(config: ConfigService) {
    return () => config.load();
}

@Injectable()
export class ConfigService {

    private _configValues: Object;
    private _headers: Headers;

    constructor(private _http: Http) {
        this._configValues = new Object();
        this._headers = new Headers({
            "Cache-Control": "no-cache",
            "Pragma": "no-cache",
            "Expires": "Sat, 01 Jan 2000 00:00:00 GMT"
        });
    }

    public load(): Promise<Object> {
        let promise = this._http.get("/app/config.json", { headers: this._headers })
            .map((respone: Response) => {
                return <any>respone.json();
            })
            .toPromise()
            .catch(this.handleError);
        promise.then(configValues => this._configValues = configValues);
        return promise;
    }

    public getConfigValue(key: string): string | null {
        if (key in this._configValues) {
            return this._configValues[key];
        }
        return null;
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}
-------------------------------------------------------------------------------------------------

// in the app.module.ts:

@NgModule({
    imports: [BrowserModule, ReactiveFormsModule, FormsModule, HttpModule, routing ],
    declarations: [AppComponent, HomeComponent ],
    providers: [
        {
            provide: APP_BASE_HREF,
            useValue: '/'
        },
        ConfigService,
        {
            provide: APP_INITIALIZER,
            useFactory: configFactory,
            deps: [ConfigService],
            multi: true
        }
    ],
    bootstrap: [AppComponent]
})
---------------------------------------------------------------------------------------------------

//usage in component:
constructor(private _configService: ConfigService) {
    }
    
this._configService.getConfigValue("redcolor");

reloadConfig() {
        this._configService.load();
    }


