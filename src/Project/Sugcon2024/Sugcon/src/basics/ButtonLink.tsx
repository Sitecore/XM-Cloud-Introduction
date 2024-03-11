import React from 'react';
import { LinkField, Link as JssLink, useSitecoreContext } from '@sitecore-jss/sitecore-jss-nextjs';
import { Button, Link } from '@chakra-ui/react';

type ButtonLinkProps = {
  // Sitecore link field
  field: LinkField;

  // button variation
  variant?: string;
};

export const ButtonLink = (props: ButtonLinkProps): JSX.Element => {
  const { sitecoreContext } = useSitecoreContext();
  const { href, target = '', text } = props.field.value;
  const { variant = 'primary' } = props;

  // If in Sitecore Experience Editor, return a JSS Link
  if (sitecoreContext.pageEditing) {
    return (
      <Button variant={variant}>
        <JssLink field={props.field} />
      </Button>
    )
  }

  return (
    <Link href={href} isExternal={target == '_blank'}>
      <Button variant={variant}>{text}</Button>
    </Link>
  );
};
