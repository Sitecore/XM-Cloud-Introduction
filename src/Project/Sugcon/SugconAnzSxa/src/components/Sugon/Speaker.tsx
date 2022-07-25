import React from 'react';
import {
  withDatasourceCheck,
  GetStaticComponentProps,
  useComponentProps,
  Text,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { fetchSessionizeData } from 'lib/sessionize/fetch-sessonize-data';
import { ComponentData } from 'lib/sessionize/sessionizeData';

const Speaker = (): JSX.Element => (
  <div>
    <p>Speaker Component</p>
  </div>
);

export const getStaticProps: GetStaticComponentProps = async (rendering) => {
  const sessionizeSpeakerUrl = rendering?.fields?.SessionizeURL.value;
  return await fetchSessionizeData(sessionizeSpeakerUrl);
};

export const Default = (props: ComponentData): JSX.Element => {
  const externalData = useComponentProps<string>(props.rendering.uid);

  return (
    <div className="container component">
      <h1 className="p-3">
        <Text field={props?.fields?.Title} />
      </h1>
      <div dangerouslySetInnerHTML={{ __html: externalData as string }} />
    </div>
  );
};

export default withDatasourceCheck()(Speaker);
