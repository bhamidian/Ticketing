import { useSyncExternalStore } from "react";
import { ticketsApi } from "../api/TicketsApi";
import type {
  CreateTicketRequest,
  TicketDetails,
  TicketPriority,
  PagedTicketsResult,
  TicketSummary,
} from "../types/ticket";
import { TicketStatus } from "../types/ticket";
interface TicketsState {
  items: TicketSummary[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  selected: TicketDetails | null;
  loading: boolean;
  error: string | null;
}
class TicketsStoreClass {
  private state: TicketsState = {
    items: [],
    page: 1,
    pageSize: 12,
    totalCount: 0,
    totalPages: 0,
    selected: null,
    loading: false,
    error: null,
  };
  private listeners = new Set<() => void>();
  getSnapshot = () => this.state;
  subscribe = (listener: () => void) => {
    this.listeners.add(listener);
    return () => this.listeners.delete(listener);
  };
  private set(patch: Partial<TicketsState>) {
    this.state = { ...this.state, ...patch };
    this.listeners.forEach((listener) => listener());
  }
  async load(
    filters: { status?: TicketStatus; priority?: TicketPriority; page?: number; pageSize?: number; sortOrder?: 'newest' | 'oldest'; assignedOnly?: boolean; unassignedOnly?: boolean } = {},
  ) {
    this.set({ loading: true, error: null });
    try {
      const result: PagedTicketsResult = await ticketsApi.list(filters);
      this.set({ items: result.items, page: result.page, pageSize: result.pageSize, totalCount: result.totalCount, totalPages: result.totalPages });
    } catch (e) {
      this.set({
        error: e instanceof Error ? e.message : "دریافت تیکت‌ها ناموفق بود.",
      });
    } finally {
      this.set({ loading: false });
    }
  }
  async open(id: string) {
    this.set({ loading: true, error: null, selected: null });
    try {
      const ticket = await ticketsApi.get(id);
      this.set({
        selected: ticket,
        items: this.state.items.map((item) => item.id === id ? ticket : item),
      });
    } catch (e) {
      this.set({
        error: e instanceof Error ? e.message : "دریافت گفتگو ناموفق بود.",
      });
    } finally {
      this.set({ loading: false });
    }
  }
  async create(request: CreateTicketRequest, image?: File | null) {
    const ticket = await ticketsApi.create(request, image);
    this.set({ selected: ticket, items: [ticket, ...this.state.items] });
    return ticket;
  }
  async reply(id: string, message: string) {
    const ticket = await ticketsApi.reply(id, message);
    this.set({ selected: ticket });
    return ticket;
  }
  async close(id: string) {
    const ticket = await ticketsApi.close(id);
    this.set({
      selected: ticket,
      items: this.state.items.map((item) => (item.id === id ? ticket : item)),
    });
    return ticket;
  }
  async changeStatus(id: string, status: TicketStatus) {
    const ticket = await ticketsApi.changeStatus(id, status);
    this.set({
      selected: ticket,
      items: this.state.items.map((item) => (item.id === id ? ticket : item)),
    });
    return ticket;
  }
  async confirmMessage(ticketId: string, messageId: string) {
    await ticketsApi.confirmMessage(messageId);
    const ticket = await ticketsApi.get(ticketId);
    this.set({ selected: ticket, items: this.state.items.map(item => item.id === ticketId ? ticket : item) });
    return ticket;
  }
  async updateAgentMessage(ticketId: string, messageId: string, message: string) {
    await ticketsApi.updateAgentMessage(ticketId, messageId, message);
    const ticket = await ticketsApi.get(ticketId);
    this.set({ selected: ticket, items: this.state.items.map(item => item.id === ticketId ? ticket : item) });
    return ticket;
  }
}
export const ticketsStore = new TicketsStoreClass();
export function useTicketsStore() {
  return useSyncExternalStore(
    ticketsStore.subscribe,
    ticketsStore.getSnapshot,
    ticketsStore.getSnapshot,
  );
}
