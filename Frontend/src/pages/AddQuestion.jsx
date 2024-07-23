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
} from "@chakra-ui/react";
import { useAppContext } from "../context/AppContext";

// schema to validate form
const schema = z.object({
  questionText: z.string().min(1, "Question Text is required"),
});

const AddQuestion = () => {


  const {handleAddQuestionSubmit} = useAppContext();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(schema),
  });

  // const onSubmit = (data) => {
  //   console.log(data);
  // };

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
      <form onSubmit={handleSubmit(handleAddQuestionSubmit)}>
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
        <Button mt={4} colorScheme="blue" type="submit" w="full">
          Add
        </Button>
      </form>
    </Box>
  );
};

export default AddQuestion;
