import { Component, OnInit, ElementRef, ViewChild } from "@angular/core";
import { Location } from "@angular/common";
import { IFoo } from "../../Models/IFoo";
import { ConfigService } from "../../Services/ConfigService";
import { FooService } from "../../Services/FooService";
import { IStatus } from "../../Models/IStatus";
import { IAgency } from "../../Models/IAgency";
import { IFooDetails } from "../../Models/IFooDetails";
import { ActivatedRoute } from "@angular/router";
import { FormGroup, FormControl, ValidatorFn, AbstractControl, Validators } from "@angular/forms";


@Component({
    selector: "fooDetails",
    templateUrl: "app/Components/fooDetails/fooDetails.component.html",
    styleUrls: [
        "app/Components/fooDetails/fooDetails.component.css"
    ],
})

export class FooDetailsComponent implements OnInit {
    states: IStatus[];
    agencies: IAgency[];
    model: IFooDetails;
    errorMsg: string;
    nullableBooleanValues: Object[];
    fooDetailsForm: FormGroup;
    successSave: boolean | null;

    @ViewChild("details") detailsContainer: ElementRef;

    constructor(private _fooService: FooService, private _configService: ConfigService, private _route: ActivatedRoute, private _location: Location) {
    }

    ngOnInit(): void {
        let fooid = parseInt(this._route.snapshot.paramMap.get("fooid"));
        let dmcode = this._route.snapshot.paramMap.get("dmcode");

        this.fooDetailsForm = new FormGroup({
            "comment": new FormControl("", [this.commentValidator()]),
            "status": new FormControl(""),
            "contacted": new FormControl("", this.contactedValidator()),
            "agency": new FormControl("", this.agencyValidator()),
            "validFoo": new FormControl("")
        });

        this._fooService.getStates().subscribe(states => {
            this.states = states;
            this.states.unshift({ "Id": null, "Name": "-", "Code": "null" });
        }, error => this.errorMsg = <any>error);

        this._fooService.getAgencies(dmcode).subscribe(agencies => {
            this.agencies = agencies;
        }, error => this.errorMsg = <any>error);

        this._fooService.getFooDetails(fooid).subscribe(model => {
            this.model = model;
            this.fooDetailsForm.controls["comment"].setValue(this.model.Comment);
            this.fooDetailsForm.controls["status"].setValue(this.model.CurrentStatusId);
            this.fooDetailsForm.controls["contacted"].setValue(this.model.Contacted);
            this.fooDetailsForm.controls["agency"].setValue(this.model.Agency_Id);
            this.fooDetailsForm.controls["validFoo"].setValue(this.model.ValidFoo);
        }, error => this.errorMsg = <any>error);

        this.nullableBooleanValues = [
            { "Value": null, "Name": "-" },
            { "Value": true, "Name": "Igen" },
            { "Value": false, "Name": "Nem" }
        ];

        this.detailsContainer.nativeElement.scrollIntoView();
        this.successSave = null;
    }

    get comment() { return this.fooDetailsForm ? this.fooDetailsForm.get("comment") : null; }
    get status() { return this.fooDetailsForm ? this.fooDetailsForm.get("status") : null; }
    get contacted() { return this.fooDetailsForm ? this.fooDetailsForm.get("contacted") : null; }
    get agency() { return this.fooDetailsForm ? this.fooDetailsForm.get("agency") : null; }
    get validFoo() { return this.fooDetailsForm ? this.fooDetailsForm.get("validFoo") : null; }


    commentValidator(): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } => {
            const dependantState = (control.value === null || control.value === "") &&
                this.status &&
                this.states &&
                this.status.value !== null &&
                this.states.filter(s => s.Code.toLowerCase() === "p")[0].Id === this.status.value;
            if (dependantState) {
                return { "commentDependantState": { value: control.value } };
            }
            const validFoo = (control.value === null || control.value === "") &&
                this.validFoo &&
                this.validFoo.value === false
            if (validFoo) {
                return { "commentValidFoo": { value: control.value } };
            }
            return null;
        };
    }

    agencyValidator(): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } => {
            const stateAdjusted = control.value === null &&
                this.status &&
                this.states &&
                this.status.value !== null &&
                this.states.filter(s => s.Code.toLowerCase() === "f")[0].Id !== this.status.value;
            if (stateAdjusted) {
                return { "agencyState": { value: control.value } };
            }
            const validFoo = (control.value === null || control.value === "") &&
                this.validFoo &&
                (this.validFoo.value === false || this.validFoo.value === true)
            if (validFoo) {
                return { "agencyValidFoo": { value: control.value } };
            }
            const contacted = (control.value === null || control.value === "") &&
                this.contacted &&
                (this.contacted.value === false || this.contacted.value === true)
            if (contacted) {
                return { "agencyContacted": { value: control.value } };
            }
            return null;
        };
    }

    contactedValidator(): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } => {
            const stateAdjusted = control.value === null &&
                this.status &&
                this.states &&
                this.status.value !== null &&
                this.states.filter(s => s.Code.toLowerCase() === "f")[0].Id !== this.status.value;
            if (stateAdjusted) {
                return { "contactedState": { value: control.value } };
            }
            const validFoo = (control.value === null || control.value === "") &&
                this.validFoo &&
                (this.validFoo.value === false || this.validFoo.value === true)
            if (validFoo) {
                return { "contactedValidFoo": { value: control.value } };
            }
            return null;
        };
    }

    formValueChanged(event: Event) {
        this.comment.updateValueAndValidity();
        this.contacted.updateValueAndValidity();
        this.agency.updateValueAndValidity();
    }

    goBack() {
        this._location.back();
    }

    save() {
        if (this.fooDetailsForm.valid) {
            this.model.Comment = this.comment.value;
            this.model.CurrentStatusId = this.status.value;
            this.model.Contacted = this.contacted.value;
            this.model.Agency_Id = this.agency.value;
            this.model.ValidFoo = this.validFoo.value;

            this._fooService.updateFooDetails(this.model).subscribe(response => {
                this.successSave = true;
                setTimeout(() => {
                    this.goBack();
                }, 3000);
            }, error => {
                this.errorMsg = <any>error;
                this.successSave = false;
            });
        }
    }
}
