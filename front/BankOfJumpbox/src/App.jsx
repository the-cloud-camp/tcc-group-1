import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./App.css";

import ProtectedRoutes from "./utils/ProtectedRoutes";
import Home from "./pages/Home/Home";
import Login from "./pages/Login/Login";
import PawnRegister from "./pages/pawnRegister/PawnRegister";
import MainPage from "./pages/main-page/MainPage";


function App() {
  return (
    <>
        <Router>
          <Routes>
            <Route element={<ProtectedRoutes/>}>
              <Route element={<Home/>} path="/" exact />
              <Route element={<PawnRegister/>} path="/pawn-register" exact />
            </Route>
            <Route element={<Login />} path="/login" />
            <Route element={<MainPage/>} path="/main" />
          </Routes>
        </Router>
    </>
  );
}

export default App;
