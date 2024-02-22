import React from 'react';
import { ComponentParams } from '@sitecore-jss/sitecore-jss-nextjs';
import { Field, RichText } from '@sitecore-jss/sitecore-jss-nextjs';
import {
  Accordion,
  AccordionIcon,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  Box,
  Heading,
  Text,
} from '@chakra-ui/react';
import { isSitecoreTextFieldPopulated } from 'lib/utils/sitecoreUtils';

interface Element {
  fields: {
    /** Title of an accordion item */
    Title: Field<string>;

    /** Text of an accordion item */
    Text: Field<string>;
  };
}
// Define the type of props that Hero will accept
interface Fields {
  /** Title of the event banner */
  Title: Field<string>;

  /** Multilist containing accordion item elements */
  AccordionElementList: Element[];
}

interface AccordionProps {
  params: ComponentParams;
  fields: Fields;
}

export const Default = (props: AccordionProps): JSX.Element => {
  const { Title, AccordionElementList } = props.fields;

  return (
    <Box>
      {isSitecoreTextFieldPopulated(Title) && (
        <Heading size="lg" mb={4}>
          {Title.value}
        </Heading>
      )}
      <Accordion allowMultiple allowToggle borderColor="sugcon.500">
        {AccordionElementList?.length
          ? AccordionElementList.map((element, index) => (
              <AccordionItem key={index}>
                <AccordionButton
                  _expanded={{ bg: 'sugcon.blue', color: 'white' }}
                  px={2}
                  py={4}
                  color="sugcon.blue"
                >
                  <Box flex="1" textAlign="left" fontWeight="semibold">
                    {isSitecoreTextFieldPopulated(element.fields.Title) && (
                      <Text>{element.fields.Title.value}</Text>
                    )}
                  </Box>
                  <AccordionIcon fontSize="2em" ml={4} />
                </AccordionButton>
                <AccordionPanel color="sugcon.gray.4">
                  {isSitecoreTextFieldPopulated(element.fields.Text) && (
                    <RichText field={element.fields.Text} />
                  )}
                </AccordionPanel>
              </AccordionItem>
            ))
          : null}
      </Accordion>
    </Box>
  );
};
