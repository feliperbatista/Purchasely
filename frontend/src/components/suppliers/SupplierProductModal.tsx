import { useState } from 'react';
import { useSuppliers } from '../../hooks/useSuppliers';
import type { Product } from '../../types/product';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  supplierProductSchema,
  type SupplierProductFormData,
} from '../../schemas/supplierProduct.schema';
import { toast } from 'sonner';
import { getErrorMessage } from '../../lib/errors';
import Modal from '../common/Modal';
import { Trash2, X } from 'lucide-react';
import Input from '../common/Input';
import Button from '../common/Button';
import DeleteDialog from '../common/DeleteDialog';
import { useProducts } from '../../hooks/useProducts';
import Select from '../common/Select';

type Props = {
  supplierId: string;
  product?: Product;
  onClose: () => void;
};

export default function SupplierProductModal({
  supplierId,
  product,
  onClose,
}: Props) {
  const { addProduct, removeProduct } = useSuppliers(supplierId);
  const { products } = useProducts();
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SupplierProductFormData>({
    resolver: zodResolver(supplierProductSchema),
    defaultValues: product ?? {},
  });

  const onSubmit = (data: SupplierProductFormData) =>
    addProduct.mutate(
      {
        productId: data.productId,
        unitPrice: data.unitPrice,
      },
      {
        onSuccess: onClose,
        onError: (error) => toast.error(getErrorMessage(error)),
      },
    );

  const handleDelete = (id: string) => {
    removeProduct.mutate(id, {
      onSuccess: onClose,
      onError: (error) => toast.error(getErrorMessage(error)),
    });
  };

  return (
    <Modal onClose={onClose}>
      <div className='relative bg-white rounded-2xl shadow-xl w-full max-w-md mx-4 p-6'>
        <div className='flex items-center justify-between mb-6'>
          <h2 className='text-base font-semibold text-gray-900'>
            {product ? 'View Product' : 'New Product'}
          </h2>
          <button
            className='text-gray-400 hover:text-gray-600 transition'
            onClick={onClose}
          >
            <X className='w-5 h-5' />
          </button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className='space-y-4'>
          <div className='grid grid-cols-2 gap-4'>
            <div className='col-span-2 flex flex-col gap-2'>
              <Select
                label='Product'
                errors={errors.productId?.message}
                selected={product?.name}
                disabled={!!product}
                options={products.map((product) => {
                  return {
                    id: product.id,
                    option: `${product.name} - ${product.sku}`,
                  };
                })}
              />

              <Input
                label='Unit Price'
                placeholder='1.00'
                error={errors.unitPrice?.message}
                required
                disabled={!!product}
                {...register('unitPrice', {
                  valueAsNumber: true,
                })}
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
              {!product && (
                <Button type='submit' loading={addProduct.isPending}>
                  {product ? 'Save Changes' : 'Create Product'}
                </Button>
              )}
            </div>
          </div>
        </form>

        {showDeleteDialog && (
          <DeleteDialog
            onClose={() => setShowDeleteDialog(false)}
            itemName={product?.name}
            loading={removeProduct.isPending}
            onConfirm={() => handleDelete(product!.id)}
          />
        )}
      </div>
    </Modal>
  );
}
