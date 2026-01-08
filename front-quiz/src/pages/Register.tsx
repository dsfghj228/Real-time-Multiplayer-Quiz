import { useState } from "react";
import toast, { Toaster } from "react-hot-toast";
import { Link, useNavigate } from "react-router-dom";
import { register } from "../api/api";
import AuthBg from "../assets/images/AuthBg.svg";
import Eye from "../assets/images/Eye.svg";
import Eyeoff from "../assets/images/Eyeoff.svg";
import { validationRegisterErrors } from "../types/apiAuth.types";

function Register() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [errors, setErrors] = useState<validationRegisterErrors>({
    password: "",
    email: "",
    username: "",
  });
  const navigate = useNavigate();

  const validationErrors = (): boolean => {
    const newErrors: validationRegisterErrors = {
      password: "",
      email: "",
      username: "",
    };

    if (!username.trim()) {
      newErrors.username = "Username is required";
    } else if (!email) {
      newErrors.email = "Email is required";
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

    if (
      newErrors.password === "" &&
      newErrors.username === "" &&
      newErrors.email === ""
    ) {
      return true;
    }

    setErrors(newErrors);
    return false;
  };

  const handleRegister = async () => {
    if (!validationErrors()) return;

    setLoading(true);
    try {
      await register({ username, email, password });
      navigate("/login", { replace: true });
      toast.success("Successful registration");
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
            <p className="text-[#757575] text-lg">Welcome!</p>
            <h1 className="text-[#424242] text-3xl">
              Create your new account.
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
                Email
              </label>
              <input
                id="email"
                value={email}
                onChange={(e) => {
                  setEmail(e.target.value);
                  setErrors((prev) => ({ ...prev, email: "" }));
                }}
                disabled={loading}
                placeholder="Enter email"
                className="bg-[#F5F5F5] text-[#212121] text-lg font-normal focus:outline-none focus:ring-0 h-[30px] w-[300px]"
              />
            </div>
            <div className="h-[25px] w-[360px] flex items-center justify-center">
              {errors.email && (
                <p className="text-red-500 text-sm">{errors.email}</p>
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
            onClick={handleRegister}
            className="w-[360px] h-[55px] bg-[#2F3538] rounded-[5px] text-[#FFFFFF] text-sm font-bold mb-[40px]"
            disabled={loading}
          >
            Register
          </button>
          <div className="flex w-[360px] justify-center">
            <p className="mr-[5px] text-[#757575] text-lg">
              Already have an account?
            </p>
            <Link to="/login">
              <p className="text-[#212121] text-lg hover:underline">LOGIN</p>
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

export default Register;
