import React from 'react';
import { Field, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import useSWR from 'swr';
import clsx from 'clsx';
import { Flex } from '@chakra-ui/react';
import { ComponentProps } from 'lib/component-props';

interface Fields {
  SessionizeUrl: Field<string>;
}

type SessionsProps = ComponentProps & {
  fields: Fields;
};

const SessionsDefaultComponent = (props: SessionsProps): JSX.Element => (
  <div className={`component promo ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Sessions</span>
    </div>
  </div>
);

export const SessionsComponent = (props: SessionsProps): JSX.Element => {
  const id = props.params.RenderingIdentifier || undefined;

  const fetcher = (url: string) => fetch(url).then((res) => res.text());
  const { data, error } = useSWR(
    props.fields.SessionizeUrl.value ? props.fields.SessionizeUrl.value : null,
    fetcher
  );

  if (!props?.fields?.SessionizeUrl?.value) {
    return <SessionsDefaultComponent {...props} />;
  }

  //TODO: design error
  if (error) {
    return <div>Failed to load...</div>;
  }

  //TODO: design loading
  if (!data) {
    return <div>Loading</div>;
  }

  return (
    <Flex>
      <div className={clsx('component', 'sessions', props.params.styles)} id={id}>
        <div className="component-content">
          <div dangerouslySetInnerHTML={{ __html: data as string }} />
        </div>
      </div>
    </Flex>
  );
};

export const Default = withDatasourceCheck()<SessionsProps>(SessionsComponent);
