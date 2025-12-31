'use client';
import React, { JSX } from 'react';
import { Box, Heading, Text, Image, Link } from '@chakra-ui/react';
import {
  Field,
  ImageField,
  LinkField,
  TextField,
  Image as ContentSdkImage,
  Link as ContentSdkLink,
  Text as ContentSdkText,
  useSitecore,
} from '@sitecore-content-sdk/nextjs';

// Define the type of props that Event will accept
export interface Fields {
  /** URL for the event logo */
  Image: ImageField;

  /** Name of the event */
  EventName: TextField;

  /** Date of the event */
  EventDate: Field<string>;

  /** City of the event */
  EventCity: Field<string>;

  /** State of the event */
  EventState: Field<string>;

  /** Country of the event */
  EventCountry: Field<string>;

  /** Link to event website */
  LinkToSite: LinkField;
}

export const Default = (props: Fields): JSX.Element => {
  const { page } = useSitecore();
  const dateString: string =
    props.EventDate?.value !== '0001-01-01T00:00:00Z'
      ? new Date(props.EventDate?.value).toDateString()
      : '';

  return (
    <Box
      w="full"
      minW={{ base: '100%', md: 'calc(50% - 30px)', lg: 'calc(25% - 30px)' }}
      px={8}
      py={4}
      mb={{ lg: '55px' }}
      bg="sugcon.gray.200"
      borderRadius={{ base: 'lg', lg: '8px 8px 0 8px' }}
      position="relative"
    >
      <Image
        as={ContentSdkImage}
        src={props.Image?.value?.src}
        maxW="190px"
        h="auto"
        field={props?.Image}
        mb="20px"
      />

      <Heading as="h3" fontSize="25px" mb="12px">
        <ContentSdkText field={props?.EventName} />
      </Heading>

      <Text color="sugcon.gray.500" mb={0}>
        {page.mode.isEditing ? <ContentSdkText field={props?.EventDate} /> : <>{dateString}</>}
      </Text>

      <Text color="sugcon.gray.500" mb="12px">
        <ContentSdkText field={props?.EventCity} />, <ContentSdkText field={props?.EventState} />{' '}
        <ContentSdkText field={props?.EventCountry} />
      </Text>

      <Link
        as={ContentSdkLink}
        size="sm"
        isExternal={props.LinkToSite?.value?.target == '_blank'}
        field={props?.LinkToSite}
      />

      {/* Triangle Corner */}
      <Box
        position="absolute"
        bottom="-38px"
        right="-40px"
        w={0}
        h={0}
        borderStyle="solid"
        borderWidth="40px"
        borderTopColor="transparent"
        borderRightColor="transparent"
        borderBottomColor="sugcon.gray.200"
        borderLeftColor="transparent"
        transform="rotate(45deg)"
        display={{ base: 'none', lg: 'block' }}
      />
    </Box>
  );
};
