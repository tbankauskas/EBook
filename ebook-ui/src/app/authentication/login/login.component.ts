import { SpinnerService } from '../../shared-components-module/spinner/spinner.service';
import { Component, EventEmitter, Output, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit, OnDestroy {

    @Output() changed = new EventEmitter<string>();

    authSubscription: Subscription;
    error = '';
    message = '';
    ref = '';
    loginForm: FormGroup;

    public hasError = (controlName: string, errorName: string) => {
        return this.loginForm.controls[controlName].hasError(errorName);
    }

    handleEnterKeyboardEvent(event: KeyboardEvent): void {
        if (event.key === 'Enter') {
            this.login(this.loginForm.get('username').value, this.loginForm.get('password').value);
        }
    }

    constructor(private authenticationService: AuthenticationService,
        private router: Router,
        private spinner: SpinnerService) {

        if (this.router.getCurrentNavigation().extras.state?.isRegisterSuccess) {
            this.message = 'Registration was successfully. Please Login!';
        }
        if (this.router.getCurrentNavigation().extras.state?.expired) {
            this.message = this.router.getCurrentNavigation().extras.state?.expired;
        }
    }

    ngOnInit(): void {

        this.loginForm = new FormGroup({
            username: new FormControl('', [Validators.required]),
            password: new FormControl('', [Validators.required])
        });

        this.authSubscription = this.authenticationService.authChanged
            .pipe(filter(x => x.event === 'login')).subscribe(() => {
                this.navigateFurther();
            });
    }

    private navigateFurther(): void {
        let { ref } = this;
        if (this.router.url.includes(ref)) {
            ref = '';
        }
        this.router.navigate(ref === '' ? ['/'] : [ref]);
    }

    ngOnDestroy(): void {
        if (this.authSubscription) {
            this.authSubscription.unsubscribe();
        }
    }

    login(username: string, password: string): void {

        if (this.loginForm.invalid) {
            this.loginForm.get('username').markAsTouched();
            this.loginForm.get('password').markAsTouched();
            return;
        }

        this.authenticationService
            .login(username, password)
            .subscribe(
                () => { },
                (error) => {
                    this.error = error || `username and/or password could not be verified.`;
                }
            );
    }

    clearMessages(): void {
        this.error = '';
        this.message = '';
    }
}
