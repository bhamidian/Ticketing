export interface LoginRequest { emailOrUserName: string; password: string }
export interface RegisterRequest { email: string; password: string }
export interface AuthToken { accessToken: string; accessTokenExpiresAtUtc: string; refreshTokenExpiresAtUtc: string }
export interface AuthState { token: AuthToken | null; userName: string; isAgent: boolean; isRestoring: boolean }
