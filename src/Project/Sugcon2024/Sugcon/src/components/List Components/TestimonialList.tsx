import React from 'react';
import { Box, Flex, Text, Image, Heading } from '@chakra-ui/react';
import clsx from 'clsx';
import { LayoutFlex } from 'components/Templates/LayoutFlex';

// Define the type of props that Testimonial List will accept
interface Fields {
  /** Testimonial */
  Testimonials: Array<Testimonial>;
}

// Define the type of props for a Person
interface Testimonial {
  displayName: string;

  fields: {
    Testimonial: {
      value: string;
    };
    Name: {
      value: string;
    };
    Role: {
      value: string;
    };
    Image: {
      value: {
        src: string;
      };
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
  return (
    <LayoutFlex
      direction="column"
      className={clsx('testimonial-list-container', props?.params?.Styles)}
    >
      <Heading as="h2" size="lg" mb={8}>
        What people say
      </Heading>
      <Flex direction={['column', 'column', 'row']} wrap="wrap" justify="space-between" gap={6}>
        {props.fields.Testimonials.map((testimonial) => (
          <Box
            key={testimonial.id}
            p={6}
            bg="#F2F2F2"
            borderRadius="20px"
            display="flex"
            flexDirection="column"
            alignItems="flex-start"
            maxW="sm"
            mx="auto"
          >
            <Flex alignItems="center" mb={4}>
              <Image
                src={testimonial.fields.Image.value?.src}
                alt={testimonial.fields.Name.value}
                borderRadius="full"
                boxSize="75px"
                mr={4}
              />
              <Flex direction="column" alignContent="center">
                <Text fontWeight="bold" fontSize="lg" mb={0}>
                  {testimonial.fields.Name.value},
                </Text>
                <Text fontWeight="bold" fontSize="lg" mb={0}>
                  {testimonial.fields.Role.value}
                </Text>
              </Flex>
            </Flex>
            <Text fontSize="lg" color="#4D4D4D" mt={2}>
              “{testimonial.fields.Testimonial.value}”
            </Text>
          </Box>
        ))}
      </Flex>
    </LayoutFlex>
  );
};
