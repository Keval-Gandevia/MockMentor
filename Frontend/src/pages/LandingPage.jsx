// src/pages/LandingPage.jsx
import React from 'react';
import { Box, Button, Container, Heading, Text, VStack, Stack, Card, CardBody, CardHeader, CardFooter, Image, Badge, Icon, SimpleGrid } from '@chakra-ui/react';
import { AtSignIcon, QuestionIcon, ViewIcon, InfoOutlineIcon, ArrowForwardIcon, CheckCircleIcon } from '@chakra-ui/icons';
// import { Link as RouterLink } from 'react-router-dom';

const features = [
  {
    title: 'Add Question',
    description: 'Create your own question to simulate a real interview experience.',
    icon: QuestionIcon,
  },
  {
    title: 'Record Interview',
    description: 'Record your response to the question for a comprehensive mock interview.',
    icon: ViewIcon,
  },
  {
    title: 'AI Feedback',
    description: 'Get AI-driven feedback on your interview performance to improve your skills.',
    icon: InfoOutlineIcon,
  },
];

const steps = [
  {
    title: 'Start Interview',
    description: 'Begin by signing in and starting a new interview session.',
    icon: ArrowForwardIcon,
    color: 'blue',
  },
  {
    title: 'Add Question',
    description: 'Add the question you want to practice for your mock interview.',
    icon: QuestionIcon,
    color: 'teal',
  },
  {
    title: 'Record Answer',
    description: 'Record your answer for question to simulate a real interview.',
    icon: ViewIcon,
    color: 'purple',
  },
  {
    title: 'Get AI Feedback',
    description: 'Receive detailed feedback from our AI to improve your skills.',
    icon: CheckCircleIcon,
    color: 'green',
  },
];

const LandingPage = () => {
  return (
    <Container maxW="container.xl" py={8}>
      <VStack spacing={6} textAlign="center">
        <Heading as="h1" size="2xl">
          Welcome to MockMentor - The Mock Interview App
        </Heading>
        <Text fontSize="lg" mb={8}>
          Enhance your interview skills with our AI-driven mock interview platform.
        </Text>
        <Stack spacing={8} direction={['column', 'row']} wrap="wrap" justify="center">
          {features.map((feature, index) => (
            <Card key={index} maxW="sm" borderRadius="lg" overflow="hidden" boxShadow="lg">
              <CardHeader bg="black" color="white" py={4}>
                <VStack>
                  <Icon as={feature.icon} boxSize="24px" />
                  <Heading size="md">{feature.title}</Heading>
                </VStack>
              </CardHeader>
              <CardBody>
                <Text>{feature.description}</Text>
              </CardBody>
            </Card>
          ))}
        </Stack>
        <Heading as="h2" size="xl" mt={10}>
          How to Improve Your Skills
        </Heading>
        <SimpleGrid columns={[1, 2, 2, 4]} spacing={10} mt={6}>
          {steps.map((step, index) => (
            <Card key={index} maxW="sm" borderRadius="lg" overflow="hidden" boxShadow="lg">
              <CardHeader bg={`${step.color}.500`} color="white" py={4}>
                <VStack>
                  <Badge colorScheme={`${step.color}`} fontSize="xl" mb={2}>{index + 1}</Badge>
                  <Icon as={step.icon} boxSize="24px" />
                  <Heading size="md">{step.title}</Heading>
                </VStack>
              </CardHeader>
              <CardBody>
                <Text>{step.description}</Text>
              </CardBody>
            </Card>
          ))}
        </SimpleGrid>
      </VStack>
    </Container>
  );
};

export default LandingPage;
