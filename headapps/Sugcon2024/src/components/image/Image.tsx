import {
  Field,
  ImageField,
  NextImage as ContentSdkImage,
  Link as ContentSdkLink,
  LinkField,
  Text,
} from "@sitecore-content-sdk/nextjs";
import React from "react";
import { ComponentProps } from "lib/component-props";

interface ImageFields {
  Image: ImageField;
  ImageCaption: Field<string>;
  TargetUrl: LinkField;
}

interface ImageProps extends ComponentProps {
  fields: ImageFields;
}

const ImageWrapper: React.FC<{
  className: string;
  id?: string;
  children: React.ReactNode;
}> = ({ className, id, children }) => (
  <div className={className.trim()} id={id}>
    <div className="component-content">{children}</div>
  </div>
);

const ImageDefault: React.FC<ImageProps> = ({ params }) => (
  <ImageWrapper className={`component image ${params.styles}`}>
    <span className="is-empty-hint">Image</span>
  </ImageWrapper>
);

export const Banner: React.FC<ImageProps> = ({ params, fields }) => {
  const { styles, RenderingIdentifier: id } = params;
  const imageField = fields.Image && {
    ...fields.Image,
    value: {
      ...fields.Image.value,
      style: { objectFit: "cover", width: "100%", height: "100%" },
    },
  };

  return (
    <div className={`component hero-banner ${styles}`.trim()} id={id}>
      <div className="component-content sc-sxa-image-hero-banner">
        <ContentSdkImage
          field={imageField}
          loading="eager"
          fetchPriority="high"
        />
      </div>
    </div>
  );
};

export const Default: React.FC<ImageProps> = (props) => {
  const { fields, params, page } = props;
  const { styles, RenderingIdentifier: id } = params;

  if (!fields) {
    return <ImageDefault {...props} />;
  }

  const Image = () => <ContentSdkImage field={fields.Image} />;
  const shouldWrapWithLink =
    !page.mode.isEditing && fields.TargetUrl?.value?.href;

  return (
    <ImageWrapper className={`component image ${styles}`} id={id}>
      {shouldWrapWithLink ? (
        <ContentSdkLink field={fields.TargetUrl}>
          <Image />
        </ContentSdkLink>
      ) : (
        <Image />
      )}
      <Text
        tag="span"
        className="image-caption field-imagecaption"
        field={fields.ImageCaption}
      />
    </ImageWrapper>
  );
};
