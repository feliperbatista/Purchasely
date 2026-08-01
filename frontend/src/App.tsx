import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import ProtectedRoute from './components/common/ProtectedRoute';
import DashboardPage from './pages/DashboardPage';
import LoginPage from './pages/LoginPage';
import PurchaseOrderDetailPage from './pages/purchase-orders/PurchaseOrderDetailPage';
import NewRequisitionPage from './pages/requisitions/NewRequisitionPage';
import RequisitionDetailPage from './pages/requisitions/RequisitionDetailPage';
import { AppProviders } from './providers/AppProviders';
import RequisitionsPage from './pages/requisitions/RequisitionsPage';
import DashboardLayout from './components/layout/DashboardLayout';
import PurchaseOrdersPage from './pages/purchase-orders/PurchaseOrdersPage';
import ProductsPage from './pages/products/ProductsPage';
import SuppliersPage from './pages/suppliers/SuppliersPage';
import SupplierDetailsPage from './pages/suppliers/SupplierDetailsPage';
import NotFoundPage from './pages/NotFoundPage';
import UsersPage from './pages/users/UsersPage';
import AdminRoute from './components/common/AdminRoute';

export default function App() {
  return (
    <AppProviders>
      <BrowserRouter>
        <Routes>
          <Route path='/login' element={<LoginPage />} />

          <Route element={<ProtectedRoute />}>
            <Route element={<DashboardLayout />}>
              <Route path='/' element={<Navigate to='/dashboard' replace />} />
              <Route path='/dashboard' element={<DashboardPage />} />
              <Route path='/requisitions' element={<RequisitionsPage />} />
              <Route
                path='/requisitions/new'
                element={<NewRequisitionPage />}
              />
              <Route
                path='/requisitions/:id/edit'
                element={<NewRequisitionPage />}
              />
              <Route
                path='/requisitions/:id'
                element={<RequisitionDetailPage />}
              />
              <Route path='/purchase-orders' element={<PurchaseOrdersPage />} />
              <Route
                path='/purchase-orders/:id'
                element={<PurchaseOrderDetailPage />}
              />
              <Route path='/products' element={<ProductsPage />} />
              <Route path='/suppliers' element={<SuppliersPage />} />
              <Route path='/suppliers/new' element={<SupplierDetailsPage />} />
              <Route path='/suppliers/:id' element={<SupplierDetailsPage />} />

              <Route element={<AdminRoute />}>
                <Route path='/users' element={<UsersPage />} />
              </Route>
            </Route>
          </Route>

          <Route path='*' element={<NotFoundPage />} />
        </Routes>
      </BrowserRouter>
    </AppProviders>
  );
}
