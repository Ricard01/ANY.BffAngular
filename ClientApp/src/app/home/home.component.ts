import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {Component, OnInit} from '@angular/core';
import {BehaviorSubject, catchError, Observable} from 'rxjs';
import {AuthenticationService} from "../authentication.service";
import {UserService} from "../users/services/user.service";
import {FormBuilder} from "@angular/forms";


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  // @ts-ignore
  userForm;
  id = "";
  name = "";

  // private readonly users = new BehaviorSubject<IUser[]>([]);
  // public readonly users$: Observable<IUser[]> = this.users;

  private readonly errors = new BehaviorSubject<string>('');
  public readonly error$: Observable<string> = this.errors;

  private readonly response = new BehaviorSubject<any>(undefined);
  public readonly response$: Observable<any> = this.response;

  private readonly title = new BehaviorSubject<string>('');
  public readonly titles$: Observable<string> = this.title;

  public isAuthenticated$: Observable<boolean> = new Observable();

  public constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private usersService: UserService,
    private auth: AuthenticationService) {
  }

  public ngOnInit(): void {

    this.initForm();

    // this.auth.getIsAuthenticated()
    //   .pipe(
    //     filter(isAuthenticated => isAuthenticated),
    //     tap(() => {
    //       this.getClaims()
    //     })
    //   ).subscribe();
  }

  private initForm() {
    this.userForm = this.formBuilder.group({
      userId: [''],
      name: ['John Doe']
    });

  }

  get form() {
    return this.userForm.controls
  }

  clearObservables() {
    this.title.next('');
    this.response.next(undefined);
    this.errors.next('');
  }

  getClaims() {
    this.usersService.getClaims()
      .pipe(catchError(this.showError))
      .subscribe(resp => {
        this.title.next("getClaims")
        this.response.next(resp);
      });
  }

  getClaimsApi2() {

    this.clearObservables();

    this.usersService.getClaimsApi2()
      .pipe(catchError(this.showError))
      .subscribe(resp => {
        this.title.next("getClaimsApi2")
        this.response.next(resp);
      });

  }

  getClaimsApi3() {
    this.clearObservables();

    this.usersService.getClaimsApi3()
      .pipe(catchError(this.showError))
      .subscribe(resp => {
        this.title.next("getClaimsApi3")
        this.response.next(resp);
      });
  }

  getOrders() {
    this.clearObservables();

    this.usersService.getOrders()
      .pipe(catchError(this.showError))
      .subscribe(resp => {
        this.title.next("getOrders")
        this.response.next(resp);
      });


  }

  getUsers() {

    this.usersService.get()
      .pipe(catchError(this.showError))
      .subscribe(resp => {
        this.response.next(resp);
      });

  }

  getUserById() {

    if (this.userForm.valid) {

      this.usersService.getById(this.form.userId.value)
        .pipe(catchError(this.showError))
        .subscribe(resp => {
          this.response.next(resp);
        });

    }


  }

  getApiTest2() {

    this.http
      .get('api2/roles')
      .pipe(catchError(this.showError))
      .subscribe((claims) => {

        this.response.next(claims);
        console.log(claims);
      });
  }


  showError = (err: HttpErrorResponse) => {
    // if (err.status !== 401) {

    this.errors.next(err.message);
    // }
    throw err;
  }

}


