import { httpClient } from '@/shared/api/httpClient'
import type { AuthToken, LoginRequest, RegisterRequest } from '../types/auth'
export class AuthApi {
  login(request: LoginRequest) { return httpClient.post<AuthToken>('/api/Auth/login', request) }
  register(request: RegisterRequest) { return httpClient.post<AuthToken>('/api/Auth/register', request) }
  refresh() { return httpClient.post<AuthToken>('/api/Auth/refresh') }
  logout() { return httpClient.post<void>('/api/Auth/logout') }
}
export const authApi = new AuthApi()
