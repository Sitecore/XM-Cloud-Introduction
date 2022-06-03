import React from 'react';
import { Text, Field, ImageField } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';
import Image from 'next/image';

interface Fields {
  Text: Field<string>;
  Label: Field<string>;
  BackgroundImage: ImageField;
}

type HeroProps = ComponentProps & {
  params: { [key: string]: string };
  fields: Fields;
};

const Hero = (props: HeroProps): JSX.Element => (
  <div className={`component hero ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Hero Default view</span>
    </div>
  </div>
);

export const Default = (props: HeroProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className="mb-3 d-flex justify-content-center">
        <Image
          src="https://mvp-cd.sitecore.com/-/media/Sugcon/EU/budapest-view.jpg?h=900&iar=0&w=1600&hash=743F1E41D91667BF68372E39A71AEBB3"
          width="1600"
          height="900"
        />
        <div className="caption d-none d-lg-block">
          <p className="hero-label">
            <Text field={props.fields.Label} />
          </p>
          <p className="h1 py-4">
            <Text className="contentTitle" field={props.fields.Text} />
          </p>
        </div>
      </div>
    );
  }
  return <Hero {...props} />;
};
