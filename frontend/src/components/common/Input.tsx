import { forwardRef, type InputHTMLAttributes } from 'react';

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  hint?: string;
}

const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, hint, className = '', ...props }, ref) => {
    return (
      <div className='flex flex-col gap-1.5'>
        {label && (
          <label className='text-sm font-medium text-gray-700'>
            {label}
            {props.required && <span className='text-red-500 ml-1'>*</span>}
          </label>
        )}

        <input
          ref={ref}
          {...props}
          className={`w-full px-3.5 py-2.5 text-sm border rounded-lg outline-none transition
            focus:ring-2 focus:ring-orange-300 focus:border-transparent
            disabled:bg-gray-50 disabled:text-gray-400 disabled:cursor-not-allowed
            ${
              error
                ? 'border-red-400 bg-red-50 text-red-900 placeholder-red-300'
                : 'border-gray-300 bg-white text-gray-900 placeholder-gray-400'
            }
            ${className}`}
        />

        {error && <p className='text-xs text-red-500'>{error}</p>}

        {hint && <p className='text-xs text-gray-400'>{hint}</p>}
      </div>
    );
  },
);

Input.displayName = 'Input';

export default Input;
