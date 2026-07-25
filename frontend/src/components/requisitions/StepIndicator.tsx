export default function StepIndicator({ current }: { current: number }) {
  const steps = ['Header', 'Lines'];
  return (
    <div className='flex items-center gap-2 mb-8'>
      {steps.map((label, i) => {
        const step = i + 1;
        const isActive = step === current;
        const isDone = step < current;
        return (
          <div key={label} className='flex items-center gap-2'>
            <div className='flex items-center gap-2'>
              <div
                className={`w-7 h-7 rounded-full flex items-center justify-center text-xs font-semibold transition
                    ${isActive ? 'bg-orange-500 text-white' : isDone ? 'bg-green-500 text-white' : 'bg-gray-100 text-gray-400'}`}
              >
                {isDone ? '✓' : step}
              </div>
              <span
                className={`text-sm font-medium ${isActive ? 'text-gray-900' : 'text-gray-400'}`}
              >
                {label}
              </span>
            </div>
            {i < steps.length - 1 && (
              <div
                className={`w-12 h-px mx-1 ${isDone ? 'bg-green-400' : 'bg-gray-200'}`}
              />
            )}
          </div>
        );
      })}
    </div>
  );
}
