import React from 'react';
import { Box, Heading, SimpleGrid } from '@chakra-ui/react';
import { Field } from '@sitecore-jss/sitecore-jss-nextjs';
import { PersonFields, PersonProps, Default as Person } from '../Basic Components/Person';

// Define the type of props that People Grid will accept
interface Fields {
  /** Headline */
  Headline: Field<string>;

  /** Multilist of People */
  People: Array<Person>;
}

// Define the type of props for an Person
interface Person {
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

export type PeopleGridProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: PeopleGridProps): JSX.Element => {
  const styles = props.params && props.params.Styles ? props.params.Styles : '';
  const cols = props.params && props.params.Columns ? parseInt(props.params.Columns) : 4;

  return (
    <Box w="100%" mt={20} className={styles}>
      <Box w="80%" pt={10} m="auto">
        {props.fields?.Headline?.value !== '' && (
          <Heading as="h2" size="lg">
            {props.fields?.Headline?.value}
          </Heading>
        )}
        <SimpleGrid columns={{ base: 1, md: cols }} mt={10}>
          {props.fields?.People.map((person, idx) => {
            const pp: PersonProps = {
              params: props.params,
              fields: person.fields,
            };
            return <Person key={idx} {...pp}></Person>;
          })}
        </SimpleGrid>
      </Box>
    </Box>
  );
};
