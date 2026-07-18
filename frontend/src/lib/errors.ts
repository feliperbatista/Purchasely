import axios from 'axios';

export const getErrorMessage = (error: unknown): string => {
  if (axios.isAxiosError(error)) {
    return (
      error.response?.data.detail ??
      error.response?.data?.errors?.[0] ??
      error.message ??
      'Something went wrong'
    );
  }

  return 'Something went wrong';
};
