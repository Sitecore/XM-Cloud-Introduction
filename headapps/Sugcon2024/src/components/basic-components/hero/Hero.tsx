import React, { JSX } from 'react';
import { Box, Heading, Text, Image, Flex, Button } from '@chakra-ui/react';

import {
  TextField,
  ImageField,
  LinkField,
  Text as JssText,
  Link as JssLink,
  withDatasourceCheck,
} from '@sitecore-content-sdk/nextjs';

import { ButtonLink } from '../../../basics/ButtonLink';

import {
  isSitecoreLinkFieldPopulated,
  isSitecoreTextFieldPopulated,
} from 'lib/utils/sitecoreUtils';
import { PaddingX, Template } from 'components/templates/LayoutConstants';
import { LayoutFlex } from 'components/page-structure/layout-flex/LayoutFlex';
import { ComponentProps } from 'lib/component-props';

// Define the type of props that Hero will accept
interface Fields {
  /** Title of the event banner */
  Headline: TextField;

  /** Date of the event */
  EventDate: TextField;

  /** Description of the event */
  Text: TextField;

  /** Link to trigger when the button is clicked */
  CallToAction: LinkField;

  /** URL for the event image */
  Image: ImageField;
}

export type HeroProps = ComponentProps & {
  fields: Fields;
};

const HeroHomepageComponent = (props: HeroProps): JSX.Element => {
  return (
    <Flex flexDir={{ base: 'column', md: 'row' }} width="100%" paddingRight="0px" paddingLeft="0px">
      <Box
        ml="auto"
        maxW={{ base: '100%', md: `calc((${Template.MaxWidth}) / 2)` }}
        px={{ base: PaddingX.Mobile, md: PaddingX.Desktop }}
        py={{ base: '30px', md: '60px' }}
      >
        <Heading as="h1" fontSize="3xl" fontWeight="bold" mb="33px">
          <JssText field={props.fields.Headline} />
        </Heading>

        {isSitecoreTextFieldPopulated(props.fields.EventDate) && (
          <Text fontSize="18px" mb={6}>
            <JssText field={props.fields.EventDate} />
          </Text>
        )}

        {isSitecoreTextFieldPopulated(props.fields.Text) && (
          <Text mb={6} fontSize="18px">
            <JssText field={props.fields.Text} />
          </Text>
        )}

        {isSitecoreLinkFieldPopulated(props.fields.CallToAction) && (
          <Box width="auto" alignSelf="start">
            <ButtonLink field={props.fields.CallToAction} />
          </Box>
        )}
      </Box>
      <Flex flexGrow={{ md: 'grow' }} maxW={{ base: '100%', md: '50%' }}>
        <Image
          alt="Event Image"
          w="full"
          h="auto"
          objectFit="cover"
          objectPosition="center"
          aspectRatio="1440/500"
          src={props.fields.Image?.value?.src}
          minH="220px"
        />
      </Flex>
    </Flex>
    
  );
};

export const HeroHomepage = withDatasourceCheck()<HeroProps>(HeroHomepageComponent);

const HeroEventComponent = (props: HeroProps): JSX.Element => {
  return (
    <>
    <Flex 
      flexDir={{ base: 'column', md: 'row' }} 
      width="100%" 
      paddingRight="0px" 
      paddingLeft="0px"
      bg="black"
      color="white"
      >
      <Box
        ml="auto"
        maxW={{ base: '100%', md: `calc((${Template.MaxWidth}) / 2)` }}
        px={{ base: PaddingX.Mobile, md: PaddingX.Desktop }}
        py={{ base: '30px', md: '60px' }}
      >
        <Heading as="h1" fontSize="3xl" fontWeight="bold" mb="33px">
          <JssText field={props.fields.Headline} />
        </Heading>

        {isSitecoreTextFieldPopulated(props.fields.EventDate) && (
          <Text fontSize="18px" mb={6}>
            <JssText field={props.fields.EventDate} />
          </Text>
        )}

        {isSitecoreTextFieldPopulated(props.fields.Text) && (
          <Text mb={6} fontSize="18px">
            <JssText field={props.fields.Text} />
          </Text>
        )}

        {isSitecoreLinkFieldPopulated(props.fields.CallToAction) && (
          <Box width="auto" alignSelf="start">
            <ButtonLink field={props.fields.CallToAction} />
          </Box>
        )}
      </Box>
      <Flex flexGrow={{ md: 'grow' }} maxW={{ base: '100%', md: '50%' }}>
        <Image
          alt="Event Image"
          w="full"
          h="auto"
          objectFit="cover"
          objectPosition="center"
          aspectRatio="1440/500"
          src={props.fields.Image?.value?.src}
          minH="220px"
        />
      </Flex>
    </Flex>
    </>
  );
};

export const HeroEvent = withDatasourceCheck()<HeroProps>(HeroEventComponent);

const HeroJustificationLetterComponent = (props: HeroProps): JSX.Element => {
  return (
    <Flex position="relative" width="full" alignItems="center" mt="30px">
      {/* Content */}
      <LayoutFlex
        position="relative"
        zIndex={2}
        flexDirection="column"
        alignItems="flex-start"
        my={{ base: '20px', lg: '30px' }}
      >
        <Heading as="h2" fontSize="3xl" color="black" fontWeight="bold" mb="33px">
          <JssText field={props.fields.Headline} />
        </Heading>

        {isSitecoreTextFieldPopulated(props.fields.Text) && (
          <Text mb={6} fontSize="18px" color="black">
            <JssText field={props.fields.Text} />
          </Text>
        )}

        <Button as={JssLink} field={props?.fields?.CallToAction} variant="secondary" size="md">
          {props?.fields?.CallToAction?.value?.text}
        </Button>
      </LayoutFlex>

      {/* Background Image */}
      <Box
        display={{ base: 'none', lg: 'block' }}
        position="absolute"
        top="-30px"
        right={0}
        bottom={0}
        bgImage="url('/images/SUGCON-justification-letter-chatbox-artwork.svg')"
        bgRepeat="no-repeat"
        bgPos="top right"
        bgSize="contain"
        w="full"
        h="calc(100% + 30px)"
        zIndex={1}
      />

      {/* Gradient Overlay */}
      <Box
        position="absolute"
        top={0}
        left={0}
        right={0}
        bottom={0}
        bgGradient="linear(180deg, #F2F2F2, #F2F2F2 50%, #374086)"
        zIndex={0}
      />
    </Flex>
  );
};

export const HeroJustificationLetter = withDatasourceCheck()<HeroProps>(
  HeroJustificationLetterComponent
);
