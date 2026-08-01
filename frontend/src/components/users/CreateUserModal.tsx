import { useForm } from 'react-hook-form';
import { useUsers } from '../../hooks/useUsers';
import {
  createUserSchema,
  type CreateUserFormData,
} from '../../schemas/user.schema';
import { zodResolver } from '@hookform/resolvers/zod';
import { toast } from 'sonner';
import { getErrorMessage } from '../../lib/errors';
import Modal from '../common/Modal';
import { X } from 'lucide-react';
import Input from '../common/Input';
import Button from '../common/Button';

type Props = {
  onClose: () => void;
};

export default function CreateUserModal({ onClose }: Props) {
  const { create } = useUsers();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CreateUserFormData>({
    resolver: zodResolver(createUserSchema),
    defaultValues: { role: 'Requester' },
  });

  const onSubmit = (data: CreateUserFormData) => {
    create.mutate(data, {
      onSuccess: () => {
        toast.success('User created sucessfully');
        onClose();
      },
      onError: (e) => toast.error(getErrorMessage(e)),
    });
  };

  return (
    <Modal onClose={onClose}>
      <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-md mx-4 p-6'>
        <div className='flex items-center justify-between mb-6'>
          <h2 className='text-base font-semibold text-gray-900'>New User</h2>
          <button
            onClick={onClose}
            className='text-gray-400 hover:text-gray-600 transition'
          >
            <X />
          </button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className='space-y-4'>
          <Input
            label='Name'
            placeholder="User's name"
            error={errors.name?.message}
            required
            {...register('name')}
          />
          <Input
            label='Email'
            type='email'
            placeholder='name@company.com'
            error={errors.email?.message}
            required
            {...register('email')}
          />
          <Input
            label='Password'
            type='password'
            placeholder='Min. 8 characters'
            error={errors.password?.message}
            required
            {...register('password')}
          />
          <div>
            <label className='block text-sm font-medium text-gray-700 mb-1.5'>
              Role <span className='text-red-500'>*</span>
            </label>
            <select
              {...register('role')}
              className='w-full px-3.5 py-2.5 text-sm border border-gray-300 rounded-lg outline-none focus:ring-2 focus:ring-orange-300 bg-white'
            >
              <option value='Requester'>Requester</option>
              <option value='Manager'>Manager</option>
              <option value='Buyer'>Buyer</option>
              <option value='Admin'>Admin</option>
            </select>
            {errors.role && (
              <p className='mt-1.5 text-xs text-red-500'>
                {errors.role.message}
              </p>
            )}
          </div>
          <div className='flex justify-end gap-2 pt-2'>
            <Button variant='secondary' type='button' onClick={onClose}>
              Cancel
            </Button>
            <Button type='submit' loading={create.isPending}>
              Create User
            </Button>
          </div>
        </form>
      </div>
    </Modal>
  );
}
