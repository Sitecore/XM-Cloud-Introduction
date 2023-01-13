import React from 'react';
import {
  Text,
  Field,
  ImageField,
  Image,
  RichText as JssRichText,
  LinkField,
  Link,
  useSitecoreContext,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

interface Fields {
  Text: Field<string>;
  Label: Field<string>;
  BackgroundImage: ImageField;
  Description: Field<string>;
  Link: LinkField;
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
  const { sitecoreContext } = useSitecoreContext();
  if (props.fields) {
    return (
      <div className={`component hero ${props.params.styles}`}>
        <div className="component-content" id="box-search">
          <div className="thumbnail">
            <Image media={props.fields.BackgroundImage} className="img-fluid heroimage"></Image>
            <div className="col-12">
              <div className="caption d-md-block hero-backdrop">
                <p className="hero-label">
                  <Text field={props.fields.Label} />
                </p>
                <h1 className="h1 py-4">
                  <Text className="contentTitle" field={props.fields.Text} />
                </h1>
                <div className="hero-text">
                  <JssRichText field={props.fields.Description} />
                  {sitecoreContext.pageState === 'edit' ? (
                    props.fields?.Link ? (
                      <Link className="link-button primary" field={props.fields.Link}></Link>
                    ) : (
                      ''
                    )
                  ) : (
                    props.fields?.Link?.value?.href ? (
                      <Link className="link-button primary" field={props.fields.Link}></Link>
                    ) : (
                      ''
                    )
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
  return <Hero {...props} />;
};
