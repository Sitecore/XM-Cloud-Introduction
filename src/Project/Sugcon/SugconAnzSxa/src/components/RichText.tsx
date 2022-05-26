import React from 'react';
import { Field, RichText as JssRichText } from '@sitecore-jss/sitecore-jss-nextjs';

interface Fields {
  Text: Field<string>;
}

export type RichTextProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: RichTextProps): JSX.Element => {
  return (
    <div className="container component">
      <div className="row richtext">
        <div className="col-12">
          <JssRichText field={props.fields.Text} />
        </div>
      </div>
    </div>
  );
};
