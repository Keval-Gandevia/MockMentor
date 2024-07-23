import { useToast } from "@chakra-ui/react";

const ToastNotification = () => {

    const toast = useToast();

    const showToast = ({ message, status }) => {
      toast({
        title: message,
        status: status,
        duration: 5000,
        isClosable: true,
        position: "top-right",
      });
    };
  
    return { showToast };
}

export default ToastNotification