import type { TicketSummary } from '../types/ticket'
import { TicketCard } from './TicketCard'
type AgentTicketTab = 'available' | 'assigned'

interface Props {
  items: TicketSummary[]
  isAgent: boolean
  emptyAgentTab: AgentTicketTab
  onOpen: (ticket: TicketSummary) => void
}

export function TicketList({ items, isAgent, emptyAgentTab, onOpen }: Props) {
  if (!items.length) {
    const title = !isAgent
      ? 'هنوز درخواستی اینجا نیست'
      : emptyAgentTab === 'available'
        ? 'درخواست منتشرشده‌ای وجود ندارد'
        : 'درخواستی به شما واگذار نشده است'
    const description = !isAgent
      ? 'با ثبت یک درخواست جدید، گفتگو با پشتیبانی را شروع کن.'
      : emptyAgentTab === 'available'
        ? 'درخواست‌های جدید در این بخش نمایش داده می‌شوند.'
        : 'درخواست‌هایی که شما پذیرفته‌اید در این بخش نمایش داده می‌شوند.'

    return (
      <div className="empty-state">
        <span>◇</span>
        <h3>{title}</h3>
        <p>{description}</p>
      </div>
    )
  }

  return (
    <div className="ticket-list">
      {items.map((ticket) => (
        <TicketCard
          key={ticket.id}
          ticket={ticket}
          showRequesterEmail={isAgent}
          onOpen={() => onOpen(ticket)}
        />
      ))}
    </div>
  )
}
