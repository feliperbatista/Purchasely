import { useNavigate, useParams } from 'react-router-dom';
import { useRequisitions } from '../../hooks/useRequisitions';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import {
  type CreateRequisitionFormData,
  createRequisitionSchema,
} from '../../schemas/requisition.schema';
import { zodResolver } from '@hookform/resolvers/zod';
import { toast } from 'sonner';
import { getErrorMessage } from '../../lib/errors';
import { ArrowLeft } from 'lucide-react';
import StepIndicator from '../../components/requisitions/StepIndicator';
import Step1 from '../../components/requisitions/Step1';
import Step2 from '../../components/requisitions/Step2';
import NotFoundPage from '../NotFoundPage';

export default function NewRequisitionPage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const {
    create,
    submit,
    requisition,
    isLoadingRequisition: isLoading,
    update,
  } = useRequisitions(id);
  const [step, setStep] = useState(1);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const form = useForm<CreateRequisitionFormData>({
    resolver: zodResolver(createRequisitionSchema),
    defaultValues: {
      priority: requisition?.priority ?? 'Normal',
      justification: requisition?.justification ?? '',
      lines: requisition?.lines ?? [
        {
          productId: '',
          productName: '',
          quantityRequested: 1,
          estimatedUnitPrice: 0,
        },
      ],
    },
  });

  const handleSaveDraft = form.handleSubmit(async (data) => {
    create.mutate(data, {
      onSuccess: (req) => {
        toast.success(`Requisition #${req.number} saved as draft`);
        navigate('/requisitions');
      },
      onError: (error) => toast.error(getErrorMessage(error)),
    });
  });

  const handleSubmit = form.handleSubmit(async (data) => {
    setIsSubmitting(true);
    create.mutate(data, {
      onSuccess: async (req) => {
        submit.mutate(req.id, {
          onSuccess: () => {
            toast.success(`Requisition #${req.number} submitted for approval`);
            navigate('/requisitions');
          },
          onError: (error) => {
            toast.error(getErrorMessage(error));
          },
        });
      },
      onError: (error) => {
        toast.error(getErrorMessage(error));
      },
    });
  });

  const handleEdit = form.handleSubmit(async (data) => {
    update.mutate(
      { id: id!, data },
      {
        onSuccess: () => {
          toast.success('Requisition successfully updated');
          navigate('/requisitions');
        },
        onError: (error) => toast.error(getErrorMessage(error)),
      },
    );
  });

  if (isLoading) return <div>loading</div>;

  if (id && !requisition) return <NotFoundPage inline/>;

  if (requisition && requisition?.status !== 'Draft')
    navigate(`/requisitions/${requisition?.id}`);

  return (
    <div className='space-y-5'>
      <div className='flex items-center gap-3'>
        <button
          onClick={() => navigate('/requisitions')}
          className='p-2 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition'
        >
          <ArrowLeft className='w-4 h-4' />
        </button>
        <div>
          <h1 className='text-lg font-semibold text-gray-900'>
            {requisition
              ? `Edit Requisition #${requisition.number}`
              : 'New Requisitions'}
          </h1>
          <p className='text-sm text-gray-500'>
            {`Fill in the details to ${requisition ? 'update' : 'create'} a purchase requisition`}
          </p>
        </div>
      </div>

      <div className='bg-white border border-gray-200 rounded-2xl p-6 max-w-2xl'>
        <StepIndicator current={step} />

        {step === 1 && <Step1 form={form} onNext={() => setStep(2)} />}

        {step === 2 && (
          <Step2
            form={form}
            onBack={() => setStep(1)}
            onSaveDraft={handleSaveDraft}
            onUpdate={handleEdit}
            onSubmit={handleSubmit}
            isSaving={create.isPending && !isSubmitting}
            isSavingUpdate={update.isPending}
            isSubmitting={isSubmitting}
            editting={!!requisition}
          />
        )}
      </div>
    </div>
  );
}
