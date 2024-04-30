import React from 'react';
import { Box, Link } from '@chakra-ui/react';
import { ButtonLink } from 'src/basics/ButtonLink';
import { LinkField, Link as JssLink } from '@sitecore-jss/sitecore-jss-nextjs';
import { LinkField, Link as JssLink, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

// Define the type of props that Link will accept
interface Fields {
  /** Link */
  Link: LinkField;
}

export type LinkProps = ComponentProps & {
  fields: Fields;
};

const LinkComponent = (props: LinkProps): JSX.Element => {
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

export const Default = withDatasourceCheck()<LinkProps>(LinkComponent);

export const Button = (props: LinkProps): JSX.Element => {
  return <ButtonLink field={props.fields.Link} />;
};
