type quizResult = {
  resultId: string;
  userId: string;
  sessionId: string;
  correctAnswers: number;
  totalQuestions: number;
  completedAt: string;
};

export type GetUsersResultsResponse = quizResult[];
