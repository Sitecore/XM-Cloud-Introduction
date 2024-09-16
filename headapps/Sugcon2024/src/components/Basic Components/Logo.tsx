import React from 'react';
import {
  Image as JssImage,
  Link as JssLink,
  ImageField,
  Field,
  LinkField,
  Text,
  useSitecoreContext,
} from '@sitecore-jss/sitecore-jss-nextjs';
import clsx from 'clsx';
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
  <div className={clsx('component', 'image', props.params.styles)}>
    <div className="component-content">
      <span className="is-empty-hint">Logo</span>
    </div>
  </div>
);

export const Banner = (props: LogoProps): JSX.Element => {
  const { sitecoreContext } = useSitecoreContext();
  const isPageEditing = sitecoreContext.pageEditing;
  const classHeroBannerEmpty =
    isPageEditing && props.fields?.Image?.value?.class === 'scEmptyImage'
      ? 'hero-banner-empty'
      : '';
  const backgroundStyle = { backgroundImage: `url('${props?.fields?.Image?.value?.src}')` };
  const modifyImageProps = {
    ...props.fields.Image,
    editable: props?.fields?.Image?.editable
      ?.replace(`width="${props?.fields?.Image?.value?.width}"`, 'width="100%"')
      .replace(`height="${props?.fields?.Image?.value?.height}"`, 'height="100%"'),
  };
  const id = props?.params?.RenderingIdentifier || undefined;

  return (
    <div
      className={clsx('component', 'hero-banner', props?.params?.styles, classHeroBannerEmpty)}
      id={id}
    >
      <div className="component-content sc-sxa-image-hero-banner" style={backgroundStyle}>
        {sitecoreContext.pageEditing ? <JssImage field={modifyImageProps} /> : ''}
      </div>
    </div>
  );
};

export const Default = (props: LogoProps): JSX.Element => {
  const { sitecoreContext } = useSitecoreContext();

  if (props.fields) {
    const Image = () => <JssImage field={props.fields.Image} />;
    const id = props?.params?.RenderingIdentifier || undefined;

    return (
      <Box
        className={clsx('component', 'image', 'header-image', props?.params?.styles)}
        id={id}
        px="0"
        mr="15px"
      >
        <Flex w={{ base: 155, lg: 190 }}>
          {sitecoreContext?.pageState === 'edit' || !props?.fields?.TargetUrl?.value?.href ? (
            <Image />
          ) : (
            <JssLink field={props?.fields?.TargetUrl}>
              <Image />
            </JssLink>
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
