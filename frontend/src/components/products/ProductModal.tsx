import { useForm } from 'react-hook-form';
import type { Product } from '../../types/product';
import {
  productSchema,
  type ProductFormData,
} from '../../schemas/product.schema';
import { zodResolver } from '@hookform/resolvers/zod';
import { useProducts } from '../../hooks/useProducts';
import { Trash2, X } from 'lucide-react';
import Input from '../common/Input';
import Button from '../common/Button';
import TextArea from '../common/TextArea';
import Modal from '../common/Modal';
import { useState } from 'react';
import { toast } from 'sonner';
import { getErrorMessage } from '../../lib/errors';

interface ProductModalProps {
  product?: Product;
  onClose: () => void;
}

export default function ProductModal({ product, onClose }: ProductModalProps) {
  const { update, create, remove } = useProducts();
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);
  const isEditing = !!product;

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ProductFormData>({
    resolver: zodResolver(productSchema),
    defaultValues: product ?? {},
  });

  const onSubmit = (data: ProductFormData) => {
    if (isEditing) {
      update.reset();
      update.mutate(
        {
          id: product.id,
          data,
        },
        {
          onSuccess: onClose,
        },
      );
    } else {
      create.reset();
      create.mutate(data, { onSuccess: onClose });
    }
  };

  const handleDelete = (id: string) => {
    remove.mutate(id, {
      onSuccess: onClose,
      onError: (error) => toast.error(getErrorMessage(error)),
    });
  };

  const isPending = create.isPending || update.isPending;
  const isError = create.isError || update.isError;

  return (
    <Modal onClose={onClose}>
      <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-md mx-4 p-6'>
        <div className='flex items-center justify-between mb-6'>
          <h2 className='text-base font-semibold text-gray-900'>
            {isEditing ? 'Edit Product' : 'New Product'}
          </h2>
          <button
            onClick={onClose}
            className='text-gray-400 hover:text-gray-600 transition'
          >
            <X className='w-5 h-5' />
          </button>
        </div>

        {isError && (
          <div className='mb-4 px-4 py-3 bg-red-50 border border-red-200 rounded-lg text-sm text-red-600'>
            Something went wrong. Please try again.
          </div>
        )}

        <form onSubmit={handleSubmit(onSubmit)} className='space-y-4'>
          <div className='grid grid-cols-2 gap-4'>
            <div className='col-span-2 flex flex-col gap-2'>
              <Input
                label='Name'
                placeholder='Office Chair'
                error={errors.name?.message}
                required
                {...register('name')}
              />
              <Input
                label='SKU'
                placeholder='SKU-001'
                error={errors.sku?.message}
                required
                {...register('sku')}
              />
              <Input
                label='Category'
                placeholder='Furniture'
                error={errors.category?.message}
                required
                {...register('category')}
              />

              <TextArea
                label='Description'
                placeholder='Optional description...'
                {...register('description')}
              />
            </div>

            <div className='flex col-span-2 justify-end gap-2 pt-2'>
              {product && (
                <Button
                  variant='danger'
                  type='button'
                  onClick={() => setShowDeleteDialog(true)}
                  icon={<Trash2 className='h-4 w-4' />}
                >
                  Delete
                </Button>
              )}
              <Button variant='secondary' type='button' onClick={onClose}>
                Cancel
              </Button>
              <Button type='submit' loading={isPending}>
                {isEditing ? 'Save Changes' : 'Create Product'}
              </Button>
            </div>
          </div>
        </form>
      </div>

      {showDeleteDialog && (
        <Modal onClose={() => setShowDeleteDialog(false)}>
          <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-sm mx-4 p-6'>
            <h2 className='text-base font-semibold text-gray-900 mb-2'>
              Delete Product
            </h2>
            <p>
              Are you sure you want to delete <strong>{product?.name}</strong>?
              This action cannot be undone.
            </p>
            <div className='flex justify-end gap-2'>
              <Button
                variant='secondary'
                onClick={() => setShowDeleteDialog(false)}
              >
                Cancel
              </Button>
              <Button
                variant='danger'
                loading={remove.isPending}
                onClick={() => handleDelete(product!.id)}
              >
                Delete
              </Button>
            </div>
          </div>
        </Modal>
      )}
    </Modal>
  );
}
