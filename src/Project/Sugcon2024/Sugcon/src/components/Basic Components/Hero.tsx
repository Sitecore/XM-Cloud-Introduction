import React from 'react';
import {
  Box,
  Heading,
  Text,
  Button,
  Image,
  Flex,
  Link,
  useBreakpointValue,
} from '@chakra-ui/react';
import { Field, ImageField, LinkField } from '@sitecore-jss/sitecore-jss-nextjs';

// Define the type of props that Hero will accept
interface Fields {
  /** Title of the event banner */
  Headline: Field<string>;

  /** Date of the event */
  EventDate: Field<string>;

  /** Description of the event */
  Text: Field<string>;

  /** Link to trigger when the button is clicked */
  CallToAction: LinkField;

  /** URL for the event image */
  Image: ImageField;
}

export type HeroProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const HeroHomepage = (props: HeroProps): JSX.Element => {
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
        <Box width="auto" alignSelf="end" maxWidth="620px" minWidth="360px">
          <Heading as="h2" fontSize="30px" fontWeight="bold" mb="33px">
            {props.fields.Headline?.value}
          </Heading>
          {props.fields.EventDate?.value !== '' && (
            <Text fontSize="18px" mb={6}>
              {props.fields.EventDate?.value}
            </Text>
          )}
          {props.fields.Text?.value !== '' && (
            <Text mb={6} fontSize="18px">
              {props.fields.Text?.value}
            </Text>
          )}
          {props.fields.CallToAction?.value?.href !== '' && (
            <Box width="auto" alignSelf="start">
              <Link
                href={props.fields.CallToAction?.value?.href}
                isExternal={props.fields.CallToAction?.value?.target == '_blank'}
              >
                <Button variant="primary">{props.fields.CallToAction?.value?.anchor}</Button>
              </Link>
            </Box>
          )}
        </Box>
      </Flex>
      <Box flex="1" position="relative" minWidth="50%" maxHeight="400px">
        {' '}
        <Image
          src={props.fields.Image?.value?.src}
          //alt={props.fields.Image?.value?.alt}
          width="full"
          height="100%"
          maxHeight="400px"
          objectFit="cover"
        />
      </Box>
    </Flex>
  );
};

export const HeroEvent = (props: HeroProps): JSX.Element => {
  return (
    <Flex
      direction={{ base: 'column', md: 'row' }}
      alignItems="center"
      bg="black"
      color="white"
      w="100vw"
      maxHeight="400px"
    >
      <Flex
        direction="column"
        margin="0 auto" // Center the content box
        p={5}
        flexGrow={1}
        minWidth="50%"
      >
        <Box width="auto" alignSelf="end" maxWidth="620px" minWidth="360px">
          <Heading as="h2" fontSize="30px" fontWeight="bold" mb="33px">
            {props.fields.Headline?.value}
          </Heading>
          {props.fields.Text?.value !== '' && (
            <Text mb={6} fontSize="18px">
              {props.fields.Text?.value}
            </Text>
          )}
        </Box>
      </Flex>
      <Box flex="1" position="relative" minWidth="50%" maxHeight="400px">
        {' '}
        <Image
          src={props.fields.Image?.value?.src}
          //alt={props.fields.Image?.value?.alt}
          width="full"
          height="100%"
          maxHeight="400px"
          objectFit="cover"
        />
      </Box>
    </Flex>
  );
};

export const HeroJustificationLetter = (props: HeroProps): JSX.Element => {
  // Responsive background gradient direction
  const bgGradientDirection = useBreakpointValue({ base: '180deg' });

  return (
    <Flex
      direction={{ base: 'column', md: 'row' }}
      justifyContent="center"
      color="white"
      w="100vw"
      maxH="400px"
      mt={24}
      position="relative"
      bgGradient={`linear(${bgGradientDirection}, #F2F2F2, #F2F2F2 50%, #374086)`}
      zIndex={1}
    >
      {/* Content box that should take the left half of the screen */}
      <Box
        mb={14}
        w={{ base: '50vw' }} // Take full width on base, and half viewport width on md and up
        maxWidth="620px" // Max width is 620px
        zIndex={2}
        mr={10}
        justifyContent="flex-start"
        alignItems="flex-start"
      >
        <Heading as="h2" fontSize="30px" color="black" fontWeight="bold" mt={10} mb="33px">
          {props.fields.Headline?.value}
        </Heading>
        {props.fields.Text?.value && (
          <Text mb={6} fontSize="18px" color="black">
            {props.fields.Text?.value}
          </Text>
        )}
        {props.fields.CallToAction?.value?.href !== '' && (
          <Box width="auto" alignSelf="start">
            <Link
              href={props.fields.CallToAction?.value?.href}
              isExternal={props.fields.CallToAction?.value?.target == '_blank'}
            >
              <Button variant="secondary">{props.fields.CallToAction?.value?.anchor}</Button>
            </Link>
          </Box>
        )}
      </Box>
      <Box
        w={{ base: '50vw' }} // Take full width on base, and half viewport width on md and up
        maxWidth="620px" // Max width is 620px
      >
        &nbsp;
      </Box>

      {/* Background Image */}
      <Box
        position="absolute"
        right={0}
        top={-14}
        bottom={0}
        left="50%" // Start from the middle of the screen
        width="50vw" // Take up the right half of the screen
        backgroundImage="url('/images/SUGCON-justification-letter-chatbox-artwork.svg')"
        backgroundSize="contain"
        backgroundPosition="right center"
        backgroundRepeat="no-repeat"
        zIndex={0}
      />
    </Flex>
  );
};
