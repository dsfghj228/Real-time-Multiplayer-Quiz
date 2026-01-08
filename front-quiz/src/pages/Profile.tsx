import { useCallback, useEffect, useState } from "react";
import toast from "react-hot-toast";
import { Link, useNavigate } from "react-router-dom";
import { getUsersResults, logout } from "../api/api";
import ProfileBg from "../assets/images/Results.svg";
import { GetUsersResultsResponse } from "../types/quizResult.types";

function Profile() {
  const [quizzes, setQuizzes] = useState<GetUsersResultsResponse>([]);
  const navigate = useNavigate();

  const getQuizzes = useCallback(async () => {
    try {
      var data = await getUsersResults();
      console.log("data", data);
      setQuizzes(data);
    } catch (error: any) {
      toast.error(error || "Couldn't get users quizzes results");
    }
  }, []);

  useEffect(() => {
    getQuizzes();
  }, [getQuizzes]);

  const handleLogout = async () => {
    try {
      await logout();
      toast.success("Successful log out");
      navigate("/login");
    } catch (error: any) {
      toast.error(error || "Couldn't log out");
    }
  };

  return (
    <div
      className="flex items-center justify-center bg-center bg-no-repeat bg-cover h-screen w-screen"
      style={{ backgroundImage: `url(${ProfileBg})` }}
    >
      <div className="flex flex-col items-center justify-center w-full max-w-[500px] lg:max-w-[1000px] h-full lg:p-[50px] p-[20px]">
        <h1 className="font-semibold text-3xl sm:text-4xl mb-[30px]">
          {quizzes.length === 0
            ? "The results of your quizzes will be here"
            : "Your quizzes results:"}
        </h1>
        <div className="max-h-[400px] w-full max-w-[360px] overflow-y-auto mb-[40px] pr-2">
          <div className="grid grid-cols-3 gap-x-10 gap-y-4">
            {quizzes.map((q) => (
              <Link key={q.sessionId} to={`/quiz/${q.sessionId}/results`}>
                <div className="flex items-center justify-center w-[80px] h-[80px] backdrop-blur-md bg-white/30 border border-white/40 rounded-[10px]">
                  {q.correctAnswers} / {q.totalQuestions}
                </div>
              </Link>
            ))}
          </div>
        </div>
        <Link to="/quiz/start">
          <p className="font-semibold text-xl hover:underline mb-[30px]">
            {quizzes.length === 0
              ? "Wanna make your first quiz?"
              : "Wanna play again?"}
          </p>
        </Link>
        <button
          onClick={async () => await handleLogout()}
          className="w-[300px] sm:w-[360px] h-[55px] bg-[#2F3538] rounded-[5px] text-[#FFFFFF] text-sm font-bold mb-[40px]"
        >
          Log out
        </button>
      </div>
    </div>
  );
}

export default Profile;
