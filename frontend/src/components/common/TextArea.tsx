import { forwardRef, type TextareaHTMLAttributes } from 'react';

interface TextAreaProps extends TextareaHTMLAttributes<HTMLTextAreaElement> {
  label?: string;
  placeholder?: string;
}

const TextArea = forwardRef<HTMLTextAreaElement, TextAreaProps>(
  ({ label, placeholder, className = '', ...props }, ref) => {
    return (
      <div className='col-span-2'>
        <label className='block text-sm font-medium text-gray-700 mb-1.5'>
          {label}
        </label>
        <textarea
          ref={ref}
          {...props}
          rows={3}
          placeholder={placeholder}
          className={`w-full px-3.5 py-2.5 text-sm border border-gray-300 rounded-lg outline-none transition focus:ring-2 focus:border-transparent focus:ring-orange-400 resize-none placeholder-gray-400
          ${className}
          `}
        />
      </div>
    );
  },
);

TextArea.displayName = 'TextArea';

export default TextArea;
