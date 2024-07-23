import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import LandingPage from "./pages/LandingPage";
import AddQuestion from "./pages/AddQuestion";

function App() {
  return (
    <>
      <Router>
        <Navbar />
        <Routes>
          <Route path="/" element={<LandingPage />} />
          <Route path="/add-question" element={<AddQuestion />} />
        </Routes>
      </Router>
    </>
  );
}

export default App;
