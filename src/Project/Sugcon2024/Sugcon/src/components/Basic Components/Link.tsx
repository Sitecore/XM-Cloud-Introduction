import React from 'react';
import { Box, Link } from '@chakra-ui/react';
import { LinkField } from '@sitecore-jss/sitecore-jss-nextjs';

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
        href={props.fields?.Link?.value?.href}
        isExternal={props.fields?.Link?.value?.target == '_blank'}
      >
        {props.fields?.Link?.value?.text}
      </Link>
    </Box>
  );
};
