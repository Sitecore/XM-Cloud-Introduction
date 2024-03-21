import React from 'react';
import {
  TextField,
  Text as JssText,
  RichTextField,
  RichText as JssRichText,
} from '@sitecore-jss/sitecore-jss-nextjs';
import {
  Accordion,
  AccordionIcon,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  Box,
  Heading,
} from '@chakra-ui/react';
import { isEditorActive } from '@sitecore-jss/sitecore-jss-nextjs/utils';

interface AccordionElement {
  fields: {
    /** Title of an accordion item */
    Title: TextField;

    /** Text of an accordion item */
    Text: RichTextField;
  };
}
// Define the type of props that Accordion will accept
interface Fields {
  /** Headline of the accordion */
  Headline: TextField;

  /** Multilist containing accordion elements */
  Elements: Array<AccordionElement>;
}

interface AccordionProps {
  params: { [key: string]: string };
  fields: Fields;
}

export const Default = (props: AccordionProps): JSX.Element => {
  return (
    <Box w="100%" maxW="1366px" m="auto" pt={20}>
      {(isEditorActive() || props.fields?.Headline?.value !== '') && (
        <Heading size="lg" mb={4}>
          <JssText field={props.fields.Headline} />
        </Heading>
      )}
      <Accordion
        allowMultiple
        borderColor="sugcon.500"
        defaultIndex={isEditorActive() ? Array.from(props.fields.Elements.keys()) : []}
      >
        {props.fields.Elements?.length
          ? props.fields.Elements.map((element, index) => (
              <AccordionItem key={index}>
                <AccordionButton
                  _expanded={{ bg: 'sugcon.blue', color: 'white' }}
                  px={2}
                  py={4}
                  color="sugcon.blue"
                >
                  <Box flex="1" textAlign="left" fontWeight="semibold">
                    <JssText field={element.fields.Title} />
                  </Box>
                  <AccordionIcon fontSize="2em" ml={4} />
                </AccordionButton>
                <AccordionPanel color="sugcon.gray.4">
                  <JssRichText field={element.fields.Text} />
                </AccordionPanel>
              </AccordionItem>
            ))
          : null}
      </Accordion>
    </Box>
  );
};
