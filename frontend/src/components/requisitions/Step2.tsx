import { Controller, useFieldArray, type UseFormReturn } from 'react-hook-form';
import type {
  CreateRequisitionFormData,
  RequisitionLineFormData,
} from '../../schemas/requisition.schema';
import { ArrowLeft, Plus, Save, Trash2 } from 'lucide-react';
import ProductSearch from '../products/ProductSearch';
import Input from '../common/Input';
import Button from '../common/Button';

type Props = {
  form: UseFormReturn<CreateRequisitionFormData>;
  onBack: () => void;
  onSaveDraft: () => void;
  onSubmit: () => void;
  isSaving: boolean;
  isSubmitting: boolean;
};

export default function Step2({
  form,
  onBack,
  onSaveDraft,
  onSubmit,
  isSaving,
  isSubmitting,
}: Props) {
  const {
    control,
    register,
    formState: { errors },
    watch,
  } = form;

  const { fields, append, remove } = useFieldArray({
    control,
    name: 'lines',
  });

  const lines = watch('lines') as RequisitionLineFormData[];

  const total =
    lines?.reduce(
      (sum, l) =>
        sum + (l.quantityRequested || 0) * (l.estimatedUnitPrice || 0),
      0,
    ) ?? 0;

  const addLine = () => {
    append({
      productId: '',
      productName: '',
      quantityRequested: 1,
      estimatedUnitPrice: 0,
    });
  };

  return (
    <div className='space-y-4'>
      <div className='space-y-3'>
        {fields.map((field, index) => (
          <div
            key={field.id}
            className='bg-white border border-gray-200 rounded-xl p-4 space-y-3'
          >
            <div className='flex items-center justify-between'>
              <p className='text-sm font-medium text-gray-700'>
                Line {index + 1}
              </p>
              {fields.length > 1 && (
                <button
                  type='button'
                  onClick={() => remove(index)}
                  className='p-1 rounded text-gray-400 hover:text-red-500 hover:bg-red-50 transition'
                >
                  <Trash2 className='w-4 h-4' />
                </button>
              )}
            </div>

            <Controller
              name={`lines.${index}.productId`}
              control={control}
              render={({ field: f }) => (
                <ProductSearch
                  value={f.value}
                  onChange={(productId, product) => {
                    f.onChange(productId);
                    if (product) {
                      form.setValue(`lines.${index}.productName`, product.name);
                      form.setValue(
                        `lines.${index}.estimatedUnitPrice`,
                        product.unitPrice!,
                      );
                    }
                  }}
                  error={errors.lines?.[index]?.productId?.message}
                />
              )}
            />

            <div className='grid grid-cols-2 gap-3'>
              <Input
                label='Quantity'
                type='number'
                min={1}
                error={errors.lines?.[index]?.quantityRequested?.message}
                required
                {...register(`lines.${index}.quantityRequested`, {
                  valueAsNumber: true,
                })}
              />
              <Input
                label='Unit Price'
                type='number'
                step={0.01}
                error={errors.lines?.[index]?.estimatedUnitPrice?.message}
                required
                {...register(`lines.${index}.estimatedUnitPrice`, {
                  valueAsNumber: true,
                })}
              />
            </div>
          </div>
        ))}
      </div>
      <button
        type='button'
        onClick={addLine}
        className='w-full py-3 border-2 border-dashed border-gray-200 rounded-xl text-sm text-gray-400 hover:border-orange-300 hover:text-orange-500 transition flex items-center justify-center gap-2'
      >
        <Plus className='w-4 h-4' />
        Add Line
      </button>

      {errors?.lines?.root && (
        <p className='text-xs text-red-500'>{errors.lines.root.message}</p>
      )}

      <div className='flex justify-end'>
        <div className='bg-gray-50 rounded-xl px-5 py-3 text-right'>
          <p className='text-xs text-gray-500'>Estimated Total</p>
          <p className='text-xl font-bold text-gray-900 mt-0.5'>
            {total.toLocaleString(undefined, { minimumFractionDigits: 2 })}
          </p>
        </div>
      </div>

      <div className='flex items-center justify-between pt-2'>
        <Button
          variant='ghost'
          icon={<ArrowLeft className='w-4 h-4' />}
          onClick={onBack}
        >
          Back
        </Button>
        <div className='flex gap-2'>
          <Button
            variant='secondary'
            icon={<Save className='w-4 h-4' />}
            loading={isSaving}
            onClick={onSaveDraft}
          >
            Save as Draft
          </Button>
          <Button
            icon={<Save className='w-4 h-4' />}
            loading={isSubmitting}
            onClick={onSubmit}
          >
            Submit
          </Button>
        </div>
      </div>
    </div>
  );
}
