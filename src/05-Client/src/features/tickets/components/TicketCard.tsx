import type { TicketSummary } from '../types/ticket'
import { TicketPriority } from '../types/ticket'
import { TicketStatusBadge } from './TicketStatusBadge'
import { formatPersianDate } from '@/shared/utils/date'
const priorityLabel = { [TicketPriority.High]: 'زیاد', [TicketPriority.Moderate]: 'معمولی', [TicketPriority.Low]: 'کم' }
export function TicketCard({ ticket, showRequesterEmail, onOpen }: { ticket: TicketSummary; showRequesterEmail: boolean; onOpen: () => void }) {
  return <button className="ticket-card" onClick={onOpen}>
    <div className="ticket-card-head"><span className="ticket-id">#{ticket.id.slice(0, 8)}</span><TicketStatusBadge status={ticket.status} /></div>
    <h3>{ticket.subject}</h3>
    {showRequesterEmail && ticket.normalUserEmail && <span className="ticket-requester-email" dir="ltr">{ticket.normalUserEmail}</span>}
    {showRequesterEmail && ticket.departmentName && <span className="ticket-department">دپارتمان: {ticket.departmentName}</span>}
    <div className="ticket-card-foot"><span>اولویت: {priorityLabel[ticket.priority] ?? 'معمولی'}</span><time>{formatPersianDate(ticket.updatedAtUtc)}</time></div>
  </button>
}
