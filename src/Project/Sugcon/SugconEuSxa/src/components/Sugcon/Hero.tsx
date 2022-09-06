import React from 'react';
import { Text, Field, ImageField, Image } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

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
      <div className={`component hero ${props.params.styles}`}>
        <div className="component-content" id="box-search">
          <div className="thumbnail">
            <Image media={props.fields.BackgroundImage} className="img-fluid heroimage"></Image>
            <div className="caption d-none d-md-block">
              <p className="hero-label">
                <Text field={props.fields.Label} />
              </p>
              <p className="h1 py-4">
                <Text className="contentTitle" field={props.fields.Text} />
              </p>
            </div>
          </div>
        </div>
      </div>
    );
  }
  return <Hero {...props} />;
};
