import {
  Box,
  Button,
  Container,
  Heading,
  HStack,
  Spinner,
  Text,
  useBreakpointValue,
  VStack,
} from "@chakra-ui/react";
import React, { useEffect } from "react";
import { useAppContext } from "../context/AppContext";
import { useNavigate } from "react-router-dom";

const ViewFeedback = () => {
  const isMobile = useBreakpointValue({ base: true, md: false });
  const {
    questionText,
    isLoading,
    getFeedback,
    getEmotion,
    clearLocalStorage,
    feedbackText,
    emotionValue,
  } = useAppContext();
  const navigate = useNavigate();

  const handleRefresh = async () => {
    await getFeedback();
    await getEmotion();
  };

  const handleEndInterview = () => {
    clearLocalStorage();
    navigate("/");
  };

  useEffect(() => {
    handleRefresh();
  }, []);

  return (
    <Container maxW="container.lg" py={8}>
      <VStack spacing={6} align="stretch">
        <Box bg="blue.500" p={2} rounded="md" color="white" textAlign="center">
          <Heading as="h1" size="md">
            {questionText || "No Question Added"}
          </Heading>
        </Box>
        <HStack
          spacing={6}
          align="start"
          flexDirection={isMobile ? "column" : "row"}
        >
          <Box
            flex={2}
            bg="gray.100"
            p={4}
            rounded="md"
            shadow="md"
            overflow="auto"
            maxH="60vh"
          >
            <Heading as="h2" size="md" mb={4}>
              Feedback
            </Heading>
            {isLoading ? (
              <Spinner />
            ) : feedbackText ? (
              <Text fontSize="md" whiteSpace="pre-wrap">
                {feedbackText}
              </Text>
            ) : (
              <Text fontSize="md" whiteSpace="pre-wrap">
                Result is pending
              </Text>
            )}
          </Box>
          <Box
            flex={1}
            bg="gray.100"
            p={4}
            rounded="md"
            shadow="md"
            overflow="auto"
            maxH="60vh"
          >
            <Heading as="h2" size="md" mb={4}>
              Your Behaviour
            </Heading>
            {isLoading ? (
              <Spinner />
            ) : emotionValue ? (
              <Text fontSize="md" whiteSpace="pre-wrap">
                {emotionValue}
              </Text>
            ) : (
              <Text fontSize="md" whiteSpace="pre-wrap">
                Result is pending
              </Text>
            )}
          </Box>
        </HStack>
        <HStack spacing={4} justifyContent={isMobile ? "center" : "flex-end"}>
          <Button colorScheme="blue" onClick={handleRefresh}>
            Refresh
          </Button>
          <Button colorScheme="red" onClick={handleEndInterview}>
            End Interview
          </Button>
        </HStack>
      </VStack>
    </Container>
  );
};

export default ViewFeedback;
