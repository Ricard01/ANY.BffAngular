import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {Component, OnInit} from '@angular/core';
import {BehaviorSubject, catchError, filter, Observable, tap} from 'rxjs';
import {AuthenticationService} from "../authentication.service";



@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit{

  private readonly wets = new BehaviorSubject<Weather[]>([]);
  public readonly wets$: Observable<Weather[]> = this.wets;

  private readonly users = new BehaviorSubject<IUser[]>([]);
  public readonly users$: Observable<IUser[]> = this.users;

  private readonly errors = new BehaviorSubject<string>('');
  public readonly error$: Observable<string> = this.errors;

  public isAuthenticated$: Observable<boolean> = new Observable();

  public date = "";
  public name = "";

  public constructor(
    private http: HttpClient,
    private auth: AuthenticationService) { }

  public ngOnInit(): void {
    this.auth.getIsAuthenticated()
      .pipe(
        filter(isAuthenticated => isAuthenticated),
        tap(() => {
        this.fetchWeather();
          // this.fetchTodos();
          // this.getClaims();
          // this.fetchTodos2();
        })
      ).subscribe();
  }

  private getClaims(): void {
    this.http
      .get('claims')
      .pipe(catchError(this.showError))
      .subscribe((claims) => {
        console.log(claims);
      });
  }
  public fetchWeather(): void {
    this.http
      .get<Weather[]>('weatherforecast')
      .pipe(catchError(this.showError))
      .subscribe((wet) => {
        console.log(wet)
        this.wets.next(wet);
      });
  }
  public fetchTodos(): void {
    this.http
      .get<IUserVm>('users')
      .pipe(catchError(this.showError))
      .subscribe((todos) => {
        console.log(todos)
        this.users.next(todos.users);
      });
  }

  private readonly showError = (err: HttpErrorResponse) => {
    if (err.status !== 401) {
      this.errors.next(err.message);
    }
    throw err;
  }

  private fetchTodos2(): void {
    this.http
      .get<Todo[]>('todos')
      .pipe(catchError(this.showError))
      .subscribe((todos) => {
        console.log(todos);
        // this.todos.next(todos);
      });
  }
}

export interface IUserVm {
  users: IUser[];
}

export interface IUser {
  id: string,
  userName: string,
  nombre: string
}


interface Todo {
  id: number;
  name: string;
  date: string;
  user: string;
}

interface Weather {
  date: string;
  temperatureC: string;
  temperatureF: string;
  summary: string;
}

