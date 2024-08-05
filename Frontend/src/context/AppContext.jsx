import axios from "axios";
import { createContext, useContext, useReducer } from "react";
import reducer from "./reducer";
import {
  ADD_QUESTION,
  ADD_VIDEO,
  RESET_STATE,
  SET_EMOTION,
  SET_FEEDBACK,
  SET_LOADING,
} from "./actions";
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
  // baseURL: `${window.location.origin}:8080/api`,
  baseURL: `https://localhost:44337/api`,
});

const URL = {
  addQuestionURL: `/Questions/addQuestion`,
  addVideoURL: `/Videos/addVideo`,
  getFeedbackURL: `/Feedbacks/getFeedback`,
  getEmotionURL: `/Emotions/getEmotion`,
};

const initialState = {
  questionId: localStorage.getItem("questionId") || 0,
  questionText: localStorage.getItem("questionText") || "",
  isLoading: false,
  videoId: localStorage.getItem("videoId") || 0,
  feedbackText: localStorage.getItem("feedbackText") || "Result is pending",
  emotionValue: localStorage.getItem("emotionValue") || "Result is pending",
};

const AppContext = createContext();

const AppProvider = ({ children }) => {
  const [state, dispatch] = useReducer(reducer, initialState);

  const handleAddQuestionSubmit = async (data) => {
    dispatch({ type: SET_LOADING, payload: true });
    try {
      const res = await api.post(URL.addQuestionURL, data);
      if (res.data.statusCode === 201) {
        dispatch({ type: ADD_QUESTION, data: res.data.payload });
      }
      return res.data;
    } catch (error) {
      console.error("Error while adding question!", error);
      return { statusCode: 500, message: "Error while adding question" };
    } finally {
      dispatch({ type: SET_LOADING, payload: false });
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
        ContentType: "video/webm",
      });

      await s3Client.send(command);
      const url = `https://${bucketName}.s3.${
        import.meta.env.VITE_AWS_REGION
      }.amazonaws.com/${key}`;
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
      return await addVideo(videoS3Url);
    } catch (error) {
      alert("Error submitting video");
    }
  };

  const addVideo = async (videoUrl) => {
    dispatch({ type: SET_LOADING, payload: true });
    const video = {
      questionId: state.questionId,
      videoUrl: videoUrl,
    };
    try {
      const res = await api.post(URL.addVideoURL, video);
      dispatch({ type: ADD_VIDEO, data: res.data.payload });
      return res.data;
    } catch (error) {
      console.error("Error while adding video!", error);
      return { statusCode: 500, message: "Error while adding video" };
    } finally {
      dispatch({ type: SET_LOADING, payload: false });
    }
  };

  const getFeedback = async () => {
    dispatch({ type: SET_LOADING, payload: true });
    try {
      const res = await api.get(
        `${URL.getFeedbackURL}?questionId=${state.questionId}`
      );
      if (res.data.statusCode !== 404) {
        dispatch({ type: SET_FEEDBACK, data: res.data.payload });
      }
      return res.data;
    } catch (error) {
      console.error("Error while fetching feedback", error);
      return { statusCode: 500, message: "Error while fetching feedback" };
    } finally {
      dispatch({ type: SET_LOADING, payload: false });
    }
  };

  const getEmotion = async () => {
    dispatch({ type: SET_LOADING, payload: true });
    try {
      const res = await api.get(
        `${URL.getEmotionURL}?videoId=${state.videoId}`
      );
      if (res.data.statusCode !== 404) {
        dispatch({ type: SET_EMOTION, data: res.data.payload });
      }
      return res.data;
    } catch (error) {
      console.error("Error while fetching feedback", error);
      return { statusCode: 500, message: "Error while fetching feedback" };
    } finally {
      dispatch({ type: SET_LOADING, payload: false });
    }
  };

  const clearLocalStorage = () => {
    localStorage.clear();
    dispatch({ type: RESET_STATE });
  };

  return (
    <AppContext.Provider
      value={{
        ...state,
        handleAddQuestionSubmit,
        handleVideoSubmit,
        getFeedback,
        getEmotion,
        clearLocalStorage,
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
