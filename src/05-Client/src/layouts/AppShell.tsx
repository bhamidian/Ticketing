import type { PropsWithChildren } from 'react'
import { useEffect, useRef, useState } from 'react'

interface Props extends PropsWithChildren { userName: string; isAgent: boolean; onLogout: () => void; onTickets: () => void; onNew: () => void }
export function AppShell({ children, userName, isAgent, onLogout, onTickets, onNew }: Props) {
  const [userMenuOpen, setUserMenuOpen] = useState(false)
  const userMenuRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    if (!userMenuOpen) return

    const closeOnOutsideClick = (event: MouseEvent) => {
      if (!userMenuRef.current?.contains(event.target as Node)) setUserMenuOpen(false)
    }
    const closeOnEscape = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setUserMenuOpen(false)
    }

    document.addEventListener('mousedown', closeOnOutsideClick)
    document.addEventListener('keydown', closeOnEscape)
    return () => {
      document.removeEventListener('mousedown', closeOnOutsideClick)
      document.removeEventListener('keydown', closeOnEscape)
    }
  }, [userMenuOpen])

  return (
    <div className="app-shell" dir="rtl">
      <header className="app-header">
        <a className="brand" href="/tickets" aria-label="خانه" onClick={(event) => { event.preventDefault(); onTickets() }}><span className="brand-mark">ت</span><span>تیکت‌پلاس</span></a>
        <nav className="main-nav"><button className="nav-link active" onClick={onTickets}>{isAgent ? 'میز پشتیبانی' : 'درخواست‌های من'}</button>{!isAgent && <button className="nav-link" onClick={onNew}>ثبت تیکت</button>}</nav>
        <div className="header-user" ref={userMenuRef}>
          <button
            className="user-menu-trigger"
            aria-expanded={userMenuOpen}
            aria-haspopup="menu"
            onClick={() => setUserMenuOpen((open) => !open)}
          >
            <span className="user-avatar">{userName.slice(0, 1)}</span>
            <span className="user-name">{userName}</span>
            <span className="user-menu-chevron" aria-hidden="true">⌄</span>
          </button>
          {userMenuOpen && (
            <div className="user-menu" role="menu">
              <button
                className="logout-button"
                role="menuitem"
                onClick={() => {
                  setUserMenuOpen(false)
                  onLogout()
                }}
              >
                خروج
              </button>
            </div>
          )}
        </div>
      </header>
      <main className="app-content">{children}</main>
      <footer className="app-footer">تیکت‌پلاس <span>·</span> سامانهٔ پشتیبانی</footer>
    </div>
  )
}
