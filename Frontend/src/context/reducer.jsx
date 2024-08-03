import { ADD_QUESTION, ADD_VIDEO, SET_EMOTION, SET_FEEDBACK, SET_LOADING } from "./actions";

const reducer = (state, action) => {
  if (action.type === ADD_QUESTION) {
    localStorage.setItem("questionId", action.data.questionId)
    localStorage.setItem("questionText", action.data.questionText)
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
    localStorage.setItem("videoId", action.data.videoId)
    return { ...state, videoId: action.data.videoId };
  }
  if (action.type === SET_FEEDBACK) {
    localStorage.setItem("feedbackText", action.data.feedbackText)
    return { ...state, feedbackText: action.data.feedbackText };
  }
  if (action.type === SET_EMOTION) {
    console.log(JSON.stringify(action.data.emotionValue))
    localStorage.setItem("emotionValue", JSON.stringify(action.data.emotionValue))
    return { ...state, emotionValue: (action.data.emotionValue) };
  }
};

export default reducer;