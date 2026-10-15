import { useRef, useState } from "react";
import type { FormEvent } from "react";
import { Button } from "@/components/ui/Button";
import { TicketPriority } from "../types/ticket";
import type { TicketPriority as TicketPriorityType } from "../types/ticket";
export function NewTicketPage({
  onCancel,
  onCreate,
}: {
  onCancel: () => void;
  onCreate: (
    subject: string,
    message: string,
    priority: TicketPriority,
    image: File | null,
  ) => Promise<void>;
}) {
  const [subject, setSubject] = useState("");
  const [message, setMessage] = useState("");
  const [priority, setPriority] = useState<TicketPriorityType>(
    TicketPriority.Moderate,
  );
  const [image, setImage] = useState<File | null>(null);
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);
  const imageInput = useRef<HTMLInputElement>(null);
  async function submit(event: FormEvent) {
    event.preventDefault();
    setBusy(true);
    setError("");
    try {
      await onCreate(subject.trim(), message.trim(), priority, image);
    } catch (e) {
      setError(e instanceof Error ? e.message : "ثبت درخواست انجام نشد.");
    } finally {
      setBusy(false);
    }
  }
  return (
    <section className="page-wrap">
      <button className="back-link" onClick={onCancel}>
        ← بازگشت به درخواست‌ها
      </button>
      <div className="page-title">
        <div>
          <p className="eyebrow">درخواست پشتیبانی</p>
          <h1>ثبت تیکت جدید</h1>
          <p className="muted">
            موضوع و توضیحات را بنویس تا همکاران پشتیبانی پیگیری کنند.
          </p>
        </div>
      </div>
      <form className="panel form-stack new-ticket-form" onSubmit={submit}>
        <label>
          موضوع درخواست
          <input
            maxLength={200}
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            required
            placeholder="مثلاً مشکل در ورود به حساب"
          />
        </label>
        <label>
          اولویت
          <select
            value={priority}
            onChange={(e) =>
              setPriority(Number(e.target.value) as TicketPriorityType)
            }
          >
            <option value={TicketPriority.High}>زیاد</option>
            <option value={TicketPriority.Moderate}>معمولی</option>
            <option value={TicketPriority.Low}>کم</option>
          </select>
        </label>
        <label>
          شرح مشکل
          <textarea
            rows={7}
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            required
            placeholder="مشکل را با جزئیات توضیح بده…"
          />
        </label>
        <label>
          تصویر (اختیاری)
          <input
            type="file"
            ref={imageInput}
            accept="image/jpeg,image/png,image/gif,image/webp"
            onChange={(event) => {
              const file = event.currentTarget.files?.[0] ?? null;
              if (file && file.size > 5 * 1024 * 1024) {
                setError("حجم تصویر نباید بیشتر از ۵ مگابایت باشد.");
                event.currentTarget.value = "";
                setImage(null);
                return;
              }
              if (file && !["image/jpeg", "image/png", "image/gif", "image/webp"].includes(file.type)) {
                setError("فقط تصویرهای JPG، PNG، GIF یا WebP پذیرفته می‌شوند.");
                event.currentTarget.value = "";
                setImage(null);
                return;
              }
              setError("");
              setImage(file);
            }}
          />
        </label>
        {image && (
          <div className="selected-ticket-image" dir="rtl">
            <span>{image.name}</span>
            <Button type="button" variant="secondary" onClick={() => { setImage(null); if (imageInput.current) imageInput.current.value = ""; }}>
              حذف تصویر
            </Button>
          </div>
        )}
        {error && <p className="alert-error">{error}</p>}
        <div className="form-actions">
          <Button type="button" variant="secondary" onClick={onCancel}>
            انصراف
          </Button>
          <Button disabled={busy}>
            {busy ? "در حال ثبت…" : "ثبت درخواست"}
          </Button>
        </div>
      </form>
    </section>
  );
}
