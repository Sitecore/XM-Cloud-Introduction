import React from 'react';
import { Box, Heading, Flex } from '@chakra-ui/react';
import { TextField, Text as JssText } from '@sitecore-jss/sitecore-jss-nextjs';
import { Fields as EventFields, Default as Event } from './Event';
import { isEditorActive } from '@sitecore-jss/sitecore-jss-nextjs/utils';

// Define the type of props that Event Teaser will accept
interface Fields {
  /** Headline */
  Headline: TextField;

  /** Multilist of Events */
  Events: Array<Event>;
}

// Define the type of props for an Event
interface Event {
  /** Display name of the event item */
  displayName: string;

  /** The details of an event */
  fields: EventFields;

  /** The item id of the event item */
  id: string;

  /** Name of the event item */
  name: string;

  /** Url of the event item */
  url: string;
}

export type EventTeaserProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: EventTeaserProps): JSX.Element => {
  return (
    <Box w="80%" mx="auto" mt={20}>
      {(isEditorActive() || props.fields?.Headline?.value !== '') && (
        <Heading as="h2" size="lg">
          <JssText field={props.fields.Headline} />
        </Heading>
      )}
      <Flex flexWrap="wrap" mt={10}>
        {props.fields?.Events.map((event, idx) => {
          return <Event key={idx} {...event.fields}></Event>;
        })}
      </Flex>
    </Box>
  );
};
