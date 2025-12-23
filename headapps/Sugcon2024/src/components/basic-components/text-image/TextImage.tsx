import React, { JSX } from 'react';
import { Heading, Text, Image, Flex, Container } from '@chakra-ui/react';
import {
  TextField,
  Text as JssText,
  RichTextField,
  RichText as JssRichText,
  ImageField,
  Image as JssImage,
  withDatasourceCheck,
} from '@sitecore-content-sdk/nextjs';
import { LayoutFlex } from 'components/page-structure/layout-flex/LayoutFlex';
import { ComponentProps } from 'lib/component-props';

// Define the type of props that TextImage will accept
interface Fields {
  /** Title of the TextImage */
  Headline: TextField;

  /** Richtext of the TextImage */
  Text: RichTextField;

  /** Image of the TextImage */
  Image: ImageField;
}

export type TextImageProps = ComponentProps & {
  fields: Fields;
};

const TextImageComponent = (props: TextImageProps): JSX.Element => {
  return (
    <LayoutFlex flexWrap="wrap">
      <Container minW={{ base: '100%', lg: '50%' }} flex="1" pr={{ base: '0', lg: '120' }} pl="0">
        <Heading as="h2" fontSize="3xl" fontWeight="bold" mb="33px">
          <JssText field={props.fields.Headline} />
        </Heading>
        <Text as={JssRichText} mb={6} fontSize="18px" field={props.fields.Text} />
      </Container>

      <Flex minW={{ base: '100%', lg: '50%' }} flex="1">
        <Image
          as={JssImage}
          src={props.fields.Image?.value?.src}
          alt={props.fields.Image?.value?.alt as string}
          width="400px"
          height="100%"
          w="100%"
          h="auto"
          borderRadius={15}
          margin="0 auto"
          field={props.fields.Image}
        />
      </Flex>
    </LayoutFlex>
  );
};

export const Default = withDatasourceCheck()<TextImageProps>(TextImageComponent);
