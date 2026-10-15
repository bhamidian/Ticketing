export const TicketStatus = { Open: 0, InProgress: 1, Answered: 2, Closed: 3 } as const
export type TicketStatus = typeof TicketStatus[keyof typeof TicketStatus]
export const TicketPriority = { High: 0, Low: 1, Moderate: 2 } as const
export type TicketPriority = typeof TicketPriority[keyof typeof TicketPriority]
export interface TicketSummary { id: string; subject: string; status: TicketStatus; priority: TicketPriority; normalUserId: string; normalUserEmail: string | null; assignedAgentId: string | null; assignedAgentName: string | null; createdAtUtc: string; updatedAtUtc: string }
export interface PagedTicketsResult { items: TicketSummary[]; page: number; pageSize: number; totalCount: number; totalPages: number }
export interface TicketMessage { id: string; senderId: string; senderName: string; message: string; pictureUrl: string | null; createdAtUtc: string }
export interface TicketDetails extends TicketSummary { closedAtUtc: string | null; closedById: string | null; messages: TicketMessage[] }
export interface CreateTicketRequest { subject: string; message: string; priority: TicketPriority }
