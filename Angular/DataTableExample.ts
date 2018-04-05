//Further info: https://github.com/afermon/angular-4-data-table-bootstrap-4-demo/

import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { Location } from "@angular/common";
import { DataTableResource, DataTableParams, DataTable, DataTableRow } from "angular-4-data-table-bootstrap-4";
import { IFoo } from "../../Models/IFoo";
import { FooService } from "../../Services/FooService";
import { OriginationEnum } from "../../Enums/OriginationEnum";
import { ConfigService } from "../../Services/ConfigService";
import { IUser } from "../../Models/IUser";
import { Router, ActivatedRoute } from "@angular/router";
import { IStatus } from "../../Models/IStatus";

@Component({
    selector: "newFoo-table",
    templateUrl: "app/Components/newFoo/newFoo.component.html",
    styleUrls: [
        "app/Components/newFoo/newFoo.component.css"
    ],
})

export class NewFooComponent implements OnInit {
    items: IFoo[];
    originalItems: IFoo[];
    itemCount = 0;
    newFoosResource: DataTableResource<IFoo>;
    errorMsg: string;
    selectedUser: IUser;
    possibleSPECs: IUser[];
    assignComment: string;
    states: IStatus[];

    @ViewChild(DataTable) newFooTable: DataTable;
    @ViewChild("clientNameFilter") clientNameFilter: ElementRef;
    @ViewChild("currentStatusFilter") currentStatusFilter: ElementRef;
    @ViewChild("SPECNameFilter") SPECNameFilter: ElementRef;

    constructor(private _fooService: FooService, private _configService: ConfigService, private _router: Router, private _route: ActivatedRoute, private _location: Location) {
        this.rowColors = this.rowColors.bind(this);
        this.rowTooltip = this.rowTooltip.bind(this);
    }

    ngOnInit(): void {
        this._fooService.getStates().subscribe(states => {
            this.states = states;
            this.states.unshift({ "Id": null, "Name": "Állapot nélküli", "Code": "null" });
            this.states.unshift({ "Id": 0, "Name": "Bármilyen állapotban", "Code": "All" });
        }, error => this.errorMsg = <any>error);

        this._fooService.getNewFoos().subscribe(foos => {
            let queryParam: DataTableParams = { "offset": 0, "limit": 5, "sortBy": "", "sortAsc": true };

            this.originalItems = foos;
            this.items = this.originalItems;
            this.newFoosResource = new DataTableResource(this.items);
            this.newFoosResource.count().then(count => this.itemCount = count);
            this.newFoosResource.query(queryParam).then(items => this.items = items);
            this.selectedUser = null;
        }, error => this.errorMsg = <any>error);

        this._fooService.getPossibleSPECsByUserId().subscribe(users => {
            this.possibleSPECs = users;
        }, error => this.errorMsg = <any>error);
    }

    reloadNewFoos(params: DataTableParams) {
        if (this.newFoosResource) {
            this.newFoosResource.query(params).then(items => this.items = items);
        }
    }

    refreshTable() {
        //mielőtt ezt meghívjuk a this.items -nek tartalmaznia kell az összes megjeleníteni kívánt elemet.
        if (this.newFoosResource) {
            let queryParams = this.newFooTable.displayParams;
            queryParams.offset = 0;
            this.newFoosResource = new DataTableResource(this.items);
            this.newFoosResource.count().then(count => this.itemCount = count);
            this.newFoosResource.query(queryParams).then(items => this.items = items);
        }
    }

    clearFilters() {
        this.clientNameFilter.nativeElement.disabled = false;
        this.SPECNameFilter.nativeElement.disabled = false;
        this.currentStatusFilter.nativeElement.disabled = false;

        this.clientNameFilter.nativeElement.value = "";
        this.currentStatusFilter.nativeElement.value = 0;
        this.SPECNameFilter.nativeElement.value = null;

        this.items = this.originalItems;
        this.refreshTable();
    }

    rowTooltip(item: IFoo) {
        if (this.states) {
            let currentStatus = this.states.filter(s => s.Id === item.CurrentStatusId)[0];
            if (currentStatus) {
                return item.Id + " - " + item.ClientName + " - " + currentStatus.Name;
            }
        }
        return item.Id + " - " + item.ClientName;
    }

    rowColors(foo: IFoo) {
        let now = new Date(),
            now2 = new Date(),
            yesterday = new Date(now.setDate(now.getDate() - 1)),
            dayBeforeYesterday = new Date(now2.setDate(now2.getDate() - 2));
        if (new Date(foo.FooReceived) < dayBeforeYesterday) {
            return this._configService.getConfigValue("redcolor");
        }
        else if (new Date(foo.FooReceived) < yesterday) {
            return this._configService.getConfigValue("yellowcolor");
        }
        else {
            return this._configService.getConfigValue("greencolor");
        }
    }

    isCscGenerated(item: IFoo) {
        return item.Origination === OriginationEnum.CSC;
    }

    isFieldGenerated(item: IFoo) {
        return item.Origination === OriginationEnum.Field;
    }

    isSalespointGenerated(item: IFoo) {
        return item.Origination === OriginationEnum.Salespoint;
    }

    narrowByClientName(inpuText: string) {
        if (inpuText === "") {
            this.items = this.originalItems;
            this.refreshTable();
            this.SPECNameFilter.nativeElement.disabled = false;
            this.currentStatusFilter.nativeElement.disabled = false;
            return;
        }
        this.currentStatusFilter.nativeElement.disabled = true;
        this.SPECNameFilter.nativeElement.disabled = true;
        this.items = this.originalItems.filter(foo => {
            if (foo.ClientName !== null && foo.ClientName !== "") {
                return foo.ClientName.indexOf(inpuText) !== -1
            }
            return false;
        });
        this.refreshTable();
    }

    narrowByCurrentState(statusId: string | null) {
        let parsedStatusId = statusId !== "null" ? parseInt(statusId) : null;
        if (parsedStatusId === 0) {
            this.clientNameFilter.nativeElement.disabled = false;
            this.SPECNameFilter.nativeElement.disabled = false;
            this.items = this.originalItems;
            this.refreshTable();
        }
        else {
            this.clientNameFilter.nativeElement.disabled = true;
            this.SPECNameFilter.nativeElement.disabled = true;

            this.items = this.originalItems.filter(foo => foo.CurrentStatusId === parsedStatusId);
            this.refreshTable();
        }
    }

    narrowBySPEC(inpuText: string) {
        if (inpuText === "") {
            this.items = this.originalItems;
            this.refreshTable();
            this.clientNameFilter.nativeElement.disabled = false;
            this.currentStatusFilter.nativeElement.disabled = false;
            return;
        }
        this.clientNameFilter.nativeElement.disabled = true;
        this.currentStatusFilter.nativeElement.disabled = true;
        this.items = this.originalItems.filter(foo => {
            if (foo.SPEC !== null && foo.SPEC !== "") {
                return foo.SPEC.indexOf(inpuText) !== -1
            }
            return false;
        });
        this.refreshTable();
    }

    rowDetails(foo: IFoo) {
        if (this._router.routerState.snapshot.url.toString().indexOf("details") !== -1) {
            this._location.back();
            setTimeout(() => {
                this._router.navigate(["./details/" + foo.Id + "/" + foo.DM_Code], { relativeTo: this._route });
            }, 200)
        }
        else {
            this._router.navigate(["./details/" + foo.Id + "/" + foo.DM_Code], { relativeTo: this._route });
        }
    }

    assign(selectedrows: DataTableRow[]) {
        for (let row of selectedrows) {
            let foo = <IFoo>row.item;
            let comment: string;
            if (this.assignComment !== null) {
                comment = this.assignComment;
            }

            alert(foo.Id + " assigned to: " + this.selectedUser.Name + " Megjegyzés: " + comment);
        }
        this.selectedUser = null;
        this.assignComment = null;
    }
}
