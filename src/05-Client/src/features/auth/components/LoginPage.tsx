import { useState } from 'react'
import type { FormEvent } from 'react'
import { authStore } from '../store/AuthStore'
import { Button } from '@/components/ui/Button'
import { ApiError } from '@/shared/api/httpClient'

export function LoginPage({ onRegister }: { onRegister: () => void }) {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [busy, setBusy] = useState(false)
  async function submit(event: FormEvent) {
    event.preventDefault(); setError(''); setBusy(true)
    try { await authStore.login({ emailOrUserName: email, password }) }
    catch (e) {
      setError(e instanceof ApiError && e.status === 401
        ? 'ایمیل یا گذرواژه اشتباه است.'
        : e instanceof Error ? e.message : 'ورود انجام نشد. دوباره تلاش کنید.')
    }
    finally { setBusy(false) }
  }
  return <main className="auth-layout" dir="rtl">
    <section className="auth-card">
      <a className="brand" href="/login"><span className="brand-mark">ت</span><span>تیکت‌پلاس</span></a>
      <p className="eyebrow">پشتیبانی، ساده و سریع</p>
      <h1>خوش برگشتی!</h1><p className="muted">برای ادامه وارد حساب کاربری‌ات شو.</p>
      <form className="form-stack" onSubmit={submit}>
        <label>ایمیل یا نام کاربری<input autoComplete="username" value={email} onChange={e => setEmail(e.target.value)} required placeholder="name@example.com" /></label>
        <label>گذرواژه<input type="password" autoComplete="current-password" value={password} onChange={e => setPassword(e.target.value)} required placeholder="گذرواژه را وارد کنید" /></label>
        {error && <p className="alert-error" role="alert">{error}</p>}
        <Button type="submit" disabled={busy}>{busy ? 'در حال ورود…' : 'ورود به حساب'}</Button>
      </form>
      <p className="auth-foot muted">حساب کاربری نداری؟ <button className="auth-link" type="button" onClick={onRegister}>ثبت‌نام کن</button></p>
    </section>
    <aside className="auth-art"><div className="art-orbit">✦</div><h2>همیشه یک قدم<br />به پاسخ نزدیک‌تر</h2><p>گفتگوها، درخواست‌ها و پاسخ‌های پشتیبانی را یک‌جا دنبال کن.</p><div className="art-stat"><strong>پشتیبانی همراه تو</strong><span>از ثبت درخواست تا حل کامل مسئله</span></div></aside>
  </main>
}
