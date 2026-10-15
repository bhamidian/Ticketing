import type { ButtonHTMLAttributes, PropsWithChildren } from 'react'
type Props = PropsWithChildren<ButtonHTMLAttributes<HTMLButtonElement> & { variant?: 'primary' | 'secondary' | 'ghost' }>
export function Button({ variant = 'primary', className = '', ...props }: Props) {
  return <button className={`button button-${variant} ${className}`} {...props} />
}
