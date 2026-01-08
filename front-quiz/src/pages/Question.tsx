import { useCallback, useEffect, useState } from "react";
import toast, { Toaster } from "react-hot-toast";
import { useNavigate, useParams } from "react-router-dom";
import { finishQuiz, getCurrentQuestion, makeMove } from "../api/api";
import QuestionBg from "../assets/images/QuestionBg.svg";
import { quizResponse } from "../types/quiz.types";

function Question() {
  const { sessionId } = useParams();
  const [question, setQuestion] = useState<quizResponse>();
  const [selectedOptionId, setSelectedOptionId] = useState<string>();
  const navigate = useNavigate();

  const fetchQuestion = useCallback(async () => {
    if (!sessionId) {
      toast.error("Couldn't get session id");
      return;
    }

    try {
      const data = await getCurrentQuestion(sessionId);
      setQuestion(data);
      setSelectedOptionId(undefined);
    } catch (error: any) {
      toast.error(error || "Something went wrong");
    }
  }, [sessionId]);

  useEffect(() => {
    fetchQuestion();
  }, [fetchQuestion]);

  const move = async () => {
    if (!selectedOptionId) {
      toast.error("Please, select an option");
      return;
    }

    if (!sessionId) {
      toast.error("Couldn't get session id");
      return;
    }

    try {
      await makeMove(sessionId, selectedOptionId);
      if (question?.questionNumber === question?.totalQuestions) {
        await finishQuiz(sessionId);
        navigate(`/quiz/${sessionId}/results`, { replace: true });
      } else {
        await fetchQuestion();
      }
    } catch (error: any) {
      toast.error(error || "Couldn`t make move");
    }
  };

  return (
    <div
      className="flex items-center justify-center bg-center bg-no-repeat bg-cover h-screen w-screen"
      style={{ backgroundImage: `url(${QuestionBg})` }}
    >
      <Toaster position="top-center" />
      <div className="flex flex-col items-center w-full max-w-[700px] lg:max-w-[1000px] h-full max-h-[750px] px-[20px] pt-[10px]">
        <div className="flex flex-col items-center font-semibold text-3xl pb-[40px]">
          <h1 className="pb-[40px]">
            Question {question?.questionNumber}/{question?.totalQuestions}
          </h1>
          <p className="pb-[40px]">{question?.question.text}</p>
          <div className="w-[200px] border-b-[2px] border-black" />
        </div>
        <div className="flex flex-col items-center">
          {question?.question.options.map((q, index) => {
            var questInd = index + 1;
            return (
              <button
                className={`w-[300px] h-[40px] md:w-[500px] md:h-[60px] lg:w-[700px] lg:h-[80px] bg-white mb-[20px] flex items-center py-[20px] px-[50px] rounded-[10px]
                    ${
                      selectedOptionId !== q.id
                        ? "bg-white] text-black"
                        : "bg-[#FE909D] text-white"
                    }`}
                onClick={() => setSelectedOptionId(q.id)}
                key={q.id}
              >
                <div
                  className={`flex items-center justify-center w-[30px] h-[30px] lg:w-[50px] lg:h-[50px] rounded-full bg-[#FFECDB] text-[#FF475D] font-semibold text-3xl mr-[50px]
                    ${
                      selectedOptionId !== q.id
                        ? "bg-[#FFECDB] text-[#FF475D]"
                        : "bg-[#FF475D] text-[#FFECDB]"
                    }`}
                >
                  {questInd}
                </div>

                <p className="font-medium text-xl">{q.text}</p>
              </button>
            );
          })}
        </div>
        <button
          className="w-[150px] h-[75px] rounded-[10px] mt-[30px] bg-[#FFB0BA] text-[#FF475D] font-semibold text-3xl"
          onClick={async () => {
            await move();
          }}
        >
          {question?.questionNumber === question?.totalQuestions
            ? "Finish"
            : "Move"}
        </button>
      </div>
    </div>
  );
}

export default Question;
