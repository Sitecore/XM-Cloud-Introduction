import React, { JSX } from 'react';
import { LinkField, Link as ContentSdkLink, Page } from '@sitecore-content-sdk/nextjs';
import { Button, Link } from '@chakra-ui/react';

type ButtonLinkProps = {
  // Sitecore link field
  field: LinkField;

  // button variation
  variant?: string;

  page: Page;
};

export const ButtonLink = (props: ButtonLinkProps): JSX.Element => {
  const { href, target = '', text } = props.field.value;
  const { variant = 'primary', page } = props;

  // If in Sitecore Experience Editor, return a ContentSdkLink
  if (page.mode.isEditing) {
    return (
      <Button variant={variant}>
        <ContentSdkLink field={props.field} />
      </Button>
    );
  }

  return (
    <Link href={href} isExternal={target == '_blank'}>
      <Button variant={variant}>{text}</Button>
    </Link>
  );
};
