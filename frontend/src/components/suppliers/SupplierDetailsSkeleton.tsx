export default function SupplierDetailsSkeleton() {
  return (
    <div className='space-y-5'>
      <div className='h-7 w-48 bg-gray-200 rounded animate-pulse' />

      <div className='grid sm:grid-cols-1 lg:grid-cols-2 gap-3'>
        <div className='flex flex-col gap-4 px-2'>
          {Array.from({ length: 5 }).map((_, i) => (
            <div key={i} className='space-y-1.5'>
              <div className='h-4 w-24 bg-gray-200 rounded animate-pulse' />
              <div className='h-10 w-full bg-gray-200 rounded-lg animate-pulse' />
            </div>
          ))}
          <div className='h-10 w-24 bg-gray-200 rounded-lg animate-pulse self-end' />
        </div>

        <div className='flex flex-col gap-1.5'>
          <div className='flex items-center justify-between mb-1'>
            <div className='h-4 w-32 bg-gray-200 rounded animate-pulse' />
            <div className='h-8 w-16 bg-gray-200 rounded-lg animate-pulse' />
          </div>
          <div className='bg-white rounded-xl border border-gray-200 p-5 space-y-3'>
            {Array.from({ length: 5 }).map((_, i) => (
              <div key={i} className='flex justify-between'>
                <div className='h-4 w-40 bg-gray-200 rounded animate-pulse' />
                <div className='h-4 w-16 bg-gray-200 rounded animate-pulse' />
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
