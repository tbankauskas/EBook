import { Injectable, OnDestroy } from '@angular/core';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';


export interface ISpinnerState {
    hide: boolean;
}

@Injectable({ providedIn: 'root' })
export class SpinnerService implements OnDestroy {
    private currentTimeout: number;

    delay = 0;

    private _spinnerSubject = new Subject<ISpinnerState>();
    spinnerState: Observable<ISpinnerState> = this._spinnerSubject;

    show(): void {
        this.currentTimeout = window.setTimeout(() => {
            this._spinnerSubject.next({ hide: false });
            this.cancelTimeout();
        }, this.delay);
    }

    hide(): void {
        this._spinnerSubject.next({ hide: true });
        this.cancelTimeout();
    }

    private cancelTimeout(): void {
        clearTimeout(this.currentTimeout);
        this.currentTimeout = undefined;
    }

    ngOnDestroy(): void {
        this.cancelTimeout();
    }
}
