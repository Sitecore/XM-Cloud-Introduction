'use client';
import React, { JSX } from 'react';
import { Box, Link as ChakraLink } from '@chakra-ui/react';
import { LinkField, Link as ContentSdkLink, Page } from '@sitecore-content-sdk/nextjs';
import { ComponentProps } from 'lib/component-props';

// Define the type of props that Link will accept
interface Fields {
  /** Link */
  Link: LinkField;
}

export type LinkProps = ComponentProps & {
  fields: Fields;
  page: Page;
};

export const Link = (props: LinkProps): JSX.Element => {
  return (
    <Box>
      <ChakraLink
        as={ContentSdkLink}
        field={props.fields.Link}
        isExternal={props.fields?.Link?.value?.target == '_blank'}
      />
    </Box>
  );
};

export default Link;