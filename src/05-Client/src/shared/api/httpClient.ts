const apiBaseUrl = (import.meta.env.VITE_API_BASE_URL ?? '').replace(/\/$/, '')
let accessToken: string | null = null

export function setAccessToken(token: string | null) { accessToken = token }

export class ApiError extends Error {
  public readonly status: number

  constructor(
    message: string,
    status: number,
  ) {
    super(message)
    this.name = 'ApiError'
    this.status = status
  }
}

function errorMessageForStatus(status: number) {
  if (status === 400) return 'اطلاعات واردشده معتبر نیست.'
  if (status === 401) return 'برای ادامه دوباره وارد حساب کاربری شوید.'
  if (status === 403) return 'اجازه انجام این عملیات را ندارید.'
  if (status === 404) return 'مورد درخواستی پیدا نشد.'
  if (status === 409) return 'این عملیات با وضعیت فعلی سازگار نیست.'
  if (status >= 500) return 'خطای غیرمنتظره‌ای رخ داد. لطفاً بعداً دوباره تلاش کنید.'
  return 'درخواست انجام نشد. لطفاً دوباره تلاش کنید.'
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const headers = new Headers(init?.headers)
  headers.set('Accept', 'application/json')
  if (accessToken) headers.set('Authorization', `Bearer ${accessToken}`)

  if (init?.body && !headers.has('Content-Type')) {
    if (!(typeof FormData !== 'undefined' && init.body instanceof FormData))
      headers.set('Content-Type', 'application/json')
  }

  let response: Response
  try {
    response = await fetch(`${apiBaseUrl}${path}`, { ...init, headers, credentials: 'include' })
  } catch {
    throw new ApiError('ارتباط با سرور برقرار نشد. اتصال اینترنت یا وضعیت سرور را بررسی کنید.', 0)
  }

  if (!response.ok) {
    const body = await response.text()
    let message = errorMessageForStatus(response.status)
    try {
      const problem = JSON.parse(body) as { detail?: string; title?: string }
      message = problem.detail ?? problem.title ?? message
    } catch { /* Retain plain-text errors. */ }
    throw new ApiError(message, response.status)
  }

  if (response.status === 204) {
    return undefined as T
  }

  return response.json() as Promise<T>
}

async function requestBlob(path: string): Promise<Blob> {
  const headers = new Headers({ Accept: 'image/*' })
  if (accessToken) headers.set('Authorization', `Bearer ${accessToken}`)

  let response: Response
  try {
    response = await fetch(`${apiBaseUrl}${path}`, { headers, credentials: 'include' })
  } catch {
    throw new ApiError('ارتباط با سرور برقرار نشد. اتصال اینترنت یا وضعیت سرور را بررسی کنید.', 0)
  }

  if (!response.ok) {
    const body = await response.text()
    let message = errorMessageForStatus(response.status)
    try {
      const problem = JSON.parse(body) as { detail?: string; title?: string }
      message = problem.detail ?? problem.title ?? message
    } catch { /* Keep the localized status message. */ }
    throw new ApiError(message, response.status)
  }

  return response.blob()
}

export const httpClient = {
  get: <T>(path: string, init?: RequestInit) => request<T>(path, init),
  post: <T>(path: string, body?: unknown, init?: RequestInit) =>
    request<T>(path, {
      ...init,
      method: 'POST',
      body: body === undefined ? undefined : typeof FormData !== 'undefined' && body instanceof FormData ? body : JSON.stringify(body),
    }),
  put: <T>(path: string, body?: unknown, init?: RequestInit) =>
    request<T>(path, {
      ...init,
      method: 'PUT',
      body: body === undefined ? undefined : JSON.stringify(body),
    }),
  patch: <T>(path: string, body?: unknown, init?: RequestInit) =>
    request<T>(path, {
      ...init,
      method: 'PATCH',
      body: body === undefined ? undefined : JSON.stringify(body),
    }),
  delete: (path: string, init?: RequestInit) =>
    request<void>(path, { ...init, method: 'DELETE' }),
  getBlob: (path: string) => requestBlob(path),
}
