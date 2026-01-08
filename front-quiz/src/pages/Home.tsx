import { Link } from "react-router-dom";
import Arrow from "../assets/images/Arrow.svg";
import HomeSvg from "../assets/images/Home.svg";

function Home() {
  return (
    <div
      className="flex items-center justify-center bg-center bg-no-repeat bg-cover h-screen w-screen"
      style={{ backgroundImage: `url(${HomeSvg})` }}
    >
      <div className="flex flex-col items-center">
        <h1 className="font-semibold text-7xl md:text-9xl">Quizzler</h1>
        <Link to="/quiz/start">
          <div className="flex pt-5">
            <p className="text-xl md:text-2xl hover:underline">
              Let’s start the quiz
            </p>
            <img
              className="h-[36px] w-[42px] pt-1 md:pt-2"
              src={Arrow}
              alt="Arrow"
            />
          </div>
        </Link>
      </div>
    </div>
  );
}

export default Home;
