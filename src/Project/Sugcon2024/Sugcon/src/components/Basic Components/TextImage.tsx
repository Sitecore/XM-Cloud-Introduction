import React from 'react';
import { Box, Heading, Text, Image, Flex } from '@chakra-ui/react';
import {
  TextField,
  Text as JssText,
  RichTextField,
  RichText as JssRichText,
  ImageField,
  Image as JssImage,
} from '@sitecore-jss/sitecore-jss-nextjs';

// Define the type of props that TextImage will accept
interface Fields {
  /** Title of the TextImage */
  Headline: TextField;

  /** Richtext of the TextImage */
  Text: RichTextField;

  /** Image of the TextImage */
  Image: ImageField;
}

export type TextImageProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: TextImageProps): JSX.Element => {
  return (
    <Flex
      direction={{ base: 'column', md: 'row' }}
      w="100vw"
      my="20"
      mx={{ base: '20px', md: '0' }}
    >
      <Flex
        direction="column"
        margin="0 auto" // Center the content box
        flexGrow={1}
        minWidth="50%"
      >
        <Box width="auto" alignSelf="end" maxWidth="620px" minWidth="360px">
          <Heading as="h2" fontSize="30px" fontWeight="bold" mb="33px">
            <JssText field={props.fields.Headline} />
          </Heading>
          <Text as={JssRichText} mb={6} fontSize="18px" field={props.fields.Text} />
        </Box>
      </Flex>
      <Box flex="1" position="relative" minWidth="50%" maxHeight="400px" alignItems="center">
        {' '}
        <Image
          as={JssImage}
          src={props.fields.Image?.value?.src}
          //alt={props.fields.Image?.value?.alt}
          width="400px"
          height="100%"
          borderRadius={15}
          margin="0 auto"
          field={props.fields.Image}
        />
      </Box>
    </Flex>
  );
};
