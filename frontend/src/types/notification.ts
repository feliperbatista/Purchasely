export interface Notification {
  id: string;
  title: string;
  message: string;
  type: 'success' | 'warning' | 'info' | 'error';
  entityId?: string;
  entityType?: 'Requisition' | 'PurchaseOrder';
  createdAt: string;
  read: boolean;
}
