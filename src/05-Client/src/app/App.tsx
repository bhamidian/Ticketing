import { useEffect, useRef, useState } from 'react'
import { AppProviders } from './providers/AppProviders'
import { AppShell } from '../layouts/AppShell'
import { TicketsPage } from '../features/tickets/pages/TicketsPage'
import { LoginPage } from '../features/auth/components/LoginPage'
import { RegisterPage } from '../features/auth/components/RegisterPage'
import { useAuthStore, authStore } from '../features/auth/store/AuthStore'
import { useTicketsStore, ticketsStore } from '../features/tickets/store/TicketsStore'
import { NewTicketPage } from '../features/tickets/pages/NewTicketPage'
import { TicketDetailPage } from '../features/tickets/pages/TicketDetailPage'
import type { TicketSummary } from '../features/tickets/types/ticket'
import { TicketPriority } from '../features/tickets/types/ticket'

type AppRoute =
  | { page: 'login' }
  | { page: 'register' }
  | { page: 'notFound' }
  | { page: 'list' }
  | { page: 'new' }
  | { page: 'detail'; ticketId: string }

function readRoute(): AppRoute {
  const segments = window.location.pathname.split('/').filter(Boolean)
  if (segments.length === 0) return { page: 'list' }
  if (segments.length === 1 && segments[0] === 'register') return { page: 'register' }
  if (segments.length === 1 && segments[0] === 'login') return { page: 'login' }
  if (segments[0] !== 'tickets') return { page: 'notFound' }
  if (segments.length === 1) return { page: 'list' }
  if (segments.length === 2 && segments[1] === 'new') return { page: 'new' }
  if (segments.length === 2) return { page: 'detail', ticketId: segments[1] }
  return { page: 'notFound' }
}

export function App() {
  const auth = useAuthStore()
  const { selected, loading, error } = useTicketsStore()
  const [route, setRoute] = useState<AppRoute>(readRoute)
  const requestedTicketId = useRef<string | null>(null)

  useEffect(() => { void authStore.restore() }, [])

  useEffect(() => {
    const onPopState = () => setRoute(readRoute())
    window.addEventListener('popstate', onPopState)
    return () => window.removeEventListener('popstate', onPopState)
  }, [])

  useEffect(() => {
    if (auth.token && ['/', '/login', '/register'].includes(window.location.pathname)) {
      window.history.replaceState(null, '', '/tickets')
      setRoute({ page: 'list' })
    }
  }, [auth.token])

  let userId = ''
  try {
    const token = auth.token?.accessToken
    if (token) {
      const data = JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))) as Record<string, string>
      userId = data.sub ?? data['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ?? ''
    }
  } catch {
    userId = ''
  }

  useEffect(() => {
    if (!auth.token || route.page !== 'detail' || selected?.id === route.ticketId) return
    if (requestedTicketId.current === route.ticketId) return

    requestedTicketId.current = route.ticketId
    void ticketsStore.open(route.ticketId)
  }, [auth.token, auth.isAgent, route, selected?.id])

  function navigate(path: string) {
    if (window.location.pathname !== path) window.history.pushState(null, '', path)
    setRoute(readRoute())
  }

  if (auth.isRestoring) return <div className="loading-state" dir="rtl">در حال بررسی نشست…</div>
  if (route.page === 'notFound') return (
    <main className="route-not-found" dir="rtl">
      <span className="route-not-found-code">۴۰۴</span>
      <h1>صفحه پیدا نشد</h1>
      <p>آدرسی که وارد کرده‌اید وجود ندارد یا ممکن است جابه‌جا شده باشد.</p>
      <button className="button button-primary" onClick={() => navigate(auth.token ? '/tickets' : '/login')}>
        {auth.token ? 'بازگشت به درخواست‌ها' : 'رفتن به صفحه ورود'}
      </button>
    </main>
  )
  if (!auth.token) return route.page === 'register'
    ? <RegisterPage onLogin={() => navigate('/login')} />
    : <LoginPage onRegister={() => navigate('/register')} />

  async function create(subject: string, message: string, priority: TicketPriority, image: File | null) {
    const ticket = await ticketsStore.create({ subject, message, priority }, image)
    requestedTicketId.current = ticket.id
    navigate(`/tickets/${ticket.id}`)
  }

  function open(ticket: TicketSummary) {
    requestedTicketId.current = ticket.id
    navigate(`/tickets/${ticket.id}`)
    void ticketsStore.open(ticket.id)
  }

  return (
    <AppProviders>
      <AppShell
        userName={auth.userName}
        isAgent={auth.isAgent}
        onLogout={() => void authStore.logout()}
        onTickets={() => navigate('/tickets')}
        onNew={() => navigate('/tickets/new')}
      >
        {route.page === 'list' && (
          <TicketsPage
            isAgent={auth.isAgent}
            onNew={() => navigate('/tickets/new')}
            onOpen={open}
          />
        )}
        {route.page === 'new' && (
          <NewTicketPage onCancel={() => navigate('/tickets')} onCreate={create} />
        )}
        {route.page === 'detail' && (selected?.id === route.ticketId ? (
          <TicketDetailPage
            ticket={selected}
            userId={userId}
            isAgent={auth.isAgent}
            onBack={() => navigate('/tickets')}
            onReply={async (text) => { await ticketsStore.reply(selected.id, text) }}
            onClose={async () => { await ticketsStore.close(selected.id) }}
            onClaim={async () => { await ticketsStore.claim(selected.id) }}
            onChangeStatus={async (status) => { await ticketsStore.changeStatus(selected.id, status) }}
          />
        ) : (
          <div className="loading-state">
            {loading ? 'در حال بارگذاری گفتگو…' : error ?? 'تیکت پیدا نشد.'}
            <button className="back-link" onClick={() => navigate('/tickets')}>بازگشت</button>
          </div>
        ))}
      </AppShell>
    </AppProviders>
  )
}
