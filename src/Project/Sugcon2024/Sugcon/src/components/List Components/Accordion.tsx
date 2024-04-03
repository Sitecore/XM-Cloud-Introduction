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
import { LayoutFlex } from 'components/Templates/LayoutFlex';

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
    <LayoutFlex direction="column">
      {(isEditorActive() || props.fields?.Headline?.value !== '') && (
        <Heading size="lg" mb={4}>
          <JssText field={props.fields.Headline} />
        </Heading>
      )}
      <Accordion
        allowMultiple
        defaultIndex={isEditorActive() ? Array.from(props.fields.Elements.keys()) : []}
      >
        {props.fields.Elements?.length
          ? props.fields.Elements.map((element, index) => (
              <AccordionItem key={index}>
                <AccordionButton>
                  <Box flex="1" textAlign="left" fontWeight="semibold">
                    <JssText field={element.fields.Title} />
                  </Box>
                  <AccordionIcon />
                </AccordionButton>
                <AccordionPanel>
                  <JssRichText field={element.fields.Text} />
                </AccordionPanel>
              </AccordionItem>
            ))
          : null}
      </Accordion>
    </LayoutFlex>
  );
};
