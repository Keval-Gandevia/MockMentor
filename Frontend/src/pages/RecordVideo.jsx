import React, { useState, useRef } from "react";
import { Box, Button, Text, VStack, HStack, Spinner } from "@chakra-ui/react";
import { useAppContext } from "../context/AppContext";
import ToastNotification from "../components/ToastNotification";
import { useNavigate } from "react-router-dom";

const RecordVideo = () => {
  const [isRecording, setIsRecording] = useState(false);
  const [mediaRecorder, setMediaRecorder] = useState(null);
  const [videoUrl, setVideoUrl] = useState("");
  const videoRef = useRef(null);
  const streamRef = useRef(null);
  const recordedChunks = useRef([]);
  const { showToast } = ToastNotification();
  const navigate = useNavigate();

  const { questionText, handleVideoSubmit, isLoading } = useAppContext();

  const handlePermissions = async () => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({
        video: true,
        audio: true,
      });
      streamRef.current = stream;
      videoRef.current.srcObject = stream;
      const recorder = new MediaRecorder(stream);
      recorder.ondataavailable = (event) => {
        if (event.data.size > 0) {
          recordedChunks.current.push(event.data);
        }
      };
      recorder.onstop = () => {
        const blob = new Blob(recordedChunks.current, {
          type: "video/webm",
        });
        setVideoUrl(URL.createObjectURL(blob));
        recordedChunks.current = [];
      };
      setMediaRecorder(recorder);
    } catch (error) {
      console.error("Error accessing media devices.", error);
    }
  };

  const startRecording = () => {
    mediaRecorder.start();
    setIsRecording(true);
  };

  const stopRecording = () => {
    mediaRecorder.stop();
    setIsRecording(false);
  };

  const stopStream = () => {
    if (streamRef.current) {
      streamRef.current.getTracks().forEach((track) => track.stop());
      streamRef.current = null;
    }
  };

  const onSubmit = async (videoUrl) => {
    stopStream();
    const response = await handleVideoSubmit(videoUrl);
    if (response.statusCode === 201) {
      showToast({ message: response.message, status: "success" });
      navigate("/view-feedback");
    } else {
      showToast({ message: response.message, status: "error" });
    }
  };

  return (
    <Box
      display="flex"
      flexDirection="column"
      alignItems="center"
      p={4}
      height="100vh"
    >
      <Text mb={4}>Question: {questionText}</Text>
      <Box
        width="70%"
        bg="black"
        height="400px"
        display="flex"
        justifyContent="center"
        alignItems="center"
        mb={4}
      >
        <video
          ref={videoRef}
          autoPlay
          muted
          width="100%"
          height="100%"
          style={{ backgroundColor: "black" }}
        />
      </Box>
      {isLoading ? (
        <Spinner size="xl" />
      ) : (
        <HStack spacing={4} mt={2}>
          <Button colorScheme="blue" onClick={handlePermissions}>
            Grant Permissions
          </Button>
          <Button
            colorScheme="green"
            onClick={startRecording}
            isDisabled={!mediaRecorder || isRecording}
          >
            Start
          </Button>
          <Button
            colorScheme="red"
            onClick={stopRecording}
            isDisabled={!isRecording}
          >
            Stop
          </Button>
          <Button
            colorScheme="purple"
            onClick={() => onSubmit(videoUrl)}
            isDisabled={!videoUrl}
          >
            Submit
          </Button>
        </HStack>
      )}
    </Box>
  );
};

export default RecordVideo;
