import type { ButtonHTMLAttributes, ReactNode } from 'react';

type Variant = 'primary' | 'secondary' | 'danger' | 'ghost';
type Size = 'sm' | 'md' | 'lg';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: Variant;
  size?: Size;
  loading?: boolean;
  icon?: ReactNode;
  children: ReactNode;
}

const variants: Record<Variant, string> = {
  primary: 'bg-blue-600 hover:bg-blue-700 text-white disabled:bg-blue-400',
  secondary:
    'bg-white hover:bg-gray-50 text-gray-700 border border-gray-300 disabled:bg-gray-50 disabled:text-gray-400',
  danger: 'bg-red-600 hover:bg-red-700 text-white disabled:bg-red-400',
  ghost:
    'bg-transparent hover:bg-gray-100 text-gray-600 disabled:text-gray-300',
};

const sizes: Record<Size, string> = {
  sm: 'px-3 py-1.5 text-xs',
  md: 'px-4 py-2.5 text-sm',
  lg: 'px-5 py-3 text-base',
};

export default function Button({
  variant = 'primary',
  size = 'md',
  loading = false,
  icon,
  children,
  disabled,
  className = '',
  ...props
}: ButtonProps) {
  return (
    <button
      disabled={disabled || loading}
      {...props}
      className={`inline-flex items-center justify-center gap-2 font-medium rounded-lg transition cursor-pointer disabled:cursor-not-allowed 
        ${variants[variant]}
        ${sizes[size]}
        ${className}`}
    >
      {loading ? (
        <span className='w-4 h-4 border-2 border-current border-t-transparent rounded-full animate-spin' />
      ) : (
        icon && <span className='shrink-0'>{icon}</span>
      )}
      {children}
    </button>
  );
}
