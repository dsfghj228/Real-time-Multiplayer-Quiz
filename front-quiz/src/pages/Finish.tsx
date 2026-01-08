import { useCallback, useEffect, useState } from "react";
import toast, { Toaster } from "react-hot-toast";
import { Link, useParams } from "react-router-dom";
import { getQuizResultsBySessionId } from "../api/api";
import ResultsBg from "../assets/images/ResultBg.svg";
import { finishQuizResponse } from "../types/quiz.types";

function Finish() {
  const [finishData, setFinishData] = useState<finishQuizResponse>();
  const { sessionId } = useParams();

  const finish = useCallback(async () => {
    if (!sessionId) {
      toast.error("Couldn't get session id");
      return;
    }

    try {
      var res = await getQuizResultsBySessionId(sessionId);
      setFinishData(res);
    } catch (error: any) {
      toast.error(error || "Couldn`t finish quiz");
    }
  }, [sessionId]);

  useEffect(() => {
    finish();
  }, [finish]);

  return (
    <div
      className="flex items-center justify-center bg-center bg-no-repeat bg-cover h-screen w-screen"
      style={{ backgroundImage: `url(${ResultsBg})` }}
    >
      <Toaster position="top-center" />
      <div className="flex flex-col items-center text-white ">
        <h1 className="font-bold text-3xl sm:text-5xl mb-[50px]">
          Bravo! You have scored:
        </h1>
        <p className="font-bold text-9xl mb-[50px]">
          {finishData?.correctAnswers} / {finishData?.totalQuestions}
        </p>
        <Link to="/quiz/start">
          <p className="font-semibold text-3xl sm:text-5xl hover:underline mb-[30px]">
            Wanna play again?
          </p>
        </Link>
        <Link to="/profile">
          <p className="font-semibold text-xl sm:text-2xl hover:underline">
            Go to profile.
          </p>
        </Link>
      </div>
    </div>
  );
}

export default Finish;
