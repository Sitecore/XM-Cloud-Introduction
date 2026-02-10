import React, { JSX } from 'react';
import { Heading, Box, Flex, Container } from '@chakra-ui/react';
import {
  TextField,
  Text as ContentSdkText,
  RichTextField,
  RichText as ContentSdkRichText,
  ImageField,
  Image as ContentSdkImage
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
          <ContentSdkText field={props.fields.Headline} />
        </Heading>
        <Box mb={6} fontSize="18px" className="rich-text-content">
          <ContentSdkRichText field={props.fields.Text} />
        </Box>
      </Container>

      <Flex minW={{ base: '100%', lg: '50%' }} flex="1">
        <Box w="100%" borderRadius={15} overflow="hidden" margin="0 auto">
          <ContentSdkImage
            field={props.fields.Image}
            style={{ width: '100%', height: 'auto' }}
          />
        </Box>
      </Flex>
    </LayoutFlex>
  );
};

export const Default = TextImageComponent;
