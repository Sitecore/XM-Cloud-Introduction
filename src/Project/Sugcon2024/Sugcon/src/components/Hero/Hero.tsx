import React from 'react';
import { Box, Heading, Text, Button, Image, Flex, Link } from '@chakra-ui/react';

// Define the type of props that Hero will accept
export interface HeroProps {
  /** Title of the event banner */
  title: string;

  /** Date of the event */
  date: string;

  /** Description of the event */
  description: string;

  /** Text for the registration button */
  buttonText: string;

  /** URL for the event image */
  imageUrl: string;

  /** Link to trigger when the button is clicked */
  callToActionLink: string;
}

const Hero: React.FC<HeroProps> = ({
  title,
  date,
  description,
  buttonText,
  imageUrl,
  callToActionLink,
}) => {
  return (
    <Flex
      direction={{ base: 'column', md: 'row' }}
      alignItems="center"
      bg="#f0f0f0"
      w="100vw"
      boxShadow="-20px 19px 40px 0px rgba(0, 0, 0, 0.2) inset"
      maxHeight="400px"
    >
      <Flex
        direction="column"
        margin="0 auto" // Center the content box
        p={5}
        flexGrow={1}
        minWidth="50%"
      >
        <Box width="auto" alignSelf="end" maxWidth="620px">
          <Heading as="H1" fontSize="30px" fontWeight="bold" mb="33px">
            {title}
          </Heading>
          <Text fontSize="18px" mb={3}>
            {date}
          </Text>
          <Text mb={5} fontSize="18px">
            {description}
          </Text>
          <Box width="auto" alignSelf="start">
            <Link href={callToActionLink} isExternal>
              <Button colorScheme="red" size="lg" borderRadius="full">
                {buttonText}
              </Button>
            </Link>
          </Box>
        </Box>
      </Flex>
      <Box flex="1" position="relative" minWidth="50%" maxHeight="400px">
        {' '}
        <Image
          src={imageUrl}
          alt="Event image"
          width="full"
          height="100%"
          maxHeight="400px"
          objectFit="cover"
        />
      </Box>
    </Flex>
  );
};

export default Hero;
