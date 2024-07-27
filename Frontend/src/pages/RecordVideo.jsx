import React, { useState, useRef } from "react";
import { Box, Button, Text, VStack, HStack } from "@chakra-ui/react";
import { useAppContext } from "../context/AppContext";
import ToastNotification from "../components/ToastNotification";

const RecordVideo = () => {
  const [isRecording, setIsRecording] = useState(false);
  const [mediaRecorder, setMediaRecorder] = useState(null);
  const [videoUrl, setVideoUrl] = useState("");
  const videoRef = useRef(null);
  const recordedChunks = useRef([]);
  const { showToast } = ToastNotification();

  const {questionText, handleVideoSubmit} = useAppContext();

  const handlePermissions = async () => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({
        video: true,
        audio: true,
      });
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

  const onSubmit = async (videoUrl) => {
    const response = await handleVideoSubmit(videoUrl);
    if (response.statusCode === 201) {
      showToast({ message: response.message, status: "success" });
      // navigate(`/record-video`);
    } else {
      showToast({ message: response.message, status: "error" });
    }
  }


  // const downloadVideo = () => {
  //   const link = document.createElement("a");
  //   link.href = videoUrl;
  //   link.download = "recorded_video.webm";
  //   document.body.appendChild(link);
  //   link.click();
  //   document.body.removeChild(link);
  // };

  return (
    <Box display="flex" flexDirection="column" alignItems="center" p={4} height="100vh">
      <Text mb={4}>Question: {questionText}</Text>
      <Box width="70%" bg="black" height="400px" display="flex" justifyContent="center" alignItems="center" mb={4}>
        <video ref={videoRef} autoPlay muted width="100%" height="100%" style={{ backgroundColor: "black" }} />
      </Box>
      <HStack spacing={4} mt={2}>
        <Button colorScheme="blue" onClick={handlePermissions}>
          Grant Permissions
        </Button>
        <Button colorScheme="green" onClick={startRecording} isDisabled={!mediaRecorder || isRecording}>
          Start
        </Button>
        <Button colorScheme="red" onClick={stopRecording} isDisabled={!isRecording}>
          Stop
        </Button>
        <Button colorScheme="purple" onClick={() => onSubmit(videoUrl)} isDisabled={!videoUrl}>
          Submit
        </Button>
      </HStack>
    </Box>
  );
};

export default RecordVideo;