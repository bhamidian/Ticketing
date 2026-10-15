import { httpClient } from "@/shared/api/httpClient";
import type {
  CreateTicketRequest,
  TicketDetails,
  PagedTicketsResult,
  TicketPriority,
  TicketStatus,
} from "../types/ticket";
export class TicketsApi {
  list(filters: { status?: TicketStatus; priority?: TicketPriority; page?: number; pageSize?: number; sortOrder?: 'newest' | 'oldest'; assignedOnly?: boolean; unassignedOnly?: boolean } = {}) {
    const query = new URLSearchParams();
    if (filters.status !== undefined)
      query.set("status", String(filters.status));
    if (filters.priority !== undefined)
      query.set("priority", String(filters.priority));
    query.set("page", String(filters.page ?? 1));
    query.set("pageSize", String(filters.pageSize ?? 12));
    query.set("sortOrder", filters.sortOrder ?? 'newest');
    if (filters.assignedOnly) query.set("assignedOnly", "true");
    if (filters.unassignedOnly) query.set("unassignedOnly", "true");
    return httpClient.get<PagedTicketsResult>(
      `/api/Ticket${query.size ? `?${query}` : ""}`,
    );
  }
  get(id: string) {
    return httpClient.get<TicketDetails>(`/api/Ticket/${id}`);
  }
  claim(id: string) {
    return httpClient.post<TicketDetails>(`/api/Ticket/${id}/claim`);
  }
  create(request: CreateTicketRequest, image?: File | null) {
    const form = new FormData();
    form.set("subject", request.subject);
    form.set("message", request.message);
    form.set("priority", String(request.priority));
    if (image) form.set("image", image);
    return httpClient.post<TicketDetails>("/api/Ticket", form);
  }
  reply(id: string, message: string) {
    return httpClient.post<TicketDetails>(`/api/Ticket/${id}/messages`, {
      message,
    });
  }
  close(id: string) {
    return httpClient.post<TicketDetails>(`/api/Ticket/${id}/close`);
  }
  changeStatus(id: string, status: TicketStatus) {
    return httpClient.patch<TicketDetails>(`/api/Ticket/${id}/status`, {
      status,
    });
  }
}
export const ticketsApi = new TicketsApi();
