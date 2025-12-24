import React, { JSX } from 'react';
import {
  Image as ContentSdkImage,
  Link as ContentSdkLink,
  ImageField,
  Field,
  LinkField,
  Text,
  useSitecore,
} from '@sitecore-content-sdk/nextjs';

interface ImageFields {
  Image: ImageField;
  ImageCaption: Field<string>;
  TargetUrl: LinkField;
}

type ImageProps = {
  params: { [key: string]: string };
  fields: ImageFields;
};

const ImageDefault = (props: ImageProps): JSX.Element => (
  <div className={`component image ${props.params.styles}`.trimEnd()}>
    <div className="component-content">
      <span className="is-empty-hint">Image</span>
    </div>
  </div>
);

export const Banner = (props: ImageProps): JSX.Element => {
  const { page } = useSitecore();
  const classHeroBannerEmpty =
    page.mode.isEditing && props.fields?.Image?.value?.class === 'scEmptyImage'
      ? 'hero-banner-empty'
      : '';
  const backgroundStyle = { backgroundImage: `url('${props?.fields?.Image?.value?.src}')` };
  const modifyImageProps = {
    ...props.fields.Image,
    width: props.fields.Image.value?.width ?? "100%",
    height: props.fields.Image.value?.height ?? "100%",
  };
  const id = props.params.RenderingIdentifier;

  return (
    <div
      className={`component hero-banner ${props.params.styles} ${classHeroBannerEmpty}`}
      id={id ? id : undefined}
    >
      <div className="component-content sc-sxa-image-hero-banner" style={backgroundStyle}>
        {page.mode.isEditing ? <ContentSdkImage field={modifyImageProps} /> : ''}
      </div>
    </div>
  );
};

export const Default = (props: ImageProps): JSX.Element => {
  const { page } = useSitecore();

  if (props.fields) {
    const id = props.params.RenderingIdentifier;

    return (
      <div className={`component image ${props.params.styles}`} id={id ? id : undefined}>
        <div className="component-content">
          {page.mode.isEditing || !props.fields.TargetUrl?.value?.href ? (
            <ContentSdkImage field={props.fields.Image} />
          ) : (
            <ContentSdkLink field={props.fields.TargetUrl}>
              <ContentSdkImage field={props.fields.Image} />
            </ContentSdkLink>
          )}
          <Text
            tag="span"
            className="image-caption field-imagecaption"
            field={props.fields.ImageCaption}
          />
        </div>
      </div>
    );
  }

  return <ImageDefault {...props} />;
};
