export default function SkeletonRow() {
  return (
    <>
      {Array.from({ length: 5 }).map((_, i) => (
        <tr key={i}>
          {Array.from({ length: 7 }).map((_, j) => (
            <td key={j} className='px-4 py-3'>
              <div className='h-4 bg-gray-200 rounded animate-pulse' />
            </td>
          ))}
        </tr>
      ))}
    </>
  );
}
