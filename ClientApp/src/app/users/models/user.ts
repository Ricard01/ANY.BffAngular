import { IRole } from 'src/app/roles/models';

export interface IUserVm {
    users?: IUser[] | undefined;
}

export interface IUser {
    id: string;
    userName: string;
    password: string;
    name: string;
    email: string;
    profilePicture: string;
    role: IRole;
}

export interface Result {
    succeeded: boolean;
    errors: [];
}