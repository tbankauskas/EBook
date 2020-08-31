import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';

@Component({
    selector: 'register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss']
})

export class RegisterComponent implements OnInit {

    error: string;
    registerForm: FormGroup;
    get formProperties(): any { return this.registerForm.controls; }

    constructor(private formBuilder: FormBuilder,
        private authenticationService: AuthenticationService,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.registerForm = this.formBuilder.group(
            {
                login: ['', Validators.required],
                email: ['', Validators.email],
                password: ['', [Validators.required, Validators.minLength(8),
                Validators.maxLength(30), Validators.pattern('((?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,30})')]
                ],
                confirmPassword: ['', [Validators.required]],
                firstName: [''],
                lastName: ['']
            },
            {
                validator: [this.mustMatch()]
            });

    }

    mustMatch(): any {
        return (group: FormGroup) => {
            const control = group.controls.password;
            const matchingControl = group.controls.confirmPassword;
            return control.value !== matchingControl.value ? { mustMatch: true } : null;
        };
    }

    register(): void {
        if (this.registerForm.invalid) {
            return;
        }
        this.authenticationService.register(this.registerForm.value).subscribe(() => {
            this.router.navigateByUrl('/login', { state: { isRegisterSuccess: true } });
        }, err => {
            this.error = err.error.message;
        });
    }

}