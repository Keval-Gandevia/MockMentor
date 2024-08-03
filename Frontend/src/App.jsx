import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import LandingPage from "./pages/LandingPage";
import AddQuestion from "./pages/AddQuestion";
import RecordVideo from "./pages/RecordVideo";
import ViewFeedback from "./pages/ViewFeedback";

function App() {
  return (
    <>
      <Router>
        <Navbar />
        <Routes>
          <Route path="/" element={<LandingPage />} />
          <Route path="/add-question" element={<AddQuestion />} />
          <Route path="/record-video" element={<RecordVideo />} />
          <Route path="/view-feedback" element={<ViewFeedback />} />
        </Routes>
      </Router>
    </>
  );
}

export default App;
