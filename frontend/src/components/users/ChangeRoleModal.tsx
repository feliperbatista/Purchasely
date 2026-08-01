import { useState } from 'react';
import { useUsers } from '../../hooks/useUsers';
import type { User } from '../../types/auth';
import type { UserRole } from '../../types/users';
import { toast } from 'sonner';
import { getErrorMessage } from '../../lib/errors';
import Modal from '../common/Modal';
import Button from '../common/Button';
import { Shield, X } from 'lucide-react';

type Props = {
  user: User;
  onClose: () => void;
};

export default function ChangeRoleModal({ user, onClose }: Props) {
  const { changeRole } = useUsers();
  const [role, setRole] = useState<UserRole>(user.role);

  const handleSave = () => {
    changeRole.mutate(
      {
        id: user.id,
        role,
      },
      {
        onSuccess: () => {
          toast.success(`${user.name}'s role updated to ${role}`);
          onClose();
        },
        onError: (e) => toast.error(getErrorMessage(e)),
      },
    );
  };

  return (
    <Modal onClose={onClose}>
      <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-sm mx-4 p-6'>
        <div className='flex items-center justify-between mb-6'>
          <h2 className='text-base font-semibold text-gray-900'>Change Role</h2>
          <button
            onClick={onClose}
            className='text-gray-400 hover:text-gray-600 transition'
          >
            <X className='w-5 h-5' />
          </button>
        </div>

        <p className='text-sm text-gray-500 mb-4'>
          Changing role for <strong>{user.name}</strong>
        </p>

        <select
          value={role}
          onChange={(e) => setRole(e.target.value as UserRole)}
          className='w-full px-3.5 py-2.5 text-sm border border-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-orange-400 bg-white mb-6'
        >
          <option value='Requester'>Requester</option>
          <option value='Manager'>Manager</option>
          <option value='Buyer'>Buyer</option>
          <option value='Admin'>Admin</option>
        </select>

        <div className='flex justify-end gap-2'>
          <Button variant='secondary' onClick={onClose}>
            Cancel
          </Button>
          <Button
            icon={<Shield className='w-4 h-4' />}
            loading={changeRole.isPending}
            disabled={role === user.role}
            onClick={handleSave}
          >
            Save Role
          </Button>
        </div>
      </div>
    </Modal>
  );
}
