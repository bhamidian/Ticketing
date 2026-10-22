const hasTimezone = /(?:Z|[+-]\d{2}:?\d{2})$/i

export function parseUtcDate(value: string): Date {
  const timestamp = hasTimezone.test(value) ? value : `${value}Z`
  return new Date(timestamp)
}

export function formatPersianDate(value: string, includeTime = false): string {
  return new Intl.DateTimeFormat('fa-IR', {
    dateStyle: 'medium',
    ...(includeTime ? { timeStyle: 'short' as const } : {}),
  }).format(parseUtcDate(value))
}
