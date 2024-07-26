import axios from "axios";
import { createContext, useContext, useReducer } from "react";
import reducer from "./reducer";
import { ADD_QUESTION, SET_LOADING } from "./actions";

// set-up the basic API call with axios.
const api = axios.create({
  baseURL: `https://localhost:44337/api`,
});

const URL = {
  addQuestionURL: `/Questions/addQuestion`,
};

const initialState = {
  questionId: 0,
  questionText: "",
  isLoading: false,
};

const AppContext = createContext();

const AppProvider = ({ children }) => {
  const [state, dispatch] = useReducer(reducer, initialState);

  const handleAddQuestionSubmit = async (data) => {
    dispatch({ type: SET_LOADING });
    try {
      const res = await api.post(URL.addQuestionURL, data);
      if (res.data.statusCode === 200) {
        dispatch({ type: ADD_QUESTION, data: res.data.payload });
      }
      return res.data;
    } catch (error) {
      console.error("Error while adding question!", error);
      return { statusCode: 500, message: "Error while adding question" };
    }
  };

  return (
    <AppContext.Provider
      value={{
        ...state,
        handleAddQuestionSubmit,
      }}
    >
      {children}
    </AppContext.Provider>
  );
};

const useAppContext = () => {
  return useContext(AppContext);
};

export { AppProvider, useAppContext };