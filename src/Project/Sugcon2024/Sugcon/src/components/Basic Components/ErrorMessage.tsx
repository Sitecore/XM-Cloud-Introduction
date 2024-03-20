import React from 'react';
import { Box, Heading, Text, Flex } from '@chakra-ui/react';
import {
  TextField,
  Text as JssText,
  RichTextField,
  RichText as JssRichText,
} from '@sitecore-jss/sitecore-jss-nextjs';

// Define the type of props that ErrorMessage will accept
interface Fields {
  /** Headline of the ErrorMessage */
  Headline: TextField;

  /** Richtext of the ErrorMessage */
  Text: RichTextField;

  /** StatusCode of the ErrorMessage */
  StatusCode: TextField;
}

export type ErrorMessageProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: ErrorMessageProps): JSX.Element => {
  return (
    <Flex
      direction={{ base: 'column', md: 'row' }}
      my="20"
      mx={{ base: '20px', md: 'auto' }}
      columnGap={20}
    >
      <Box
        fontSize="80px"
        fontWeight="bold"
        textColor="red"
        textAlign={'center'}
        p="20px"
        mb={{ base: '20px', md: '0' }}
        backgroundImage="url('/red-speech-bubble.svg')"
        backgroundRepeat="no-repeat"
        backgroundPosition="center"
        minW={'216px'}
        minH={'217px'}
      >
        <Text as={JssText} field={props.fields.StatusCode} />
      </Box>
      <Box flex="1" maxW={{ md: '400px' }} alignSelf="center">
        <Heading as="h2" fontSize="30px" fontWeight="bold" mb="10px" textColor="red">
          <JssText field={props.fields.Headline} />
        </Heading>
        <Text as={JssRichText} mb={6} fontSize="18px" field={props.fields.Text} />
      </Box>
    </Flex>
  );
};
