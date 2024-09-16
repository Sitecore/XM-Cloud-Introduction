import React from 'react';
import { Box, Heading, SimpleGrid } from '@chakra-ui/react';
import { TextField, Text as JssText, withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { PersonFields, PersonProps, Default as Person } from '../Basic Components/Person';
import clsx from 'clsx';
import { LayoutFlex } from 'components/Templates/LayoutFlex';
import { ComponentProps } from 'lib/component-props';

// Define the type of props that People Grid will accept
interface Fields {
  /** Headline */
  Headline: TextField;

  /** Multilist of People */
  People: Array<Person>;
}

// Define the type of props for an Person
export interface Person {
  /** Display name of the person item */
  displayName: string;

  /** The details of a person */
  fields: PersonFields;

  /** The item id of the person item */
  id: string;

  /** Name of the person item */
  name: string;

  /** Url of the person item */
  url: string;
}

export type PeopleGridProps = ComponentProps & {
  fields: Fields;
};

const PeopleGridComponent = (props: PeopleGridProps): JSX.Element => {
  const cols = props.params && props.params.Columns ? parseInt(props.params.Columns) : 4;

  if (props.params && props.params.Alphabetize == '1') {
    props.fields?.People.sort((a, b) =>
      a.fields?.Name?.value + '' > b.fields?.Name?.value + '' ? 1 : -1
    );
  }

  return (
    <Box w="100%" mt={20} className={clsx('people-grid-container', props?.params?.Styles)}>
      <LayoutFlex flexGrow="1" flexDirection="column" align="start">
        <Heading as={JssText} field={props.fields.Headline} tag="h2" size="lg" />

        <SimpleGrid
          w="full"
          columns={{ base: 1, md: Math.ceil(cols / 2), xl: cols }} // Using Math.ceil so odd numbers round up.
          mt={10}
          gap="10px"
          justifyItems="center"
        >
          {props.fields?.People.map((person, idx) => {
            const pp: PersonProps = {
              params: props.params,
              fields: person.fields,
              isPeopleGrid: true,
            };
            return <Person key={idx} {...pp}></Person>;
          })}
        </SimpleGrid>
      </LayoutFlex>
    </Box>
  );
};

export const Default = withDatasourceCheck()<PeopleGridProps>(PeopleGridComponent);
