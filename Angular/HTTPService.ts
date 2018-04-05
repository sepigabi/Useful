@Injectable()
export class LeadService {

    constructor(private _http: Http, private _configService: ConfigService) { }

    getLeadDetails(leadId: number): Observable<ILeadDetails> {
        let params = { leadId: leadId };
        return this._http.get(this._configService.getConfigValue("BASE_USER_ENDPOINT") + "GetLeadDetails/", { params: params })
            .map((respone: Response) => <ILeadDetails>respone.json())
            .catch(this.handleError);
    }

    updateLeadDetails(leadDetails: ILeadDetails): Observable<any> {
        let body = JSON.stringify(leadDetails);
        let headers = new Headers({ "Content-Type": "application/json" });
        let options = new RequestOptions({ headers: headers });
       return this._http.post(this._configService.getConfigValue("BASE_USER_ENDPOINT") + "UpdateLeadDetails/", body, options)
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}
----------------------------------------------------------------------------------------------------

//usage in component:

constructor(private _leadService: LeadService, private _configService: ConfigService) {
    }

    ngOnInit(): void {
        this._leadService.getLeadDetails(leadid).subscribe(details => {
            this.details = details;
        }, error => this.errorMsg = <any>error);
    }
