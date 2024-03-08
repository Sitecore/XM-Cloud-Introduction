import React from 'react';
import { LinkField } from '@sitecore-jss/sitecore-jss-nextjs';
import { Button, Link } from '@chakra-ui/react';

type ButtonLinkProps = {
  // Sitecore link field
  field: LinkField;

  // button variation
  variant?: string;
};

export const ButtonLink = (props: ButtonLinkProps): JSX.Element => {
  const { href, target = '', text } = props.field.value;
  const { variant = 'primary' } = props;

  return (
    <Link href={href} isExternal={target == '_blank'}>
      <Button variant={variant}>{text}</Button>
    </Link>
  );
};
