import React from 'react';
import { Box, Heading, Text, Flex } from '@chakra-ui/react';
import { Field } from '@sitecore-jss/sitecore-jss-nextjs';

// Define the type of props that VideoText will accept
interface Fields {
  /** Title of the VideoText */
  Headline: Field<string>;

  /** Youtube Video Id (not the full youtube.com url)*/
  YoutubeVideoId: Field<string>;

  /** Text headline of the VideoText */
  TextHeadline: Field<string>;

  /** Richtext of the VideoText */
  Text: Field<string>;
}

export type VideoTextProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: VideoTextProps): JSX.Element => {
  return (
    <Box w={{ base: '100vw', md: '80vw' }} my="20" mx={{ base: '20px', md: 'auto' }}>
      {props.fields.Headline?.value !== '' && (
        <Heading as="h2" fontSize="30px" fontWeight="bold" mb="33px">
          {props.fields.Headline?.value}
        </Heading>
      )}

      <Flex direction={{ base: 'column', md: 'row' }} flexGrow={1} columnGap="20" rowGap="10">
        <Box w={{ base: '100%', md: '50%' }} position="relative">
          <iframe
            width="100%"
            height="100%"
            src={`https://www.youtube.com/embed/${props.fields.YoutubeVideoId?.value}`}
            title="YouTube video player"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
            allowFullScreen
            style={{ borderRadius: '15px', position: 'absolute' }}
          ></iframe>
          <Box pt="56.25%" display="block">
            {' '}
          </Box>
        </Box>
        <Box w={{ base: '100%', md: '50%' }}>
          {props.fields.TextHeadline?.value !== '' && (
            <Heading as="h3" fontWeight="bold" mb={{ base: '10px', md: '20px' }}>
              {props.fields.TextHeadline?.value}
            </Heading>
          )}
          {props.fields.Text?.value !== '' && (
            <Text mb={6} fontSize="18px">
              {props.fields.Text?.value}
            </Text>
          )}
        </Box>
      </Flex>
    </Box>
  );
};
