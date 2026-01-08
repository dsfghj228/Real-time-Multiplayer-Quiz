import { useEffect, useState } from "react";
import toast, { Toaster } from "react-hot-toast";
import { Link, useNavigate } from "react-router-dom";
import { getCategories, startQuiz } from "../api/api";
import StartBg from "../assets/images/StartQuizBg.svg";
import { startQuizRequest } from "../types/quiz.types";

function Start() {
  const [categories, setCategories] = useState<Array<string>>([]);
  const [selectedCategory, setSelectedCategory] = useState<string>("");
  const [selectedDifficulty, setSelectedDifficulty] = useState<number>(1);
  const [amount, setAmount] = useState<number>(1);
  const navigate = useNavigate();

  enum Difficulty {
    Easy = 0,
    Medium = 1,
    Hard = 2,
  }

  useEffect(() => {
    const handleGetCategories = async () => {
      try {
        var data = await getCategories();
        setCategories(data);
      } catch (error: any) {
        toast(error || "Something went wrong");
      }
    };

    handleGetCategories();
  }, []);

  const startGame = async () => {
    try {
      var data: startQuizRequest = {
        category: selectedCategory,
        difficulty: selectedDifficulty,
        numberOfQuestions: amount,
      };

      var res = await startQuiz(data);
      navigate(`/quiz/${res.sessionId}`, { replace: true });
    } catch (error: any) {
      toast(error || "Unsuccessful attempt to start the game");
    }
  };

  return (
    <div
      className="flex items-center justify-center bg-center bg-no-repeat bg-cover h-screen w-screen"
      style={{ backgroundImage: `url(${StartBg})` }}
    >
      <Toaster position="top-center" />
      <div className="backdrop-blur-md bg-white/30 border border-white/40 rounded-xl shadow-lg flex flex-col items-center justify-center w-full max-w-[700px] lg:max-w-[1000px] h-full max-h-[750px] px-[10px]">
        <div className="flex items-center justify-end w-full pr-[50px]">
          <Link to="/profile">
            <p className="text-[#FFFFFF] text-lg hover:underline">
              Go to profile
            </p>
          </Link>
        </div>
        <div className="flex flex-col items-center mb-[30px]">
          <h1 className="text-[#FFFFFF] text-3xl">Configure your quiz.</h1>
        </div>
        <p className="text-[#FFFFFF] text-lg pb-[20px]">Choose a topic:</p>
        <div
          className="
  grid
  grid-cols-3
  sm:grid-cols-4
  lg:grid-cols-6
  gap-x-2 gap-y-2
  lg:gap-x-5 md:gap-y-4
  max-w-[700px]
  pb-[30px]
"
        >
          {categories.map((c, index) => {
            return (
              <button
                key={c}
                onClick={() => setSelectedCategory(c)}
                className={`
                  h-[40px]
      lg:w-[100px] lg:h-[80px] rounded-full
      flex items-center justify-center
      text-sm font-semibold text-white
      border-2 transition-all duration-200

      ${
        selectedCategory === c
          ? "bg-[#984EFF] border-none shadow-lg"
          : "backdrop-blur-md bg-white/30 border border-white/40 hover:scale-105"
      }

      lg:[&:nth-child(12n+7)]:-translate-x-6
      lg:[&:nth-child(12n+8)]:-translate-x-6
      lg:[&:nth-child(12n+9)]:-translate-x-6
      lg:[&:nth-child(12n+10)]:-translate-x-6
      lg:[&:nth-child(12n+11)]:-translate-x-6
      lg:[&:nth-child(12n+12)]:-translate-x-6
    `}
              >
                {c}
              </button>
            );
          })}
        </div>
        <div className="w-full flex flex-col items-center pb-[30px]">
          <p className="text-[#FFFFFF] text-lg pb-[20px]">
            Choose a difficulty:
          </p>
          <div className="flex w-[240px] justify-between">
            <button
              className={`
        w-[70px] h-[40px] rounded-[10px]
        flex items-center justify-center
        text-sm font-semibold  text-[#FFFFFF]
        border-2 transition-all duration-200
        ${
          selectedDifficulty === Difficulty.Easy
            ? "bg-[#984EFF] border-none shadow-lg"
            : "backdrop-blur-md bg-white/30 border border-white/40 hover:scale-105"
        }
      `}
              onClick={() => setSelectedDifficulty(Difficulty.Easy)}
            >
              Easy
            </button>
            <button
              className={`
        w-[70px] h-[40px] rounded-[10px]
        flex items-center justify-center
        text-sm font-semibold  text-[#FFFFFF]
        border-2 transition-all duration-200
        ${
          selectedDifficulty === Difficulty.Medium
            ? "bg-[#984EFF] border-none shadow-lg"
            : "backdrop-blur-md bg-white/30 border border-white/40 hover:scale-105"
        }
      `}
              onClick={() => setSelectedDifficulty(Difficulty.Medium)}
            >
              Medium
            </button>
            <button
              className={`
        w-[70px] h-[40px] rounded-[10px]
        flex items-center justify-center
        text-sm font-semibold  text-[#FFFFFF]
        border-2 transition-all duration-200
        ${
          selectedDifficulty === Difficulty.Hard
            ? "bg-[#984EFF] border-none shadow-lg"
            : "backdrop-blur-md bg-white/30 border border-white/40 hover:scale-105"
        }
      `}
              onClick={() => setSelectedDifficulty(Difficulty.Hard)}
            >
              Hard
            </button>
          </div>
        </div>

        <div className="flex flex-col items-center pb-[20px]">
          <p className="text-[#FFFFFF] text-lg pb-[10px]">
            Select the number of questions:
          </p>
          <input
            type="number"
            min={1}
            max={15}
            value={amount}
            onChange={(e) => setAmount(Number(e.target.value))}
            className="w-[40px] h-[40px] rounded-[10px]
        text-center leading-[40px]
        text-2xl font-semibold  text-[#FFFFFF]
        border-2 transition-all duration-200
        backdrop-blur-md bg-white/30 border-white/40"
          />
        </div>

        <button
          className="bg-[#984EFF] rounded-[10px] text-[#FFFFFF] w-[150px] h-[50px] text-lg font-bold"
          onClick={startGame}
        >
          Start
        </button>
      </div>
    </div>
  );
}

export default Start;
