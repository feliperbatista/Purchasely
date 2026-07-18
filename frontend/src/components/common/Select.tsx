import { forwardRef, type SelectHTMLAttributes } from 'react';

interface SelectProps extends SelectHTMLAttributes<HTMLSelectElement> {
  label?: string;
  errors?: string;
  selected?: string;
  options: { id: string; option: string }[];
}

const Select = forwardRef<HTMLSelectElement, SelectProps>(
  ({ label, errors, selected, options, className = '', ...props }, ref) => {
    return (
      <div>
        <label className='block text-sm font-medium text-gray-700 mb-1.5'>
          {label} <span className='text-red-500'>*</span>
        </label>
        <select
          ref={ref}
          {...props}
          className={`w-full px-3.5 py-2.5 text-sm border rounded-lg outline-none transition
                        focus:ring-2 focus:ring-orange-300 focus:border-transparent bg-white
                        disabled:bg-gray-50 disabled:text-gray-400 disabled:cursor-not-allowed
                        ${errors ? 'border-red-400 bg-red-50' : 'border-gray-300'}
                        ${className}`}
        >
          {!selected && <option value=''>Select a product</option>}
          {options.map((op) => (
            <option key={op.id} value={op.id}>
              {op.option}
            </option>
          ))}
        </select>
        {errors && <p className='mt-1.5 text-xs text-red-500'>{errors}</p>}
      </div>
    );
  },
);

Select.displayName = 'Select';

export default Select;
