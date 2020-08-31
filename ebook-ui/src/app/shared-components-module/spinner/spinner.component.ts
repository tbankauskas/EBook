import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SpinnerService, ISpinnerState } from './spinner.service';

@Component({
    selector: 'spinner',
    templateUrl: './spinner.component.html',
    styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent implements OnDestroy, OnInit {
    hide = true;
    private _spinnerStateChanged: Subscription;
    constructor(private spinnerService: SpinnerService) {
    }

    ngOnInit(): void {
        this._spinnerStateChanged = this.spinnerService.spinnerState
            .subscribe((state: ISpinnerState) => {
                this.hide = state.hide;
            });
    }

    ngOnDestroy(): void {
        this._spinnerStateChanged.unsubscribe();
    }
}
