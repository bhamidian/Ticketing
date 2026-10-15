import { useState } from 'react'
import type { FormEvent } from 'react'
import { Button } from '@/components/ui/Button'
export function TicketComposer({ onSend, disabled }: { onSend: (message: string) => Promise<void>; disabled?: boolean }) {
  const [message, setMessage] = useState('')
  async function submit(event: FormEvent) { event.preventDefault(); if (!message.trim()) return; await onSend(message.trim()); setMessage('') }
  return <form className="composer" onSubmit={submit}><textarea value={message} onChange={e => setMessage(e.target.value)} placeholder="پیامت را بنویس…" rows={3} required /><Button disabled={disabled || !message.trim()}>ارسال پاسخ</Button></form>
}
