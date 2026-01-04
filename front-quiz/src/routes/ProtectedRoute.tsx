import { Navigate, Outlet } from "react-router-dom";
import { isAuth } from "../auth/auth";

function ProtectedRoute() {
  if (!isAuth()) return <Navigate to="/login" replace />;

  return <Outlet />;
}

export default ProtectedRoute;
