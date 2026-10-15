import { TicketStatus } from '../types/ticket'
const labels: Record<TicketStatus, string> = { [TicketStatus.Open]: 'جدید', [TicketStatus.InProgress]: 'در حال بررسی', [TicketStatus.Answered]: 'پاسخ داده‌شده', [TicketStatus.Closed]: 'بسته‌شده' }
export function TicketStatusBadge({ status }: { status: TicketStatus }) { return <span className={`status-badge status-${status}`}><i />{labels[status] ?? 'نامشخص'}</span> }
