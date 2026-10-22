import { httpClient } from '@/shared/api/httpClient'
import type { AuthToken, CurrentUserProfile, LoginRequest, RegisterRequest } from '../types/auth'
export class AuthApi {
  login(request: LoginRequest) { return httpClient.post<AuthToken>('/api/Auth/login', request) }
  register(request: RegisterRequest) { return httpClient.post<AuthToken>('/api/Auth/register', request) }
  refresh() { return httpClient.post<AuthToken>('/api/Auth/refresh') }
  logout() { return httpClient.post<void>('/api/Auth/logout') }
  me() { return httpClient.get<CurrentUserProfile>('/api/Profile/me') }
}
export const authApi = new AuthApi()
