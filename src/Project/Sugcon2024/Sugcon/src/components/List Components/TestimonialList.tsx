import React from 'react';
import { Box } from '@chakra-ui/react';
// import { Text as JssText } from '@sitecore-jss/sitecore-jss-nextjs';
import { Person } from './PeopleGrid';
import clsx from 'clsx';
// import { LayoutFlex } from 'components/Templates/LayoutFlex';

// Define the type of props that Testimonial List will accept
interface Fields {
  /** Testimonial */
  Testimonials: Array<Testimonial>;
}

// Define the type of props for an Person
interface Testimonial {
  displayName: string;

  fields: {
    Person: Person;
    Testimonial: {
      value: string;
    };
  };

  /** The item id of the testimonial item */
  id: string;

  /** Name of the person item */
  name: string;

  /** Url of the person item */
  url: string;
}

export type TestimonialListProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: TestimonialListProps): JSX.Element => {
  // const cols = props.params && props.params.Columns ? parseInt(props.params.Columns) : 4;

  console.log(props);

  return (
    <Box w="100%" mt={20} className={clsx('testimonial-list-container', props?.params?.Styles)}>
      {props.fields.Testimonials.map((testimonial) => (
        <Box
          key={testimonial.id}
          p={6}
          bg="white"
          boxShadow="md"
          borderRadius="md"
          display="flex"
          flexDirection="column"
          alignItems="center"
        >
          <img
            src={testimonial.fields.Person.fields.Image.value?.src}
            alt=""
            width={80}
            height={80}
          />
          <p>{testimonial.fields.Testimonial.value}</p>
          <Box mt={2} textAlign="center">
            <p>{testimonial.fields.Person.fields.Name.value}</p>
            <p>{testimonial.fields.Person.fields.JobRole.value}</p>
          </Box>
        </Box>
      ))}
    </Box>
  );
};
