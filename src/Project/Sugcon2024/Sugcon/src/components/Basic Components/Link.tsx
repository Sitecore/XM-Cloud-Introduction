import React from 'react';
import { Box, Link } from '@chakra-ui/react';
import { LinkField, Link as JssLink } from '@sitecore-jss/sitecore-jss-nextjs';

// Define the type of props that Link will accept
interface Fields {
  /** Link */
  Link: LinkField;
}

export type LinkProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: LinkProps): JSX.Element => {
  return (
    <Box>
      <Link
        as={JssLink}
        field={props.fields.Link}
        isExternal={props.fields?.Link?.value?.target == '_blank'}
      />
    </Box>
  );
};
