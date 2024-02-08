import React from 'react';
import { Box, Heading, Flex } from '@chakra-ui/react';
import { Field } from '@sitecore-jss/sitecore-jss-nextjs';
import { Fields as EventFields, Default as Event } from './Event';

// Define the type of props that Event Teaser will accept
interface Fields {
  /** Headline */
  Headline: Field<string>;

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
  console.log('testing');
  return (
    <Box w="80%" mx="auto" mt={20}>
      <Heading as="h2" size="lg">
        {props.fields?.Headline?.value}
      </Heading>
      <Flex flexWrap="wrap" mt={10}>
        {props.fields?.Events.map((event, idx) => {
          return <Event key={idx} {...event.fields}></Event>;
        })}
      </Flex>
    </Box>
  );
};
