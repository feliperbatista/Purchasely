import type { UserRole } from '../../types/users';

const roleStyles: Record<UserRole, string> = {
  Admin: 'bg-purple-50 text-purple-700 border border-purple-200',
  Buyer: 'bg-blue-50 text-blue-700 border border-blue-200',
  Manager: 'bg-orange-50 text-orange-700 border border-orange-200',
  Requester: 'bg-gray-100 text-gray-600',
};

export default function RoleBadge({ role }: { role: UserRole }) {
  return (
    <span
      className={`px-2 py-0.5 rounded-full text-xs font-medium ${roleStyles[role]}`}
    >
      {role}
    </span>
  );
}
