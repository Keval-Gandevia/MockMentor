import React from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Box,
  Button,
  FormControl,
  FormErrorMessage,
  FormLabel,
  Textarea,
  Heading,
  Spinner,
} from "@chakra-ui/react";
import { useAppContext } from "../context/AppContext";
import ToastNotification from "../components/ToastNotification";
import { useNavigate } from "react-router-dom";

// schema to validate form
const schema = z.object({
  questionText: z.string().min(1, "Question Text is required"),
});

const AddQuestion = () => {
  const { isLoading, handleAddQuestionSubmit } = useAppContext();
  const { showToast } = ToastNotification();
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(schema),
  });

  const onSubmit = async (data) => {
    const response = await handleAddQuestionSubmit(data);
    if (response.statusCode === 201) {
      showToast({ message: response.message, status: "success" });
      navigate(`/record-video`);
    } else {
      showToast({ message: response.message, status: "error" });
    }
  };

  return (
    <Box
      maxW={{ base: "90%", md: "md" }}
      mx="auto"
      mt={10}
      px={{ base: 4, md: 0 }}
    >
      <Heading
        as="h1"
        size="lg"
        mb={6}
        textAlign={{ base: "center", md: "left" }}
      >
        Add Question
      </Heading>
      <form onSubmit={handleSubmit(onSubmit)}>
        <FormControl isInvalid={errors.questionText}>
          <FormLabel htmlFor="questionText">Question</FormLabel>
          <Textarea
            id="questionText"
            placeholder="Enter your question here"
            {...register("questionText")}
          />
          <FormErrorMessage>
            {errors.questionText && errors.questionText.message}
          </FormErrorMessage>
        </FormControl>
        <Button
          mt={4}
          colorScheme="blue"
          type="submit"
          w="full"
          isLoading={isLoading}
        >
          {isLoading ? <Spinner size="sm" /> : "Add"}
        </Button>
      </form>
    </Box>
  );
};

export default AddQuestion;
