import { useSyncExternalStore } from 'react'
import { setAccessToken } from '@/shared/api/httpClient'
import { authApi } from '../api/AuthApi'
import type { AuthState, LoginRequest, RegisterRequest } from '../types/auth'

function claims(token: string) {
  try { return JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))) as Record<string, unknown> }
  catch { return {} }
}
class AuthStoreClass {
  private state: AuthState = { token: null, userName: '', departmentName: null, isAgent: false, isAdmin: false, isRestoring: true }
  private listeners = new Set<() => void>()
  private restorePromise: Promise<void> | null = null
  getSnapshot = () => this.state
  subscribe = (listener: () => void) => { this.listeners.add(listener); return () => this.listeners.delete(listener) }
  private publish(state: AuthState) { this.state = state; this.listeners.forEach(listener => listener()) }

  private applyToken(token: AuthState['token'], fallbackName = '') {
    if (!token) {
      setAccessToken(null)
      this.publish({ token: null, userName: '', departmentName: null, isAgent: false, isAdmin: false, isRestoring: false })
      return
    }

    setAccessToken(token.accessToken)
    const payload = claims(token.accessToken)
    const role = payload.role ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    const roles = [role].flat().map(String)
    this.publish({ token, userName: String(payload.unique_name ?? payload.name ?? fallbackName), departmentName: null, isAgent: roles.includes('Agent'), isAdmin: roles.includes('Admin'), isRestoring: false })
  }

  async loadProfile() {
    const accessToken = this.state.token?.accessToken
    if (!accessToken) return
    try {
      const profile = await authApi.me()
      if (this.state.token?.accessToken === accessToken)
        this.publish({ ...this.state, departmentName: profile.departmentName })
    } catch {
      if (this.state.token?.accessToken === accessToken)
        this.publish({ ...this.state, departmentName: null })
    }
  }

  restore() {
    if (!this.restorePromise) {
      this.restorePromise = authApi.refresh()
        .then(token => this.applyToken(token))
        .catch(() => this.applyToken(null))
        .finally(() => { this.restorePromise = null })
    }
    return this.restorePromise
  }

  async login(request: LoginRequest) {
    const token = await authApi.login(request)
    this.applyToken(token, request.emailOrUserName)
  }
  async register(request: RegisterRequest) {
    const token = await authApi.register(request)
    this.applyToken(token, request.email)
  }
  async logout() {
    try { await authApi.logout() }
    finally { this.applyToken(null) }
  }
}
export const authStore = new AuthStoreClass()
export function useAuthStore() { return useSyncExternalStore(authStore.subscribe, authStore.getSnapshot, authStore.getSnapshot) }
