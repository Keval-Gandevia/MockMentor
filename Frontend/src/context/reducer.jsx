import {
  ADD_QUESTION,
  ADD_VIDEO,
  RESET_STATE,
  SET_EMOTION,
  SET_FEEDBACK,
  SET_LOADING,
} from "./actions";

const reducer = (state, action) => {
  if (action.type === ADD_QUESTION) {
    localStorage.setItem("questionId", action.data.questionId);
    localStorage.setItem("questionText", action.data.questionText);
    return {
      ...state,
      questionId: action.data.questionId,
      questionText: action.data.questionText,
    };
  }
  if (action.type === SET_LOADING) {
    return { ...state, isLoading: action.payload };
  }
  if (action.type === ADD_VIDEO) {
    localStorage.setItem("videoId", action.data.videoId);
    return { ...state, videoId: action.data.videoId };
  }
  if (action.type === SET_FEEDBACK) {
    localStorage.setItem("feedbackText", action.data.feedbackText);
    return { ...state, feedbackText: action.data.feedbackText };
  }
  if (action.type === SET_EMOTION) {
    localStorage.setItem(
      "emotionValue",
      JSON.stringify(action.data.emotionValue)
    );
    return { ...state, emotionValue: action.data.emotionValue };
  }
  if (action.type === RESET_STATE) {
    return {
      questionId: 0,
      questionText: "",
      isLoading: false,
      videoId: 0,
      feedbackText: "Result is pending",
      emotionValue: "Result is pending",
    };
  }
};

export default reducer;
