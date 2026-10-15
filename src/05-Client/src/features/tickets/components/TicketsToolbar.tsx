import { TicketPriority, TicketStatus } from '../types/ticket'

interface Props {
  status: TicketStatus | ''
  priority: TicketPriority | ''
  sortOrder: 'newest' | 'oldest'
  onStatus: (value: TicketStatus | '') => void
  onPriority: (value: TicketPriority | '') => void
  onSortOrder: (value: 'newest' | 'oldest') => void
  showStatus?: boolean
}

export function TicketsToolbar({ status, priority, sortOrder, onStatus, onPriority, onSortOrder, showStatus = true }: Props) {
  return <div className="ticket-toolbar">
    {showStatus && <select aria-label="فیلتر وضعیت" value={status} onChange={e => onStatus(e.target.value === '' ? '' : Number(e.target.value) as TicketStatus)}>
      <option value="">همه وضعیت‌ها</option>
      <option value={TicketStatus.Open}>جدید</option>
      <option value={TicketStatus.InProgress}>در حال بررسی</option>
      <option value={TicketStatus.Answered}>پاسخ داده‌شده</option>
      <option value={TicketStatus.Closed}>بسته‌شده</option>
    </select>}
    <select aria-label="فیلتر اولویت" value={priority} onChange={e => onPriority(e.target.value === '' ? '' : Number(e.target.value) as TicketPriority)}>
      <option value="">همه اولویت‌ها</option>
      <option value={TicketPriority.High}>زیاد</option>
      <option value={TicketPriority.Moderate}>معمولی</option>
      <option value={TicketPriority.Low}>کم</option>
    </select>
    <select aria-label="ترتیب درخواست‌ها" value={sortOrder} onChange={e => onSortOrder(e.target.value as 'newest' | 'oldest')}>
      <option value="newest">جدیدترین</option>
      <option value="oldest">قدیمی‌ترین</option>
    </select>
  </div>
}
