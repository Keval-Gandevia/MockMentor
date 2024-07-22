import { Box, Button, Flex, Heading, Link as ChakraLink, Spacer, Menu, MenuButton, MenuList, MenuItem } from '@chakra-ui/react';
import { ChevronDownIcon } from '@chakra-ui/icons';
import { Link } from 'react-router-dom';

const Navbar = () => {
  return (
    <Box bg="gray.100" px={4}>
      <Flex h={16} alignItems="center" justifyContent="space-between">
        <Box>
          <Heading size="md">MockMentor</Heading>
        </Box>
        <Spacer />
        <Button colorScheme='blue'>Start Interview</Button>
      </Flex>
    </Box>
  );
};

export default Navbar;
