import React from 'react';
import {
  Field,
  Link as JssLink,
  LinkField,
  Text as JssText,
} from '@sitecore-jss/sitecore-jss-nextjs';

interface Fields {
  Title: Field<string>;
  Text: Field<string>;
  CallToAction: LinkField;
}

type ActionBannerProps = {
  params: { [key: string]: string };
  fields: Fields;
};

const ActionBannerDefaultComponent = (props: ActionBannerProps): JSX.Element => (
  <div className={`component promo ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Agenda</span>
    </div>
  </div>
);

export const Default = (props: ActionBannerProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  if (props?.fields) {
    return (
      <div className={`component action-banner ${props.params.styles}`} id={id ? id : undefined}>
        <div className="component-content">
          <div className="col-1">
            <h2>
              <JssText field={props.fields.Title} />
            </h2>
          </div>
          <div className="col-2">
            <JssText field={props.fields.Text} />
          </div>
          <div className="col-1">
            <JssLink field={props.fields.CallToAction} />
          </div>
        </div>
      </div>
    );
  }

  return <ActionBannerDefaultComponent {...props} />;
};
