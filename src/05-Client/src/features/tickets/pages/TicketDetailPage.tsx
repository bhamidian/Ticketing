import { useEffect, useState } from "react";
import type { FormEvent } from "react";
import { Button } from "@/components/ui/Button";
import { httpClient } from "@/shared/api/httpClient";
import type { TicketDetails } from "../types/ticket";
import { TicketStatus } from "../types/ticket";
import { TicketStatusBadge } from "../components/TicketStatusBadge";
const formatDate = (value: string) =>
  new Intl.DateTimeFormat("fa-IR", {
    dateStyle: "medium",
    timeStyle: "short",
  }).format(new Date(value));
export function TicketDetailPage({
  ticket,
  userId,
  isAgent,
  onBack,
  onReply,
  onClose,
  onClaim,
  onChangeStatus,
}: {
  ticket: TicketDetails;
  userId: string;
  isAgent: boolean;
  onBack: () => void;
  onReply: (text: string) => Promise<void>;
  onClose: () => Promise<void>;
  onClaim: () => Promise<void>;
  onChangeStatus: (status: TicketStatus) => Promise<void>;
}) {
  const [message, setMessage] = useState("");
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");
  const [imageUrls, setImageUrls] = useState<Record<string, string>>({});
  const [imageErrors, setImageErrors] = useState<Record<string, boolean>>({});
  const isAssignedAgent = isAgent && ticket.assignedAgentId === userId;

  useEffect(() => {
    let active = true;
    const objectUrls: string[] = [];
    setImageUrls({});
    setImageErrors({});

    for (const item of ticket.messages) {
      if (!item.pictureUrl) continue;
      void httpClient.getBlob(item.pictureUrl).then((blob) => {
        const objectUrl = URL.createObjectURL(blob);
        objectUrls.push(objectUrl);
        if (active) setImageUrls((current) => ({ ...current, [item.id]: objectUrl }));
        else URL.revokeObjectURL(objectUrl);
      }).catch(() => {
        if (active) setImageErrors((current) => ({ ...current, [item.id]: true }));
      });
    }

    return () => {
      active = false;
      objectUrls.forEach((url) => URL.revokeObjectURL(url));
    };
  }, [ticket.id, ticket.messages]);

  async function submit(event: FormEvent) {
    event.preventDefault();
    if (!message.trim()) return;
    setBusy(true);
    setError("");
    try {
      await onReply(message.trim());
      setMessage("");
    } catch (e) {
      setError(e instanceof Error ? e.message : "ارسال پیام ناموفق بود.");
    } finally {
      setBusy(false);
    }
  }
  async function changeStatus(status: TicketStatus) {
    if (status === ticket.status) return;
    setBusy(true);
    setError("");
    try {
      await onChangeStatus(status);
    } catch (e) {
      setError(e instanceof Error ? e.message : "تغییر وضعیت تیکت انجام نشد.");
    } finally {
      setBusy(false);
    }
  }
  async function claimTicket() {
    setBusy(true);
    setError("");
    try {
      await onClaim();
    } catch (e) {
      setError(e instanceof Error ? e.message : "پذیرفتن مسئولیت تیکت ناموفق بود.");
    } finally {
      setBusy(false);
    }
  }
  return (
    <section className="page-wrap">
      <button className="back-link" onClick={onBack}>
        ← بازگشت به فهرست
      </button>
      <header className="detail-heading panel">
        <div>
          <span className="ticket-id">#{ticket.id.slice(0, 8)}</span>
          <h1>{ticket.subject}</h1>
          <span className="muted">
            ثبت‌شده در {formatDate(ticket.createdAtUtc)}
          </span>
        </div>
        <TicketStatusBadge status={ticket.status} />
        {isAssignedAgent && (
          <label className="ticket-status-control">
            وضعیت تیکت
            <select
              aria-label="تغییر وضعیت تیکت"
              value={ticket.status}
              disabled={busy}
              onChange={(event) =>
                void changeStatus(Number(event.target.value) as TicketStatus)
              }
            >
              <option value={TicketStatus.Open}>جدید</option>
              <option value={TicketStatus.InProgress}>در حال بررسی</option>
              <option value={TicketStatus.Answered}>پاسخ‌داده‌شده</option>
              <option value={TicketStatus.Closed}>بسته‌شده</option>
            </select>
          </label>
        )}
      </header>
      <div className="conversation panel">
        <div className="conversation-title">
          <div>
            <p className="eyebrow">گفتگوی پشتیبانی</p>
            <h2>پیام‌ها</h2>
          </div>
          {isAgent && !ticket.assignedAgentId && ticket.status !== TicketStatus.Closed && (
            <Button disabled={busy} onClick={() => void claimTicket()}>
              {busy ? "در حال پذیرش…" : "قبول مسئولیت تیکت"}
            </Button>
          )}
          {!isAgent && ticket.status !== TicketStatus.Closed && (
            <Button
              variant="secondary"
              onClick={async () => {
                setBusy(true);
                try {
                  await onClose();
                } catch (e) {
                  setError(
                    e instanceof Error ? e.message : "بستن تیکت ناموفق بود.",
                  );
                } finally {
                  setBusy(false);
                }
              }}
            >
              بستن تیکت
            </Button>
          )}
        </div>
        <div className="messages">
          {ticket.messages.map((item) => {
            const isOwnMessage = item.senderId === userId;
            const senderLabel = isOwnMessage
              ? "شما"
              : isAgent
                ? "کاربر"
                : "پشتیبانی";

            return (
              <article
                className={`message ${isOwnMessage ? "message-own" : ""}`}
                key={item.id}
              >
                <div className="message-avatar">
                  {senderLabel.slice(0, 1)}
                </div>
                <div className="message-body">
                  <div className="message-meta">
                    <strong>{senderLabel}</strong>
                    <time>{formatDate(item.createdAtUtc)}</time>
                  </div>
                  <p>{item.message}</p>
                  {item.pictureUrl ? (
                    imageUrls[item.id] ? (
                      <a className="message-image-link" href={imageUrls[item.id]} target="_blank" rel="noreferrer">
                        <img className="message-image" src={imageUrls[item.id]} alt="تصویر پیوست‌شده" />
                      </a>
                    ) : (
                      <span className="message-image-note">
                        {imageErrors[item.id] ? "بارگذاری تصویر ناموفق بود." : "در حال بارگذاری تصویر…"}
                      </span>
                    )
                  ) : (
                    null
                  )}
                </div>
              </article>
            );
          })}
        </div>
        {error && <p className="alert-error">{error}</p>}
        {ticket.status !== TicketStatus.Closed && (!isAgent || isAssignedAgent) ? (
          <form className="composer" onSubmit={submit}>
            <textarea
              rows={3}
              value={message}
              onChange={(e) => setMessage(e.target.value)}
              placeholder="پاسخت را بنویس…"
              required
            />
            <Button disabled={busy || !message.trim()}>
              {busy
                ? "در حال ارسال…"
                : isAgent
                  ? "ارسال پاسخ به کاربر"
                  : "ارسال پیام"}
            </Button>
          </form>
        ) : ticket.status === TicketStatus.Closed ? (
          <p className="closed-note">این تیکت بسته شده است.</p>
        ) : (
          <p className="closed-note">برای پاسخ‌دادن، ابتدا مسئولیت این تیکت را بپذیرید.</p>
        )}
      </div>
    </section>
  );
}
