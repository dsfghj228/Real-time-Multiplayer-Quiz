import Cookies from "js-cookie";

export const isAuth = (): boolean => {
  return Boolean(Cookies.get("access_token"));
};
