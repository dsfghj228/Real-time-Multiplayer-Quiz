export interface loginRequest {
  username: string;
  password: string;
}

export interface loginResponse {
  userName: string;
  email: string;
  token: string;
}

export interface registerRequest {
  username: string;
  email: string;
  password: string;
}

export interface refreshResponse {
  accessToken: string;
}

export type validationLoginErrors = {
  username: string;
  password: string;
};

export type validationRegisterErrors = validationLoginErrors & {
  email: string;
};
