import type { TicketSummary } from '../types/ticket'
import { TicketPriority } from '../types/ticket'
import { TicketStatusBadge } from './TicketStatusBadge'
const priorityLabel = { [TicketPriority.High]: 'زیاد', [TicketPriority.Moderate]: 'معمولی', [TicketPriority.Low]: 'کم' }
const date = (value: string) => new Intl.DateTimeFormat('fa-IR', { dateStyle: 'medium' }).format(new Date(value))
export function TicketCard({ ticket, showRequesterEmail, onOpen }: { ticket: TicketSummary; showRequesterEmail: boolean; onOpen: () => void }) {
  return <button className="ticket-card" onClick={onOpen}>
    <div className="ticket-card-head"><span className="ticket-id">#{ticket.id.slice(0, 8)}</span><TicketStatusBadge status={ticket.status} /></div>
    <h3>{ticket.subject}</h3>
    {showRequesterEmail && ticket.normalUserEmail && <span className="ticket-requester-email" dir="ltr">{ticket.normalUserEmail}</span>}
    <div className="ticket-card-foot"><span>اولویت: {priorityLabel[ticket.priority] ?? 'معمولی'}</span><time>{date(ticket.updatedAtUtc)}</time></div>
  </button>
}
