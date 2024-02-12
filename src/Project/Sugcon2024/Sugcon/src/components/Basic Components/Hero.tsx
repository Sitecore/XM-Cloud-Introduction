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
      maxHeight={{ base: 'auto', md: '400px' }}
    >
      <Flex direction="column" p={5} minWidth="50%" height="100%" ml={6} mr={6}>
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
      <Box minWidth={{ base: '100%', md: '50%' }} maxHeight="400px">
        {' '}
        <Image
          src={props.fields.Image?.value?.src}
          //alt={props.fields.Image?.value?.alt}
          width="full"
          height="100%"
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
      maxHeight={{ base: 'auto', md: '400px' }}
    >
      <Flex direction="column" ml={6} mr={6} p={5} height="100%" minWidth="50%">
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
      <Box minWidth={{ base: '100%', md: '50%' }} maxHeight="400px" height="100%">
        <Image
          src={props.fields.Image?.value?.src}
          //alt={props.fields.Image?.value?.alt}
          width="full"
          height="100%"
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
      <Box
        mb={14}
        w={{ base: 'auto', md: '50vw' }}
        maxWidth="620px"
        zIndex={2}
        ml={{ base: 'auto', md: '6' }}
        mr={{ base: 'auto', md: '10' }}
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
        display={{ base: 'none', md: 'block' }}
        position="absolute"
        right={0}
        top={-14}
        bottom={0}
        left="50%"
        width="50vw"
        backgroundImage="url('/images/SUGCON-justification-letter-chatbox-artwork.svg')"
        backgroundSize="contain"
        backgroundPosition="right center"
        backgroundRepeat="no-repeat"
        zIndex={0}
      />
    </Flex>
  );
};
