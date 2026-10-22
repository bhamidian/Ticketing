import { useEffect, useState } from 'react'
import { ticketsApi } from '../api/TicketsApi'
import { ticketsStore, useTicketsStore } from '../store/TicketsStore'
import { TicketStatus } from '../types/ticket'
import type { AgentOption, TicketPriority, TicketSummary } from '../types/ticket'
import { TicketCard } from '../components/TicketCard'
import { TicketsToolbar } from '../components/TicketsToolbar'

export function AdminTicketsPage({ onOpen }: { onOpen: (ticket: TicketSummary) => void }) {
  const { items, totalPages, totalCount, loading, error } = useTicketsStore()
  const [agents, setAgents] = useState<AgentOption[]>([])
  const [agentsError, setAgentsError] = useState('')
  const [selectedAgents, setSelectedAgents] = useState<Record<string, string>>({})
  const [assigning, setAssigning] = useState<string | null>(null)
  const [status, setStatus] = useState<TicketStatus | ''>('')
  const [priority, setPriority] = useState<TicketPriority | ''>('')
  const [sortOrder, setSortOrder] = useState<'newest' | 'oldest'>('newest')
  const [page, setPage] = useState(1)
  const [feedback, setFeedback] = useState('')
  const pageSize = 12

  useEffect(() => {
    void ticketsApi.agents().then(setAgents).catch(e => setAgentsError(e instanceof Error ? e.message : 'فهرست ایجنت‌ها دریافت نشد.'))
  }, [])
  useEffect(() => {
    void ticketsStore.load({ status: status === '' ? undefined : status, priority: priority === '' ? undefined : priority, page, pageSize, sortOrder })
  }, [status, priority, page, sortOrder])

  async function assign(ticket: TicketSummary) {
    const agentUserId = selectedAgents[ticket.id] ?? ticket.assignedAgentId ?? ''
    if (!agentUserId) return
    setAssigning(ticket.id)
    setFeedback('')
    try {
      await ticketsApi.assign(ticket.id, agentUserId)
      setFeedback('تیکت به ایجنت واگذار شد.')
      await ticketsStore.load({ status: status === '' ? undefined : status, priority: priority === '' ? undefined : priority, page, pageSize, sortOrder })
    } catch (e) {
      setFeedback(e instanceof Error ? e.message : 'واگذاری تیکت انجام نشد.')
    } finally {
      setAssigning(null)
    }
  }

  return (
    <section className="page-wrap">
      <header className="page-title">
        <div>
          <p className="eyebrow">میز مدیریت</p>
          <h1>درخواست‌های پشتیبانی</h1>
          <p className="muted">تیکت‌ها را بررسی کن، به ایجنت‌ها واگذار کن و پیام‌های در انتظار تأیید را ببین.</p>
        </div>
      </header>
      <TicketsToolbar status={status} priority={priority} sortOrder={sortOrder}
        onStatus={value => { setStatus(value); setPage(1) }}
        onPriority={value => { setPriority(value); setPage(1) }}
        onSortOrder={value => { setSortOrder(value); setPage(1) }} />
      <section className="agent-report panel" aria-labelledby="agent-report-title">
        <div className="agent-report-heading">
          <div>
            <p className="eyebrow">گزارش عملکرد</p>
            <h2 id="agent-report-title">پاسخ‌گویی ایجنت‌ها</h2>
          </div>
          <span>{agents.length.toLocaleString('fa-IR')} ایجنت</span>
        </div>
        {agents.length > 0 ? <ul className="agent-report-list">
          {agents.map(agent => <li key={agent.id}>
            <span className="agent-report-email" dir="ltr">{agent.email}</span>
            <strong>{agent.respondedTicketsCount.toLocaleString('fa-IR')} تیکت پاسخ‌داده‌شده</strong>
          </li>)}
        </ul> : !agentsError && <p className="muted">ایجنتی برای نمایش وجود ندارد.</p>}
      </section>
      {agentsError && <p className="alert-error" role="alert">{agentsError}</p>}
      {feedback && <p className={feedback.includes('انجام نشد') || feedback.includes('اجازه') ? 'alert-error' : 'admin-feedback'} role="status">{feedback}</p>}
      {error && <div className="alert-error" role="alert">{error}</div>}
      {loading ? <div className="loading-state">در حال دریافت درخواست‌ها…</div> : items.length === 0 ? (
        <div className="empty-state"><span>◇</span><h3>درخواستی پیدا نشد</h3><p>با فیلترهای دیگری جست‌وجو کن.</p></div>
      ) : (
        <div className="admin-ticket-list">
          {items.map(ticket => {
            const selected = selectedAgents[ticket.id] ?? ticket.assignedAgentId ?? ''
            return <article className="admin-ticket-row" key={ticket.id}>
              <TicketCard ticket={ticket} showRequesterEmail onOpen={() => onOpen(ticket)} />
              <div className="admin-assignment">
                <label htmlFor={`agent-${ticket.id}`}>ایجنت مسئول</label>
                <select id={`agent-${ticket.id}`} value={selected} onChange={event => setSelectedAgents(current => ({ ...current, [ticket.id]: event.target.value }))}>
                  <option value="">انتخاب ایجنت</option>
                  {agents.map(agent => <option key={agent.id} value={agent.id}>{agent.email}</option>)}
                </select>
                <button className="button button-primary" type="button" disabled={!selected || assigning === ticket.id || ticket.status === TicketStatus.Closed} onClick={() => void assign(ticket)}>
                  {assigning === ticket.id ? 'در حال واگذاری…' : ticket.assignedAgentId ? 'تغییر ایجنت' : 'واگذاری'}
                </button>
              </div>
            </article>
          })}
        </div>
      )}
      {totalCount > 0 && <div className="ticket-pagination" dir="rtl">
        <span>نمایش {Math.min((page - 1) * pageSize + 1, totalCount)} تا {Math.min(page * pageSize, totalCount)} از {totalCount} درخواست</span>
        <div className="ticket-pagination-controls">
          <button className="button button-secondary" disabled={loading || page <= 1} onClick={() => setPage(value => Math.max(1, value - 1))}>قبلی</button>
          <span>صفحه {page} از {Math.max(totalPages, 1)}</span>
          <button className="button button-secondary" disabled={loading || page >= totalPages} onClick={() => setPage(value => value + 1)}>بعدی</button>
        </div>
      </div>}
    </section>
  )
}
