import {
  Box,
  Button,
  Flex,
  Heading,
  Link as ChakraLink,
  Spacer,
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  IconButton,
  useBreakpointValue,
} from "@chakra-ui/react";
import { Link } from "react-router-dom";
import { HamburgerIcon } from "@chakra-ui/icons";

const Navbar = () => {
  const isMobile = useBreakpointValue({ base: true, md: false });

  return (
    <Box bg="gray.100" px={4}>
      <Flex h={16} alignItems="center" justifyContent="space-between">
        <Box>
          <Heading size="md" as={Link} to="/" lineHeight="inherit">
            MockMentor
          </Heading>
        </Box>
        <Spacer />
        {isMobile ? (
          <Menu>
            <MenuButton
              as={IconButton}
              icon={<HamburgerIcon />}
              variant="outline"
              aria-label="Options"
            />
            <MenuList>
              <MenuItem as={Link} to="/add-question">
                Start Interview
              </MenuItem>
            </MenuList>
          </Menu>
        ) : (
          <Button as={Link} to="/add-question" colorScheme="blue">
            Start Interview
          </Button>
        )}
      </Flex>
    </Box>
  );
};

export default Navbar;
