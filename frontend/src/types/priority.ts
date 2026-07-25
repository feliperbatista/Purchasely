export const priorities = {
  Low: {
    style: 'text-gray-400',
  },
  Normal: {
    style: 'text-yellow-900',
  },
  High: {
    style: 'text-red-600',
  },
} as const;

export type Priority = keyof typeof priorities;
