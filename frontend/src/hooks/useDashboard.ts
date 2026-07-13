import { useQuery } from '@tanstack/react-query';
import { dashboardApi } from '../api/dashboard';

export function useDashboard() {
  const { data: stats, isLoading: statsLoading } = useQuery({
    queryKey: ['dashboard', 'stats'],
    queryFn: dashboardApi.getStats,
  });

  const { data: requisitions, isLoading: reqLoading } = useQuery({
    queryKey: ['dashboard', 'requisitions'],
    queryFn: dashboardApi.getRecentRequisitions,
  });

  const { data: purchaseOrders, isLoading: poLoading } = useQuery({
    queryKey: ['dashboard', 'purchaseOrders'],
    queryFn: dashboardApi.getRecentPurchaseOrders,
  });

  return {
    stats,
    statsLoading,
    requisitions,
    reqLoading,
    purchaseOrders,
    poLoading,
  };
}
