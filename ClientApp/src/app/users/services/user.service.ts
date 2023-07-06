import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {IUser, IUserVm, Result} from '../models';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private usersUrl = 'api/users/';

  constructor(private http: HttpClient) {
  }

  getClaims() {
    return this.http.get(this.usersUrl + "claims")
  }

  getClaimsApi2() {
    return this.http.get("api2/claims")
  }

  getClaimsApi3() {
    return this.http.get("api3/claims")
  }


  getOrders() {
    return this.http.get("api3/claims/orders")
  }
  get() {
    return this.http.get<IUserVm>(this.usersUrl);
  }

  getById(id: string) {
    return this.http.get<IUser>(`${this.usersUrl}/${id}`)
  }

  create(user: IUser) {
    return this.http.post<Result>(this.usersUrl, user);
  }

  update(id: string, user: IUser) {
    return this.http.patch<Result>(`${this.usersUrl}/${id}`, user);
  }

  delete(id: string) {
    return this.http.delete(`${this.usersUrl}/${id}`);
  }

}
