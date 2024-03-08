import React from 'react';
import { Box, Heading, Text, Image, Flex, useBreakpointValue } from '@chakra-ui/react';
import { Field, ImageField, LinkField } from '@sitecore-jss/sitecore-jss-nextjs';
import { ButtonLink } from '../../basics/ButtonLink';
import {
  isSitecoreLinkFieldPopulated,
  isSitecoreTextFieldPopulated,
} from 'lib/utils/sitecoreUtils';

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
      alignItems="stretch"
      bg="#f0f0f0"
      maxHeight={{ base: 'auto', md: '400px' }}
      w="100%"
      h={{ base: 'auto', md: '100vh' }}
      overflow="hidden"
    >
      <Flex
        direction="column"
        margin="0 auto" // Center the content box
        p={5}
        flexGrow={1}
        minWidth="50%"
        justifyContent="center"
      >
        <Box width="auto" alignSelf="end" maxWidth="620px" minWidth="360px">
          <Heading as="h2" fontSize="30px" fontWeight="bold" mb="33px">
            {props.fields.Headline?.value}
          </Heading>
          {isSitecoreTextFieldPopulated(props.fields.EventDate) && (
            <Text fontSize="18px" mb={6}>
              {props.fields.EventDate?.value}
            </Text>
          )}
          {isSitecoreTextFieldPopulated(props.fields.Text) && (
            <Text mb={6} fontSize="18px">
              {props.fields.Text?.value}
            </Text>
          )}
          {isSitecoreLinkFieldPopulated(props.fields.CallToAction) && (
            <Box width="auto" alignSelf="start">
              <ButtonLink field={props.fields.CallToAction} />
            </Box>
          )}
        </Box>
      </Flex>
      <Box flexShrink={0} minWidth={{ base: '100%', md: '50%' }} h="full" overflow="hidden">
        {' '}
        <Image
          src={props.fields.Image?.value?.src}
          //alt={props.fields.Image?.value?.alt}
          width="full"
          height="full"
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
      w="100%"
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
          {isSitecoreTextFieldPopulated(props.fields.Text) && (
            <Text mb={6} fontSize="18px">
              {props.fields.Text?.value}
            </Text>
          )}
        </Box>
      </Flex>
      <Box minWidth={{ base: '100%', md: '50%' }} maxHeight="400px" h="100%" overflow="hidden">
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
      p={5}
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
        w={{ base: 'full', md: '50vw' }}
        maxWidth="620px" // Max width is 620px
        zIndex={2}
        mr={10}
        justifyContent="flex-start"
        alignItems="flex-start"
      >
        <Heading as="h2" fontSize="30px" color="black" fontWeight="bold" mt={10} mb="33px">
          {props.fields.Headline?.value}
        </Heading>
        {isSitecoreTextFieldPopulated(props.fields.Text) && (
          <Text mb={6} fontSize="18px" color="black">
            {props.fields.Text?.value}
          </Text>
        )}
        {isSitecoreLinkFieldPopulated(props.fields.CallToAction) && (
          <Box width="auto" alignSelf="start">
            <ButtonLink field={props.fields.CallToAction} variant="secondary" />
          </Box>
        )}
      </Box>
      <Box
        display={{ base: 'none', md: 'block' }}
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
        backgroundPosition="right bottom"
        backgroundRepeat="no-repeat"
        zIndex={0}
        display={{ base: 'none', md: 'block' }}
      />
    </Flex>
  );
};
