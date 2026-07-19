export default function SkeletonCard() {
  return (
    <div className='bg-white border border-gray-200 rounded-xl p-5 space-y-4'>
      <div className='flex justify-between'>
        <div className='space-y-1.5'>
          <div className='h-3 w-12 bg-gray-100 rounded animate-pulse' />
          <div className='h-4 w-40 bg-gray-100 rounded animate-pulse' />
        </div>
        <div className='h-5 w-20 bg-gray-100 rounded-full animate-pulse' />
      </div>
      <div className='space-y-1.5'>
        <div className='h-3 w-full bg-gray-100 rounded-full animate-pulse' />
        <div className='h-3 w-3/4 bg-gray-100 rounded-full animate-pulse' />
      </div>
      <div className='border-t border-gray-200' />
      <div className='flex justify-between'>
        <div className='h-3 w-24 bg-gray-100 rounded-full animate-pulse' />
        <div className='h-3 w-16 bg-gray-100 rounded-full animate-pulse' />
      </div>
    </div>
  );
}
