import { useState } from 'react';
import { useUsers } from '../../hooks/useUsers';
import Button from '../../components/common/Button';
import { Plus } from 'lucide-react';
import Table from '../../components/common/Table';
import RoleBadge from '../../components/users/RoleBadge';
import type { User } from '../../types/users';
import Pagination from '../../components/common/Pagination';
import CreateUserModal from '../../components/users/CreateUserModal';
import ChangeRoleModal from '../../components/users/ChangeRoleModal';

export default function UsersPage() {
  const {
    users,
    totalCount,
    totalPages,
    currentPage,
    hasNext,
    hasPrevious,
    isLoading,
    page,
    setPage
  } = useUsers();

  const [showCreateModal, setShowCreateModal] = useState(false);
  const [changeRoleTarget, setChangeRoleTarget] = useState<User | null>(null);

  return (
    <div className='space-y-5'>
      <div className='flex items-center justify-between'>
        <div>
          <h1 className='text-lg font-semibold text-gray-900'>Users</h1>
          <p className='text-sm text-gray-500 mt-0.5'>
            {totalCount} users total
          </p>
        </div>
        <Button
          icon={
            <Plus
              className='w-4 h-4'
              onClick={() => setShowCreateModal(true)}
            />
          }
        >
          New User
        </Button>
      </div>
      <div className='bg-white border border-gray-200 rounded-xl overflow-hidden'>
        <Table
          data={users}
          loading={isLoading}
          emptyMessage='No users yet'
          getRowKey={(user) => user.id}
          onRowClick={(user) => setChangeRoleTarget(user)}
          columns={[
            {
              header: 'Name',
              render: (user) => <p>{user.name}</p>,
            },
            {
              header: 'Email',
              render: (user) => <p>{user.email}</p>,
            },
            {
              header: 'Created',
              render: (user) => <p>{new Date(user.createdAt).toLocaleDateString()}</p>,
            },
            {
              header: 'Role',
              render: (user) => <RoleBadge role={user.role} />,
            },
          ]}
        />
        {totalPages > 1 && (
          <Pagination
            currentPage={currentPage}
            totalPages={totalPages}
            hasNext={hasNext}
            hasPrevious={hasPrevious}
            onPreviousClick={() => setPage(page - 1)}
            onNextClick={() => setPage(page + 1)}
          />
        )}
      </div>

      {showCreateModal && (
        <CreateUserModal onClose={() => setShowCreateModal(false)} />
      )}

      {changeRoleTarget && (
        <ChangeRoleModal
          user={changeRoleTarget}
          onClose={() => setChangeRoleTarget(null)}
        />
      )}
    </div>
  );
}
