import { ADD_QUESTION, SET_LOADING } from "./actions";

const reducer = (state, action) => {
  if (action.type === ADD_QUESTION) {
    return {
      ...state,
      questionId: action.data.questionId,
      questionText: action.data.questionText,
    };
  }
  if (action.type === SET_LOADING) {
    return { ...state, isLoading: action.payload };
  }
};

export default reducer;