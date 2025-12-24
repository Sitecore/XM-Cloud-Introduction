import React, { JSX } from 'react';
import {
  NextImage as ContentSdkImage,
  Link as ContentSdkLink,
  ImageField,
  Field,
  LinkField,
  Text,
  useSitecore,
} from '@sitecore-content-sdk/nextjs';
import { Box, Flex } from '@chakra-ui/react';

interface Fields {
  Image: ImageField;
  ImageCaption: Field<string>;
  TargetUrl: LinkField;
}

type LogoProps = {
  params: { [key: string]: string };
  fields: Fields;
};

const LogoDefault = (props: LogoProps): JSX.Element => (
  <div className={`component image ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Logo</span>
    </div>
  </div>
);

export const Banner = (props: LogoProps): JSX.Element => {
  const { page } = useSitecore();
  const classHeroBannerEmpty =
  page.mode.isEditing && props.fields?.Image?.value?.class === 'scEmptyImage'
      ? 'hero-banner-empty'
      : '';
  const backgroundStyle = { backgroundImage: `url('${props?.fields?.Image?.value?.src}')` };
  const modifyImageProps = {
    ...props.fields.Image,
    value: {
      ...props.fields.Image.value,
      width: props.fields.Image.value?.width ?? "100",
      height: props.fields.Image.value?.height ?? "100",
    }
  };
const id = props?.params?.RenderingIdentifier || undefined;

  return (
    <div
      className={`component hero-banner ${props?.params?.styles} ${classHeroBannerEmpty}`}
      id={id}
    >
      <div className="component-content sc-sxa-image-hero-banner" style={backgroundStyle}>
        {page.mode.isEditing ? <ContentSdkImage field={modifyImageProps} /> : ''}
      </div>
    </div>
  );
};

export const Default = (props: LogoProps): JSX.Element => {
  const { page } = useSitecore();

  if (props.fields) {
    const id = props?.params?.RenderingIdentifier || undefined;
    const modifyImageProps = {
      ...props.fields.Image,
      value: {
        ...props.fields.Image.value,
        width: props.fields.Image.value?.width ?? "100",
        height: props.fields.Image.value?.height ?? "100",
      }
    };

    return (
      <Box
        className={`component image header-image ${props?.params?.styles}`}
        id={id}
        px="0"
        mr="15px"
      >
        <Flex w={{ base: 155, lg: 190 }}>
          {page.mode.isEditing || !props?.fields?.TargetUrl?.value?.href ? (
            <ContentSdkImage field={modifyImageProps} width="100" height="100" />
          ) : (
            <ContentSdkLink field={props?.fields?.TargetUrl}>
              <ContentSdkImage field={modifyImageProps} />
            </ContentSdkLink>
          )}
        </Flex>
        <Text
          tag="span"
          className="image-caption field-imagecaption"
          field={props?.fields?.ImageCaption}
        />
      </Box>
    );
  }

  return <LogoDefault {...props} />;
};
