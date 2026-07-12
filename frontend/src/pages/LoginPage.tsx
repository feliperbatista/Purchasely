import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useAuth } from '../hooks/useAuth';
import Input from '../components/common/Input';
import Button from '../components/common/Button';

const schema = z.object({
  email: z.email('Invalid email'),
  password: z.string().min(1, 'Password is required'),
});

type FormData = z.infer<typeof schema>;

export default function LoginPage() {
  const { login } = useAuth();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
  });

  const onSubmit = (data: FormData) => login.mutate(data);

  return (
    <div className='min-h-screen bg-gray-50 flex items-center justify-center px-4'>
      <div className='w-full max-w-md'>
        <div className='mb-8 text-center'>
          <h1 className='text-3xl font-bold text-gray-900'>Purchasely</h1>
          <p className='text-gray-500 mt-2 text-sm'>Sign in to your account</p>
        </div>

        <div className='bg-white rounded-2xl shadow-sm border border-gray-100 p-8'>
          <form onSubmit={handleSubmit(onSubmit)} className='space-y-5'>
            <Input
              label='Email'
              type='email'
              autoComplete='email'
              placeholder='you@company.com'
              error={errors.email?.message}
              required
              {...register('email')}
            />
            <Input
              label='Password'
              type='password'
              autoComplete='current-password'
              placeholder='••••••••'
              error={errors.password?.message}
              required
              {...register('password')}
            />
            <Button
              type='submit'
              loading={login.isPending}
              className='w-full mt-2'
            >
              Sign in
            </Button>
          </form>

          {login.isError && (
            <div className='mt-6 flex items-center gap-2 px-4 py-3 bg-red-50 border border-red-200 rounded-lg'>
              <svg
                className='w-4 h-4 text-red-500 shrink-0'
                fill='currentColor'
                viewBox='0 0 20 20'
              >
                <path
                  fillRule='evenodd'
                  d='M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z'
                  clipRule='evenodd'
                />
              </svg>
              <p className='text-sm text-red-600'>Invalid email or password</p>
            </div>
          )}
        </div>

        <p className='text-center text-xs text-gray-400 mt-6'>
          Purchasely © {new Date().getFullYear()}
        </p>
      </div>
    </div>
  );
}
