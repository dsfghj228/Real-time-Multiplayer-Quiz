import { Navigate, Outlet } from "react-router-dom";
import { isAuth } from "../auth/auth";

function AuthProtectedRoute() {
  if (isAuth()) return <Navigate to="/profile" replace />;

  return <Outlet />;
}

export default AuthProtectedRoute;
