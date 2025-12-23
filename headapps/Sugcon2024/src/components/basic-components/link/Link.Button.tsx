import React, { JSX } from 'react';
import { ButtonLink } from 'src/basics/ButtonLink';
import { LinkField, Link as ContentSdkLink, withDatasourceCheck } from '@sitecore-content-sdk/nextjs';
import { ComponentProps } from 'lib/component-props';

// Define the type of props that Link will accept
interface Fields {
  /** Link */
  Link: LinkField;
}

export type LinkProps = ComponentProps & {
  fields: Fields;
};

const LinkButton = (props: LinkProps): JSX.Element => {
  return <ButtonLink field={props.fields.Link} />;
};

export const Default = withDatasourceCheck()<LinkProps>(LinkButton);