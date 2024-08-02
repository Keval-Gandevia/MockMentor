import { Box, Button, Container, Heading, HStack, Text, useBreakpointValue, VStack } from '@chakra-ui/react';
import React from 'react';
import { useAppContext } from '../context/AppContext';

const ViewFeedback = () => {
    const isMobile = useBreakpointValue({ base: true, md: false });
    const { questionText } = useAppContext();

    const userAnswer = "Integer viverra turpis a lectus sagittis, eget consequat tortor pretium. Ut et felis id libero rutrum venenatis. Mauris sit amet sapien purus. Nam euismod turpis id vehicula venenatis. Duis rhoncus nulla nec nisl vestibulum, nec bibendum leo pretium. Aenean porttitor fringilla dolor, non consequat orci ullamcorper ut. Quisque feugiat eros eu mi vulputate, vel viverra dolor consectetur. Nulla facilisi. Sed eget velit ut nisl ultricies consectetur ut nec magna. Curabitur lacinia orci at dapibus scelerisque. Nam nec nunc in tortor condimentum egestas et eget elit. Sed scelerisque, nunc id tristique rhoncus, eros orci sollicitudin erat, a faucibus erat lectus et libero. Pellentesque efficitur tempor arcu, at hendrerit nulla lobortis at. Vivamus vel sapien vel augue sodales pharetra at vel orci. Fusce vehicula efficitur lorem, a elementum ligula suscipit non.";

    const feedback = "Suspendisse commodo, velit sit amet facilisis condimentum, lacus odio ultricies odio, non scelerisque augue justo in eros. Proin dignissim enim id justo posuere, nec tincidunt nunc pretium. Phasellus pharetra tristique lorem, sit amet aliquet turpis efficitur sed. Donec fermentum est quis enim scelerisque, ut gravida felis luctus. Donec consequat efficitur mi, sit amet tincidunt est elementum et. Donec imperdiet risus sed erat porttitor, sit amet feugiat dolor rutrum. Nunc ut consequat lectus. Aliquam erat volutpat. Morbi vulputate, odio non fringilla rutrum, nisi nunc varius lacus, non efficitur ligula elit nec leo. Aliquam tincidunt a orci in sagittis. In vehicula magna non urna tempor, a dapibus justo consequat. Curabitur consectetur odio nec metus consequat fermentum. Nullam ut orci scelerisque, malesuada enim in, dignissim turpis. Vivamus eget odio id velit aliquam fringilla. Quisque sollicitudin lorem sit amet viverra sagittis.";

    const emotions = "Happiness: 60%, Sadness: 10%, Surprise: 15%, Anger: 5%, Disgust: 5%, Fear: 5%";

    return (
        <Container maxW="container.lg" py={8}>
            <VStack spacing={6} align="stretch">
                <Box bg="blue.500" p={2} rounded="md" color="white" textAlign="center">
                    <Heading as="h1" size="md">
                        {"what is aws sns?"}
                    </Heading>
                </Box>
                <HStack spacing={6} align="start" flexDirection={isMobile ? 'column' : 'row'}>
                    <Box flex={1} bg="gray.100" p={4} rounded="md" shadow="md" overflow="auto" maxH="60vh">
                        <Heading as="h2" size="md" mb={4}>
                            Your Answer
                        </Heading>
                        <Text fontSize="md" whiteSpace="pre-wrap">
                            {userAnswer}
                        </Text>
                    </Box>
                    <Box flex={1} bg="gray.100" p={4} rounded="md" shadow="md" overflow="auto" maxH="60vh">
                        <Heading as="h2" size="md" mb={4}>
                            Feedback
                        </Heading>
                        <Text fontSize="md" whiteSpace="pre-wrap">
                            {feedback}
                        </Text>
                    </Box>
                    <Box flex={1} bg="gray.100" p={4} rounded="md" shadow="md" overflow="auto" maxH="60vh">
                        <Heading as="h2" size="md" mb={4}>
                            Emotions
                        </Heading>
                        <Text fontSize="md" whiteSpace="pre-wrap">
                            {emotions}
                        </Text>
                    </Box>
                </HStack>
                <HStack spacing={4} justifyContent={isMobile ? 'center' : 'flex-end'}>
                    <Button colorScheme="blue" onClick={() => { /* Refresh logic */ }}>Refresh</Button>
                    <Button colorScheme="red" onClick={() => { /* End interview logic */ }}>End Interview</Button>
                </HStack>
            </VStack>
        </Container>
    );
};

export default ViewFeedback;
