import { Heading, Grid } from '@chakra-ui/react';
import { TextField, Text as JssText } from '@sitecore-jss/sitecore-jss-nextjs';
import { Fields as EventFields, Default as Event } from './Event';
import { LayoutFlex } from 'components/Templates/LayoutFlex';

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
    <LayoutFlex direction="column">
      <Heading as="h2" fontSize="3xl" width="full">
        <JssText field={props?.fields?.Headline} />
      </Heading>

      <Grid
        templateColumns={{ base: 'repeat(1, 1fr)', md: 'repeat(2, 1fr)', lg: 'repeat(4, 1fr)' }}
        autoRows="minmax(min-content, 1fr)"
        gap="30px"
      >
        {props?.fields?.Events.map((event, idx) => {
          return <Event key={idx} {...event?.fields}></Event>;
        })}
      </Grid>
    </LayoutFlex>
  );
};
