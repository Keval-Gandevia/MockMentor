import axios from "axios";
import { createContext, useContext, useReducer } from "react";
import reducer from "./reducer";
import { ADD_QUESTION, SET_LOADING } from "./actions";
import { S3Client, PutObjectCommand } from "@aws-sdk/client-s3";
import { getSignedUrl } from "@aws-sdk/s3-request-presigner";

const s3Client = new S3Client({
  region: import.meta.env.VITE_AWS_REGION,
  credentials: {
    accessKeyId: import.meta.env.VITE_AWS_ACCESS_KEY_ID,
    secretAccessKey: import.meta.env.VITE_AWS_SECRET_ACCESS_KEY,
    sessionToken: import.meta.env.VITE_AWS_SESSION_TOKEN,
  },
});

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

  const uploadVideoToS3 = async (blob) => {
    const bucketName = import.meta.env.VITE_S3_BUCKET_NAME;
    const key = `${state.questionId}_video.webm`;
    try {
      const command = new PutObjectCommand({
        Bucket: bucketName,
        Key: key,
        Body: blob,
        ContentType: "video/webm"
      });

      await s3Client.send(command);
      const url = `https://${bucketName}.s3.${import.meta.env.VITE_AWS_REGION}.amazonaws.com/${key}`;
      return url;
    } catch (error) {
      console.error("Error uploading video to S3", error);
      throw error;
    }
  };

  const handleVideoSubmit = async (videoUrl) => {
    console.log(videoUrl);
    try {
      const blob = await fetch(videoUrl).then((res) => res.blob());
      const videoS3Url = await uploadVideoToS3(blob);
      console.log(videoS3Url);
      console.log("Video submitted successfully!");
    } catch (error) {
      alert("Error submitting video");
    }
  };

  return (
    <AppContext.Provider
      value={{
        ...state,
        handleAddQuestionSubmit,
        handleVideoSubmit,
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
