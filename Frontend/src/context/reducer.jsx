import { ADD_QUESTION, ADD_VIDEO, SET_LOADING } from "./actions";

const reducer = (state, action) => {
  if (action.type === ADD_QUESTION) {
    return {
      ...state,
      questionId: action.data.questionId,
      questionText: action.data.questionText,
    };
  }
  // if (action.type === ADD_VIDEO) {
  //   return {
  //     ...state,
  //     questionId: action.data.questionId,
  //     video: action.data.questionText,
  //   };
  // }
  if (action.type === SET_LOADING) {
    return { ...state, loading: true };
  }
};

export default reducer;