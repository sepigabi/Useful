import { validate } from "../../Helpers/BankAccountValidator";

bankAccountNumberFirstpartValidator(): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } => {
            if (control) {
                const field1 = this.bankAccountNumber_firstpartInput ? this.bankAccountNumber_firstpartInput.value : null;
                const field2 = this.bankAccountNumber_secondpartInput ? this.bankAccountNumber_secondpartInput.value : null;
                const field3 = this.bankAccountNumber_thirdpartInput ? this.bankAccountNumber_thirdpartInput.value : null;

                const validationResult = validate(field1, field2, field3);
                if (validationResult) {
                    if (validationResult["RequiredFields"] && (<Array<string>>validationResult["RequiredFields"]).indexOf("field1") !== -1) {
                        return { "RequiredField": { value: control.value } };
                    }
                    else if (validationResult["InvalidFormats"] && (<Array<string>>validationResult["InvalidFormats"]).indexOf("field1") !== -1) {
                        return { "InvalidFormat": { value: control.value } };
                    }
                }
                return null;
            }
            return null;
        };
    }

    bankAccountNumberSecondpartValidator(): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } => {
            if (control) {
                const field1 = this.bankAccountNumber_firstpartInput ? this.bankAccountNumber_firstpartInput.value : null;
                const field2 = this.bankAccountNumber_secondpartInput ? this.bankAccountNumber_secondpartInput.value : null;
                const field3 = this.bankAccountNumber_thirdpartInput ? this.bankAccountNumber_thirdpartInput.value : null;

                const validationResult = validate(field1, field2, field3);
                if (validationResult) {
                    if (validationResult["RequiredFields"] && (<Array<string>>validationResult["RequiredFields"]).indexOf("field2") !== -1) {
                        return { "RequiredField": { value: control.value } };
                    }
                    else if (validationResult["InvalidFormats"] && (<Array<string>>validationResult["InvalidFormats"]).indexOf("field2") !== -1) {
                        return { "InvalidFormat": { value: control.value } };
                    }
                }
                return null;
            }
            return null;
        };
    }

    bankAccountNumberThirdpartValidator(): ValidatorFn {
        return (control: AbstractControl): { [key: string]: any } => {
            if (control) {
                const field1 = this.bankAccountNumber_firstpartInput ? this.bankAccountNumber_firstpartInput.value : null;
                const field2 = this.bankAccountNumber_secondpartInput ? this.bankAccountNumber_secondpartInput.value : null;
                const field3 = this.bankAccountNumber_thirdpartInput ? this.bankAccountNumber_thirdpartInput.value : null;

                const validationResult = validate(field1, field2, field3);
                if (validationResult) {
                    if (validationResult["RequiredFields"] && (<Array<string>>validationResult["RequiredFields"]).indexOf("field3") !== -1) {
                        return { "RequiredField": { value: control.value } };
                    }
                    else if (validationResult["InvalidFormats"] && (<Array<string>>validationResult["InvalidFormats"]).indexOf("field3") !== -1) {
                        return { "InvalidFormat": { value: control.value } };
                    }
                }
                return null;
            }
            return null;
        };
    }
    
    bankAccountNumber_firstpartChanged(event: Event) {
        const value = this.bankAccountNumber_firstpartInput.value;
        if (value && value.length === 8) {
            this.BankAccountNumberSecondpart.nativeElement.focus();
        }
        this.bankAccountNumber_secondpartInput.updateValueAndValidity();
        this.bankAccountNumber_thirdpartInput.updateValueAndValidity();
    }

    bankAccountNumber_secondpartChanged(event: Event) {
        const value = this.bankAccountNumber_secondpartInput.value;
        if (value && value.length === 8) {
            this.BankAccountNumberThirdpart.nativeElement.focus();
        }
        this.bankAccountNumber_firstpartInput.updateValueAndValidity();
        this.bankAccountNumber_thirdpartInput.updateValueAndValidity();
    }

    bankAccountNumber_thirdpartChanged(event: Event) {
        this.bankAccountNumber_firstpartInput.updateValueAndValidity();
        this.bankAccountNumber_secondpartInput.updateValueAndValidity();
    }
    
 --------------------------------------------------------------------------------------------------------------------------------
    
     <div class="inlineControls">
                    <div class="form-group nopadding col-xl-2 col-lg-3" [ngClass]="{'has-danger': bankAccountNumber_firstpartInput.invalid}">
                        <label for="bankAccountNumber_firstpart">Bankszámla száma</label>
                        <input type="text" formControlName="bankAccountNumber_firstpartControl" id="bankAccountNumber_firstpart" class="form-control" [ngClass]="{'form-control-danger': bankAccountNumber_firstpartInput.invalid}" (keyup)="bankAccountNumber_firstpartChanged($event)" />
                        <div *ngIf="this.agreementForm?.dirty && bankAccountNumber_firstpartInput.invalid">
                            <div *ngIf="bankAccountNumber_firstpartInput.errors.RequiredField" class="form-control-feedback">Required Field</div>
                            <div *ngIf="bankAccountNumber_firstpartInput.errors.InvalidFormat" class="form-control-feedback">Invalid Format</div>
                        </div>
                    </div>
                    <span class="bankaccseparator">-</span>
                    <div class="form-group nopadding col-xl-2 col-lg-3" [ngClass]="{'has-danger': bankAccountNumber_secondpartInput.invalid}">
                        <label class="hidden" for="bankAccountNumber_secondpart">Bankszámla száma2</label>
                        <input #bankAccountNumberSecondpart type="text" formControlName="bankAccountNumber_secondpartControl" id="bankAccountNumber_secondpart" class="form-control" [ngClass]="{'form-control-danger': bankAccountNumber_secondpartInput.invalid}" (keyup)="bankAccountNumber_secondpartChanged($event)" />
                        <div *ngIf="this.agreementForm?.dirty && bankAccountNumber_secondpartInput.invalid">
                            <div *ngIf="bankAccountNumber_secondpartInput.errors.RequiredField" class="form-control-feedback">Required Field</div>
                            <div *ngIf="bankAccountNumber_secondpartInput.errors.InvalidFormat" class="form-control-feedback">Invalid Format</div>
                        </div>
                    </div>
                    <span class="bankaccseparator">-</span>
                    <div class="form-group nopadding col-xl-2 col-lg-3" [ngClass]="{'has-danger': bankAccountNumber_thirdpartInput.invalid}">
                        <label class="hidden" for="bankAccountNumber_thirdpart">Bankszámla száma3</label>
                        <input #bankAccountNumberThirdpart type="text" formControlName="bankAccountNumber_thirdpartControl" id="bankAccountNumber_thirdpart" class="form-control" [ngClass]="{'form-control-danger': bankAccountNumber_thirdpartInput.invalid}" (keyup)="bankAccountNumber_thirdpartChanged($event)" />
                        <div *ngIf="this.agreementForm?.dirty && bankAccountNumber_thirdpartInput.invalid">
                            <div *ngIf="bankAccountNumber_thirdpartInput.errors.RequiredField" class="form-control-feedback">Required Field</div>
                            <div *ngIf="bankAccountNumber_thirdpartInput.errors.InvalidFormat" class="form-control-feedback">Invalid Format</div>
                        </div>
                    </div>
                </div>
                
     ------------------------------------------------------------------------------------------------------------------------------
     
     .inlineControls {
    margin-left: 0px;
    margin-right: 0px;
    display: flex;
    flex-wrap: wrap;
}

    .inlineControls .form-group {
        margin-right: 15px;
        flex-grow: 1;
    }
.bankaccseparator {
    margin-top: 35px;
    font-size: 1rem;
    margin-left: -10px;
    margin-right: 5px;
}

.nopadding {
    padding: 0px;
}

.hidden {
    visibility: hidden;
}

.nowrap {
    white-space: nowrap;
}
