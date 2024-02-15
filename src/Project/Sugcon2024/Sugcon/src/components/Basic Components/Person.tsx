import React from 'react';
import { Box, Heading, Text, Image, Link } from '@chakra-ui/react';
import { Field, ImageField, LinkField } from '@sitecore-jss/sitecore-jss-nextjs';

// Define the type of props that Hero will accept
export interface PersonFields {
  /** URL for the image */
  Image: ImageField;

  /** Person's full name */
  Name: Field<string>;

  /** Person's job role */
  JobRole: Field<string>;

  /** Person's company */
  Company: Field<string>;

  /** Person's Biography */
  Biography: Field<string>;

  /** Linkedin Link */
  Linkedin: LinkField;

  /** Twitter Link */
  Twitter: LinkField;
}

export type PersonProps = {
  params: { [key: string]: string };
  fields: PersonFields;
};

export const Default = (props: PersonProps): JSX.Element => {
  return (
    <Box mb={30}>
      <Image src={props.fields.Image?.value?.src} w={200} borderRadius={15} mb={10} />
      <Heading as="h3" size="md" mt={2}>
        {props.fields.Name?.value}
      </Heading>
      <Text fontSize="12px" mb={0}>
        {props.fields.JobRole?.value}
      </Text>
      <Text fontSize="12px" mb={0}>
        {props.fields.Company?.value}
      </Text>
      {props.params?.DisplaySocialLinks == '1' && props.fields.Linkedin?.value?.href !== '' && (
        <Link
          href={props.fields.Linkedin?.value?.href}
          isExternal={props.fields.Linkedin?.value?.target == '_blank'}
          fontSize="12px"
          mt={3}
          textDecoration="underline"
          color="#28327D"
        >
          Linkedin
        </Link>
      )}
      {props.params?.DisplaySocialLinks == '1' &&
        props.fields.Linkedin?.value?.href !== '' &&
        props.fields.Twitter?.value?.href !== '' && <Box display="inline"> / </Box>}
      {props.params?.DisplaySocialLinks == '1' && props.fields.Twitter?.value?.href !== '' && (
        <Link
          href={props.fields.Twitter?.value?.href}
          isExternal={props.fields.Twitter?.value?.target == '_blank'}
          fontSize="12px"
          mt={3}
          textDecoration="underline"
          color="#28327D"
        >
          Twitter
        </Link>
      )}
    </Box>
  );
};
