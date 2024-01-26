import React from 'react';
import { Box, Heading, Text, Button, Image, Flex, Link } from '@chakra-ui/react';
import { Field, ImageField, LinkField } from '@sitecore-jss/sitecore-jss-nextjs';

// Define the type of props that Hero will accept
interface Fields {
  /** Title of the event banner */
  Title: Field<string>;

  /** Date of the event */
  Date: Field<string>;

  /** Description of the event */
  Description: Field<string>;

  /** URL for the event image */
  Image: ImageField;

  /** Link to trigger when the button is clicked */
  CallToAction: LinkField;
}

type HeroProps = {
  params: { [key: string]: string };
  fields: Fields;
};

export const Default = (props: HeroProps): JSX.Element => {
  return (
    <Flex
      direction={{ base: 'column', md: 'row' }}
      alignItems="center"
      bg="#f0f0f0"
      w="100vw"
      boxShadow="-20px 19px 40px 0px rgba(0, 0, 0, 0.2) inset"
      maxHeight="400px"
    >
      <Flex
        direction="column"
        margin="0 auto" // Center the content box
        p={5}
        flexGrow={1}
        minWidth="50%"
      >
        <Box width="auto" alignSelf="end" maxWidth="620px">
          <Heading as="h2" fontSize="30px" fontWeight="bold" mb="33px">
            {props.fields.Title.value}
          </Heading>
          {props.fields.Date?.value !== '' && (
            <Text fontSize="18px" mb={3}>
              {props.fields.Date.value}
            </Text>
          )}
          {props.fields.Description?.value !== '' && (
            <Text mb={5} fontSize="18px">
              {props.fields.Description.value}
            </Text>
          )}
          {props.fields.CallToAction?.value?.href !== '' && (
            <Box width="auto" alignSelf="start">
              <Link
                href={props.fields.CallToAction?.value?.href}
                isExternal={props.fields.CallToAction?.value?.target == '_blank'}
              >
                <Button colorScheme="red" size="lg" borderRadius="full">
                  {props.fields.CallToAction?.value?.text}
                </Button>
              </Link>
            </Box>
          )}
        </Box>
      </Flex>
      <Box flex="1" position="relative" minWidth="50%" maxHeight="400px">
        {' '}
        <Image
          src={props.fields.Image?.value?.src}
          //alt={props.fields.Image?.value?.alt}
          width="full"
          height="100%"
          maxHeight="400px"
          objectFit="cover"
        />
      </Box>
    </Flex>
  );
};
