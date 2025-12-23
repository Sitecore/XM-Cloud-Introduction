import React, { JSX } from 'react';
import {
  TextField,
  Text as JssText,
  RichTextField,
  RichText as JssRichText,
  withDatasourceCheck,
  useSitecore,
} from '@sitecore-content-sdk/nextjs';
import {
  Accordion,
  AccordionIcon,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  Box,
  Heading,
} from '@chakra-ui/react';
import { LayoutFlex } from 'components/page-structure/layout-flex/LayoutFlex';
import { ComponentProps } from 'lib/component-props';

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

type AccordionProps = ComponentProps & {
  fields: Fields;
};

const AccordionComponent = (props: AccordionProps): JSX.Element => {
  const { page } = useSitecore();
  return (
    <LayoutFlex direction="column">
      {(page.mode.isEditing || props.fields?.Headline?.value !== '') && (
        <Heading size="lg" mb={4}>
          <JssText field={props.fields.Headline} />
        </Heading>
      )}
      <Accordion
        allowMultiple
        defaultIndex={page.mode.isEditing ? Array.from(props.fields.Elements.keys()) : []}
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

export const Default = withDatasourceCheck()<AccordionProps>(AccordionComponent);
