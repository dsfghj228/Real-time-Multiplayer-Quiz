import Cookies from "js-cookie";
import {
  loginRequest,
  loginResponse,
  refreshResponse,
  registerRequest,
} from "../types/apiAuth.types";
import {
  finishQuizResponse,
  quizResponse,
  startQuizRequest,
} from "../types/quiz.types";
import { GetUsersResultsResponse } from "../types/quizResult.types";
import { api } from "./axios";

//////////////////////////AUTHENTICATION///////////////////////////////

export const login = async (data: loginRequest): Promise<loginResponse> => {
  try {
    const response = await api.post<loginResponse>("/account/login", data);
    const fifteenMinutes = 15 / (60 * 24);
    Cookies.set("access_token", response.data.token, {
      expires: fifteenMinutes,
    });
    return response.data;
  } catch (error: any) {
    throw error.response?.data?.message;
  }
};

export const register = async (data: registerRequest): Promise<void> => {
  try {
    await api.post<void>("/account/register", data);
  } catch (error: any) {
    throw error.response?.data?.message;
  }
};

export const refresh = async (): Promise<refreshResponse> => {
  try {
    var response = await api.post<refreshResponse>("/account/refresh");
    return response.data;
  } catch (error: any) {
    throw error;
  }
};

export const logout = async (): Promise<void> => {
  try {
    await api.post<void>("/account/logout");
    Cookies.remove("access_token");
  } catch (error: any) {
    throw error;
  }
};

//////////////////////////QUIZ//////////////////////////////////////

export const getCategories = async (): Promise<Array<string>> => {
  try {
    var response = await api.get<Array<string>>("/quiz/categories");

    return response.data;
  } catch (error: any) {
    throw error.response?.data?.message;
  }
};

export const startQuiz = async (
  data: startQuizRequest
): Promise<quizResponse> => {
  try {
    var response = await api.post<quizResponse>("/quiz/start", data);

    return response.data;
  } catch (error: any) {
    throw error.response?.data?.message;
  }
};

export const getCurrentQuestion = async (
  sessionId: string
): Promise<quizResponse> => {
  try {
    var response = await api.get<quizResponse>(`/quiz/${sessionId}/current`);

    return response.data;
  } catch (error: any) {
    throw error.response?.data?.message;
  }
};

export const makeMove = async (
  sessionId: string,
  optionId: string
): Promise<quizResponse> => {
  try {
    var response = await api.post<quizResponse>(
      `/quiz/${sessionId}/answer`,
      JSON.stringify(optionId),
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    return response.data;
  } catch (error: any) {
    throw error.response?.data?.message;
  }
};

export const finishQuiz = async (
  sessionId: string
): Promise<finishQuizResponse> => {
  try {
    var response = await api.post<finishQuizResponse>(
      `/quiz/${sessionId}/finish`
    );

    return response.data;
  } catch (error: any) {
    throw error.response?.data?.message;
  }
};

export const getQuizResultsBySessionId = async (
  sessionId: string
): Promise<finishQuizResponse> => {
  try {
    var response = await api.get<finishQuizResponse>(
      `/quiz/${sessionId}/result`
    );

    return response.data;
  } catch (error: any) {
    throw error.response?.data?.message;
  }
};

//////////////////////////QUIZ-RESULT///////////////////////////////

export const getUsersResults = async (): Promise<GetUsersResultsResponse> => {
  try {
    var response = await api.get<GetUsersResultsResponse>("/quiz-results");

    return response.data;
  } catch (error: any) {
    throw error;
  }
};
