import { useState } from 'react'
import type { FormEvent } from 'react'
import { authStore } from '../store/AuthStore'
import { Button } from '@/components/ui/Button'

export function RegisterPage({ onLogin }: { onLogin: () => void }) {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [error, setError] = useState('')
  const [busy, setBusy] = useState(false)

  async function submit(event: FormEvent) {
    event.preventDefault()
    setError('')
    if (password !== confirmPassword) { setError('گذرواژه و تکرار آن یکسان نیستند.'); return }
    setBusy(true)
    try { await authStore.register({ email, password }) }
    catch (e) { setError(e instanceof Error ? e.message : 'ساخت حساب انجام نشد. اطلاعات را بررسی کنید.') }
    finally { setBusy(false) }
  }

  return <main className="auth-layout" dir="rtl">
    <section className="auth-card">
      <a className="brand" href="/register"><span className="brand-mark">ت</span><span>تیکت‌پلاس</span></a>
      <p className="eyebrow">شروعی ساده برای دریافت پشتیبانی</p>
      <h1>ساخت حساب کاربری</h1><p className="muted">حساب بساز و درخواست پشتیبانی‌ات را پیگیری کن.</p>
      <form className="form-stack" onSubmit={submit}>
        <label>ایمیل<input type="email" autoComplete="email" value={email} onChange={e => setEmail(e.target.value)} required maxLength={256} placeholder="name@example.com" /></label>
        <label>گذرواژه<input type="password" autoComplete="new-password" value={password} onChange={e => setPassword(e.target.value)} required minLength={8} maxLength={100} placeholder="حداقل ۸ نویسه" /></label>
        <p className="auth-hint">گذرواژه باید شامل حروف کوچک و بزرگ انگلیسی، عدد و نماد باشد.</p>
        <label>تکرار گذرواژه<input type="password" autoComplete="new-password" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} required minLength={8} maxLength={100} placeholder="گذرواژه را دوباره وارد کنید" /></label>
        {error && <p className="alert-error" role="alert">{error}</p>}
        <Button type="submit" disabled={busy}>{busy ? 'در حال ساخت حساب…' : 'ثبت‌نام'}</Button>
      </form>
      <p className="auth-foot muted">قبلاً حساب ساخته‌ای؟ <button className="auth-link" type="button" onClick={onLogin}>وارد شو</button></p>
    </section>
    <aside className="auth-art"><div className="art-orbit">✦</div><h2>پشتیبانی،<br />همیشه در دسترس</h2><p>حسابت را بساز و گفتگو با تیم پشتیبانی را شروع کن.</p><div className="art-stat"><strong>ساده و سریع</strong><span>درخواست‌هایت را یک‌جا مدیریت کن</span></div></aside>
  </main>
}
