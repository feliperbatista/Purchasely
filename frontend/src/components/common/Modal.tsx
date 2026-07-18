import type React from 'react';

type Props = {
  onClose: () => void;
  children: React.ReactNode;
};

export default function Modal({ onClose, children }: Props) {
  return (
    <div className='fixed inset-0 z-50 flex items-center justify-center'>
      <div className='absolute inset-0 bg-black/40' onClick={onClose} />
      {children}
    </div>
  );
}
