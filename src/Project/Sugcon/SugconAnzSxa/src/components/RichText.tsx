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
  const text = props.fields ? (
    <JssRichText field={props.fields.Text} />
  ) : (
    <span className="is-empty-hint">Rich text</span>
  );

  return (
    <div className="container component">
      <div className='row richtext'>
        <div className ='col-12'>
          <JssRichText field={props.fields.Text} />
        </div>
      </div>
    </div>
  );
};
