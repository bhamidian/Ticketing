import { useEffect, useState } from "react";
import type { FormEvent } from "react";
import { Button } from "@/components/ui/Button";
import { httpClient } from "@/shared/api/httpClient";
import type { TicketDetails } from "../types/ticket";
import { TicketStatus } from "../types/ticket";
import { TicketStatusBadge } from "../components/TicketStatusBadge";
import { formatPersianDate } from "@/shared/utils/date";
export function TicketDetailPage({
  ticket,
  userId,
  isAgent,
  isAdmin,
  onBack,
  onReply,
  onClose,
  onChangeStatus,
  onConfirmMessage,
  onEditMessage,
}: {
  ticket: TicketDetails;
  userId: string;
  isAgent: boolean;
  isAdmin: boolean;
  onBack: () => void;
  onReply: (text: string) => Promise<void>;
  onClose: () => Promise<void>;
  onChangeStatus: (status: TicketStatus) => Promise<void>;
  onConfirmMessage: (messageId: string) => Promise<void>;
  onEditMessage: (messageId: string, text: string) => Promise<void>;
}) {
  const [message, setMessage] = useState("");
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");
  const [imageUrls, setImageUrls] = useState<Record<string, string>>({});
  const [imageErrors, setImageErrors] = useState<Record<string, boolean>>({});
  const [confirmingId, setConfirmingId] = useState<string | null>(null);
  const [editingId, setEditingId] = useState<string | null>(null);
  const [editDraft, setEditDraft] = useState("");
  const [savingEditId, setSavingEditId] = useState<string | null>(null);
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
  async function confirmMessage(messageId: string) {
    setConfirmingId(messageId);
    setError("");
    try {
      await onConfirmMessage(messageId);
    } catch (e) {
      setError(e instanceof Error ? e.message : "تأیید پیام انجام نشد.");
    } finally {
      setConfirmingId(null);
    }
  }
  async function saveMessageEdit(event: FormEvent, messageId: string) {
    event.preventDefault();
    if (!editDraft.trim()) return;
    setSavingEditId(messageId);
    setError("");
    try {
      await onEditMessage(messageId, editDraft.trim());
      setEditingId(null);
      setEditDraft("");
    } catch (e) {
      setError(e instanceof Error ? e.message : "ویرایش پیام انجام نشد.");
    } finally {
      setSavingEditId(null);
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
            ثبت‌شده در {formatPersianDate(ticket.createdAtUtc, true)}
          </span>
          {(isAgent || isAdmin) && ticket.departmentName && <span className="ticket-department detail-ticket-department">دپارتمان: {ticket.departmentName}</span>}
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
          {!isAgent && !isAdmin && ticket.status !== TicketStatus.Closed && (
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
            const senderLabel = isAdmin
              ? item.senderName
              : isOwnMessage
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
                    <time>{formatPersianDate(item.createdAtUtc, true)}</time>
                    {isAdmin && item.isConfirmed === false && <span className="message-pending-badge">در انتظار تأیید</span>}
                  </div>
                  {editingId === item.id ? (
                    <form className="message-edit-form" onSubmit={(event) => void saveMessageEdit(event, item.id)}>
                      <textarea
                        rows={4}
                        maxLength={4000}
                        value={editDraft}
                        onChange={(event) => setEditDraft(event.target.value)}
                        aria-label="متن ویرایش‌شده پیام"
                        required
                      />
                      <div className="message-edit-actions">
                        <button className="button button-primary" type="submit" disabled={!editDraft.trim() || savingEditId === item.id}>
                          {savingEditId === item.id ? "در حال ذخیره…" : "ذخیره ویرایش"}
                        </button>
                        <button className="button button-secondary" type="button" disabled={savingEditId === item.id} onClick={() => { setEditingId(null); setEditDraft("") }}>
                          انصراف
                        </button>
                      </div>
                    </form>
                  ) : <p>{item.message}</p>}
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
                  {isAdmin && item.isConfirmed === false && <div className="pending-message-actions">
                    {editingId !== item.id && <button className="button button-secondary message-confirm-button" type="button" disabled={confirmingId === item.id} onClick={() => { setEditingId(item.id); setEditDraft(item.message) }}>ویرایش پیام</button>}
                    <button className="button button-primary message-confirm-button" type="button" disabled={confirmingId === item.id || savingEditId === item.id || editingId === item.id} onClick={() => void confirmMessage(item.id)}>{confirmingId === item.id ? "در حال تأیید…" : "تأیید و ارسال به کاربر"}</button>
                  </div>}
                </div>
              </article>
            );
          })}
        </div>
        {error && <p className="alert-error">{error}</p>}
        {!isAdmin && ticket.status !== TicketStatus.Closed && (!isAgent || isAssignedAgent) ? (
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
        ) : isAdmin ? (
          <p className="closed-note">ادمین فقط پیام‌های نیازمند تأیید را مدیریت می‌کند.</p>
        ) : ticket.status === TicketStatus.Closed ? (
          <p className="closed-note">این تیکت بسته شده است.</p>
        ) : (
          <p className="closed-note">فقط برای تیکت‌های واگذارشده به شما امکان پاسخ وجود دارد.</p>
        )}
      </div>
    </section>
  );
}
