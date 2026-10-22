export interface LoginRequest { emailOrUserName: string; password: string }
export interface RegisterRequest { email: string; password: string }
export interface AuthToken { accessToken: string; accessTokenExpiresAtUtc: string; refreshTokenExpiresAtUtc: string }
export interface CurrentUserProfile { userId: string; email: string | null; departmentName: string | null }
export interface AuthState { token: AuthToken | null; userName: string; departmentName: string | null; isAgent: boolean; isAdmin: boolean; isRestoring: boolean }
