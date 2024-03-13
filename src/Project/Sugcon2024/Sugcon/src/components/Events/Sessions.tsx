import React from 'react';
import { Field } from '@sitecore-jss/sitecore-jss-nextjs';
import useSWR from 'swr';

interface Fields {
  SessionizeUrl: Field<string>;
}

type SessionsProps = {
  params: { [key: string]: string };
  fields: Fields;
};

const SessionsDefaultComponent = (props: SessionsProps): JSX.Element => (
  <div className={`component promo ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Sessions</span>
    </div>
  </div>
);

export const Default = (props: SessionsProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  if (!props?.fields?.SessionizeUrl?.value) {
    return <SessionsDefaultComponent {...props} />;
  }

  //Question: linter is saying the hook shouldn't be called conditionally, but that's correct in this case?
  //eslint-disable-next-line
  const { data, error } = useSWR(props.fields.SessionizeUrl.value, () => fetch(props.fields.SessionizeUrl.value).then((response) => response.text()));

  //TODO: design error
  if (error) {
    return <div>Failed to load...</div>;
  }

  //TODO: design loading
  if (!data) {
    return <div>Loading</div>;
  }

  return (
    <div className={`component sessions ${props.params.styles}`} id={id ? id : undefined}>
      <div className="component-content">
        <div dangerouslySetInnerHTML={{ __html: data as string }} />
      </div>
    </div>
  );
};
