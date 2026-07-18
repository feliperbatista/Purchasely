import { useNavigate, useParams } from 'react-router-dom';
import Input from '../../components/common/Input';
import { useSuppliers } from '../../hooks/useSuppliers';
import { useForm } from 'react-hook-form';
import {
  type SupplierFormData,
  supplierSchema,
} from '../../schemas/supplier.schema';
import { zodResolver } from '@hookform/resolvers/zod';
import Table from '../../components/common/Table';
import Button from '../../components/common/Button';
import { Plus, Save, Trash2 } from 'lucide-react';
import { useState } from 'react';
import DeleteDialog from '../../components/common/DeleteDialog';
import SupplierProductModal from '../../components/suppliers/SupplierProductModal';
import type { Product } from '../../types/product';
import { toast } from 'sonner';
import { getErrorMessage } from '../../lib/errors';

export default function SupplierDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const {
    supplier,
    isLoadingOne: isLoading,
    create,
    update,
    remove,
  } = useSuppliers(id);
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);
  const [showAddProduct, setShowAddProduct] = useState<{
    show: boolean;
    selected?: Product | undefined;
  }>({
    show: false,
    selected: undefined,
  });

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SupplierFormData>({
    resolver: zodResolver(supplierSchema),
    defaultValues: supplier ?? {},
  });

  const onSubmit = (data: SupplierFormData) => {
    if (supplier) {
      update.mutate(data, {
        onSuccess: () => {
          toast.success('Supplier successfully updated');
          navigate('/suppliers');
        },
      });
    } else {
      create.mutate(data, {
        onSuccess: (s) => {
          toast.success('Supplier successfully created');
          navigate(`/suppliers/${s.id}`);
        },
      });
    }
  };

  const handleDelete = (id: string) => {
    remove.mutate(id, {
      onSuccess: () => navigate('/suppliers'),
      onError: (error) => toast.error(getErrorMessage(error)),
    });
  };

  if (id && !supplier) return <h1>not found</h1>;

  return (
    <div className='space-y-5'>
      <h1 className='text-lg font-semibold text-gray-900'>
        {supplier ? supplier.name : 'New Supplier'}
      </h1>

      <div className='grid sm:grid-cols-1 lg:grid-cols-2 gap-3'>
        <form onSubmit={handleSubmit(onSubmit)} className='flex flex-col gap-4'>
          <div className='flex flex-col gap-2 px-2'>
            <Input
              label='Name'
              placeholder='South Fish Inc.'
              error={errors.name?.message}
              required
              {...register('name')}
            />
            <Input
              label='Email'
              placeholder='company@email.com'
              error={errors.email?.message}
              required
              {...register('email')}
            />
            <Input
              label='Phone'
              placeholder='5544991359877'
              error={errors.phone?.message}
              required
              {...register('phone')}
            />
            <Input
              label='Address'
              placeholder='Rua São Pedro, São Paulo, São Paulo, Brasil'
              error={errors.address?.message}
              required
              {...register('address')}
            />
            <Input
              label='Tax Number'
              placeholder='99999999000199'
              error={errors.taxNumber?.message}
              required
              {...register('taxNumber')}
            />
          </div>
          <div className='justify-between gap-4 flex'>
            {supplier && (
              <Button
                variant='danger'
                type='button'
                icon={<Trash2 className='w-4 h-4' />}
                onClick={() => setShowDeleteDialog(true)}
              >
                Delete
              </Button>
            )}
            <Button type='submit' icon={<Save className='w-4 h-4' />}>
              Save
            </Button>
          </div>
        </form>
        <div className='flex flex-col gap-1.5'>
          <div className='flex items-center justify-between'>
            <div className='flex'>
              <h3 className='font-medium text-sm text-gray-900'>Products</h3>
              <p className='text-sm ml-2'>
                {supplier?.products.length ?? 0} products total
              </p>
            </div>
            <div className='flex items-center gap-2'>
              {!supplier && (
                <p className='text-xs text-gray-400'>Save the supplier first</p>
              )}
              <Button
                type='button'
                icon={<Plus className='w-4 h-4' />}
                onClick={() => setShowAddProduct({ show: true })}
                size='sm'
                disabled={!supplier}
              >
                Add
              </Button>
            </div>
          </div>

          <div className='bg-white rounded-xl border border-gray-100 overflow-hidden p-5'>
            <Table
              data={supplier?.products}
              loading={isLoading}
              emptyMessage='No products yet'
              getRowKey={(product) => product.id}
              onRowClick={(product) =>
                setShowAddProduct({ show: true, selected: product })
              }
              columns={[
                { header: 'Name', render: (product) => product.name },
                {
                  header: 'Price',
                  render: (product) => product.unitPrice?.toLocaleString(),
                },
              ]}
            />
          </div>
        </div>
      </div>

      {showDeleteDialog && (
        <DeleteDialog
          onClose={() => setShowDeleteDialog(false)}
          itemName={supplier?.name}
          loading={remove.isPending}
          onConfirm={() => handleDelete(id!)}
        />
      )}

      {showAddProduct.show && (
        <SupplierProductModal
          onClose={() => setShowAddProduct({ show: false })}
          supplierId={id!}
          product={showAddProduct.selected}
        />
      )}
    </div>
  );
}
