import axios from "axios";
import { createContext, useContext, useReducer } from "react";
import reducer from "./reducer";

// set-up the basic API call with axios.
const api = axios.create({
  baseURL: `https://localhost:44337/api`,
});

const URL = {
    addQuestionURL: `/Questions/addQuestion`,
}

const initialState = {
  questionText: "",
};

const AppContext = createContext();

const AppProvider = ({ children }) => {
  const [state, dispatch] = useReducer(reducer, initialState);

  const handleAddQuestionSubmit = async (data) => {
    const response = await api.post(URL.addQuestionURL, data)
      .then((res) => {
        console.log(res.data);
        return res;
      })
  }

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
