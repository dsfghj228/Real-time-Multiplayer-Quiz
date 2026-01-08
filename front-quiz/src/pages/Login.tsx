import { useState } from "react";
import toast, { Toaster } from "react-hot-toast";
import { Link, useNavigate } from "react-router-dom";
import { login } from "../api/api";
import AuthBg from "../assets/images/AuthBg.svg";
import Eye from "../assets/images/Eye.svg";
import Eyeoff from "../assets/images/Eyeoff.svg";
import { validationLoginErrors } from "../types/apiAuth.types";

function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [errors, setErrors] = useState<validationLoginErrors>({
    password: "",
    username: "",
  });
  const navigate = useNavigate();

  const validationErrors = (): boolean => {
    const newErrors: validationLoginErrors = { password: "", username: "" };

    if (!username.trim()) {
      newErrors.username = "Username is required";
    } else if (!password) {
      newErrors.password = "Password is required";
    } else if (password.length < 8) {
      newErrors.password = "Password must be at least 8 characters long";
    } else if (!/[A-Z]/.test(password)) {
      newErrors.password =
        "Password must contain at least one uppercase letter";
    } else if (!/[a-z]/.test(password)) {
      newErrors.password =
        "Password must contain at least one lowercase letter";
      return false;
    } else if (!/[0-9]/.test(password)) {
      newErrors.password = "Password must contain at least one digit";
    } else if (!/[^a-zA-Z0-9]/.test(password)) {
      newErrors.password =
        "Password must contain at least one non-alphanumeric character";
    }

    if (newErrors.password === "" && newErrors.username === "") {
      return true;
    }

    setErrors(newErrors);
    return false;
  };

  const handleLogin = async () => {
    if (!validationErrors()) return;

    setLoading(true);
    try {
      await login({ username, password });
      toast.success("Successful log in");
      setTimeout(() => {
        navigate("/profile", { replace: true });
      }, 1000);
    } catch (err: any) {
      toast.error(err || "Login failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-[#E5E5E5] flex items-center justify-center bg-center bg-no-repeat bg-cover h-screen w-screen">
      <Toaster position="top-center" />
      <div className="flex items-center justify-center lg:justify-between w-full max-w-[500px] lg:max-w-[1000px] h-full max-h-[750px] bg-white sm:rounded-[20px] lg:p-[50px] p-[20px]">
        <div>
          <div className="flex flex-col items-center mb-[47px]">
            <p className="text-[#757575] text-lg">Welcome back!</p>
            <h1 className="text-[#424242] text-3xl">
              Continue to your account.
            </h1>
          </div>
          <div>
            <div className="flex flex-col bg-[#F5F5F5] h-[67px] w-[360px] py-[10px] px-[15px] rounded-[5px]">
              <label className="text-[#616161] text-sm font-medium pb-[5px]">
                Username
              </label>
              <input
                id="username"
                value={username}
                onChange={(e) => {
                  setUsername(e.target.value);
                  setErrors((prev) => ({ ...prev, username: "" }));
                }}
                disabled={loading}
                placeholder="Enter username"
                className="bg-[#F5F5F5] text-[#212121] text-lg font-normal focus:outline-none focus:ring-0 h-[30px] w-[200px]"
              />
            </div>
            <div className="h-[25px] w-[360px] flex items-center justify-center">
              {errors.username && (
                <p className="text-red-500 text-sm">{errors.username}</p>
              )}
            </div>
            <div className="flex flex-col bg-[#F5F5F5] h-[67px] w-[360px] py-[10px] px-[15px] rounded-[5px]">
              <label className="text-[#616161] text-sm font-medium pb-[5px]">
                Password
              </label>
              <div className="flex items-center justify-between">
                <input
                  id="password"
                  type={showPassword ? "text" : "password"}
                  value={password}
                  onChange={(e) => {
                    setPassword(e.target.value);
                    setErrors((prev) => ({ ...prev, password: "" }));
                  }}
                  disabled={loading}
                  placeholder="Enter password"
                  className="bg-[#F5F5F5] text-[#212121] text-lg font-normal focus:outline-none focus:ring-0 h-[30px] w-[200px]"
                />
                <button
                  onClick={() => setShowPassword(!showPassword)}
                  className="mr-[10px]"
                  disabled={loading}
                >
                  {showPassword ? (
                    <img
                      className="h-[16px] w-[16px]"
                      src={Eyeoff}
                      alt="eyeoff"
                    />
                  ) : (
                    <img className="h-[16px] w-[16px]" src={Eye} alt="eye" />
                  )}
                </button>
              </div>
            </div>
            <div className="h-[50px] w-[360px] flex items-center justify-center">
              {errors.password && (
                <p className="text-red-500 text-sm">{errors.password}</p>
              )}
            </div>
          </div>
          <button
            onClick={async () => await handleLogin()}
            className="w-[360px] h-[55px] bg-[#2F3538] rounded-[5px] text-[#FFFFFF] text-sm font-bold mb-[40px]"
            disabled={loading}
          >
            Login
          </button>
          <div className="flex w-[360px] justify-center">
            <p className="mr-[5px] text-[#757575] text-lg">Not register yet?</p>
            <Link to="/register">
              <p className="text-[#212121] text-lg hover:underline">REGISTER</p>
            </Link>
          </div>
        </div>
        <img
          className="h-[700px] w-[579px] pt-2 hidden lg:block ml-[20px]"
          src={AuthBg}
          alt="bg"
        />
      </div>
    </div>
  );
}

export default Login;
