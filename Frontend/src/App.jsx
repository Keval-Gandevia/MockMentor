import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import LandingPage from "./pages/LandingPage";
import AddQuestion from "./pages/AddQuestion";
import RecordVideo from "./pages/RecordVideo";

function App() {
  return (
    <>
      <Router>
        <Navbar />
        <Routes>
          <Route path="/" element={<LandingPage />} />
          <Route path="/add-question" element={<AddQuestion />} />
          <Route path="/record-video" element={<RecordVideo />} />
        </Routes>
      </Router>
    </>
  );
}

export default App;
