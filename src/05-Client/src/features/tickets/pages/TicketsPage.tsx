import { useEffect, useState } from "react";
import { Button } from "@/components/ui/Button";
import { ticketsStore, useTicketsStore } from "../store/TicketsStore";
import { TicketPriority, TicketStatus } from "../types/ticket";
import type { TicketSummary } from "../types/ticket";
import { TicketList } from "../components/TicketList";
import { TicketsToolbar } from "../components/TicketsToolbar";
export function TicketsPage({
  isAgent,
  onNew,
  onOpen,
}: {
  isAgent: boolean;
  onNew: () => void;
  onOpen: (ticket: TicketSummary) => void;
}) {
  const { items, totalPages, totalCount, loading, error } = useTicketsStore();
  const pageSize = 12;
  const [status, setStatus] = useState<TicketStatus | "">("");
  const [priority, setPriority] = useState<TicketPriority | "">("");
  const [sortOrder, setSortOrder] = useState<"newest" | "oldest">("newest");
  const [page, setPage] = useState(1);
  useEffect(() => {
    void ticketsStore.load({
      status: status === "" ? undefined : status,
      priority: priority === "" ? undefined : priority,
      page,
      pageSize,
      sortOrder,
      assignedOnly: isAgent,
      unassignedOnly: false,
    });
  }, [status, priority, page, sortOrder, isAgent]);
  const visibleItems = items;
  return (
    <section className="page-wrap">
      <header className="page-title">
        <div>
          <p className="eyebrow">{isAgent ? "میز پشتیبانی" : "فضای کاربری"}</p>
          <h1>{isAgent ? "درخواست‌های پشتیبانی" : "درخواست‌های من"}</h1>
          <p className="muted">
            {isAgent
              ? "درخواست‌ها را بررسی و به کاربران پاسخ بده."
              : "همهٔ گفتگوها و درخواست‌های پشتیبانی‌ات را دنبال کن."}
          </p>
        </div>
        {!isAgent && <Button onClick={onNew}>＋ ثبت تیکت جدید</Button>}
      </header>
      <TicketsToolbar
        status={status}
        priority={priority}
        sortOrder={sortOrder}
        onStatus={(value) => { setStatus(value); setPage(1); }}
        onPriority={(value) => { setPriority(value); setPage(1); }}
        onSortOrder={(value) => { setSortOrder(value); setPage(1); }}
        showStatus
      />
      {error && (
        <div className="alert-error" role="alert">
          {error}{" "}
          <button onClick={() => void ticketsStore.load({ status: status === "" ? undefined : status, priority: priority === "" ? undefined : priority, page, pageSize, sortOrder, assignedOnly: isAgent, unassignedOnly: false })}>تلاش دوباره</button>
        </div>
      )}
      {loading ? (
        <div className="loading-state">در حال دریافت درخواست‌ها…</div>
      ) : (
        <TicketList
          items={visibleItems}
          isAgent={isAgent}
          emptyAgentTab="assigned"
          onOpen={onOpen}
        />
      )}
      {totalCount > 0 && <div className="ticket-pagination" dir="rtl">
        <span>نمایش {Math.min((page - 1) * pageSize + 1, totalCount)} تا {Math.min(page * pageSize, totalCount)} از {totalCount} درخواست</span>
        <div className="ticket-pagination-controls">
          <button className="button button-secondary" type="button" disabled={loading || page <= 1} onClick={() => setPage(current => Math.max(1, current - 1))}>قبلی</button>
          <span>صفحه {page} از {Math.max(totalPages, 1)}</span>
          <button className="button button-secondary" type="button" disabled={loading || page >= totalPages} onClick={() => setPage(current => current + 1)}>بعدی</button>
        </div>
      </div>}
    </section>
  );
}
