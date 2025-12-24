import React, { JSX } from 'react';
import { ButtonLink } from 'src/basics/ButtonLink';
import { LinkProps } from './Link';

export const Button = (props: LinkProps): JSX.Element => {
  return <ButtonLink field={props.fields.Link} />;
};

export default Button;