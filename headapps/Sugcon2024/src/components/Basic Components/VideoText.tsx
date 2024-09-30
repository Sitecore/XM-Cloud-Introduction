import React from 'react';
import { Box, Heading, Text, Flex } from '@chakra-ui/react';
import {
  TextField,
  Text as JssText,
  RichTextField,
  RichText as JssRichText,
  withDatasourceCheck,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { isEditorActive } from '@sitecore-jss/sitecore-jss-nextjs/utils';
import { LayoutFlex } from 'components/Templates/LayoutFlex';
import { ComponentProps } from 'lib/component-props';

// Define the type of props that VideoText will accept
interface Fields {
  /** Title of the VideoText */
  Headline: TextField;

  /** Youtube Video Id (not the full youtube.com url)*/
  YoutubeVideoId: TextField;

  /** Text headline of the VideoText */
  TextHeadline: TextField;

  /** Richtext of the VideoText */
  Text: RichTextField;
}

export type VideoTextProps = ComponentProps & {
  fields: Fields;
};
const VideoTextComponent = (props: VideoTextProps): JSX.Element => {
  return (
    <Box w="100%">
      <LayoutFlex direction="column">
        {(isEditorActive() || props.fields.Headline?.value !== '') && (
          <Heading as="h2" fontSize="30px" fontWeight="bold" mb="33px">
            <JssText field={props.fields.Headline} />
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
            {(isEditorActive() || props.fields.TextHeadline?.value !== '') && (
              <Heading as="h3" fontWeight="bold" mb={{ base: '10px', md: '20px' }}>
                <JssText field={props.fields.TextHeadline} />
              </Heading>
            )}
            <Text as={JssRichText} mb={6} fontSize="18px" field={props.fields.Text} />
            {isEditorActive() && (
              <Box>
                {'Youtube Video Id: '}
                <JssText field={props.fields.YoutubeVideoId} />
              </Box>
            )}
          </Box>
        </Flex>
      </LayoutFlex>
    </Box>
  );
};

export const Default = withDatasourceCheck()<VideoTextProps>(VideoTextComponent);
