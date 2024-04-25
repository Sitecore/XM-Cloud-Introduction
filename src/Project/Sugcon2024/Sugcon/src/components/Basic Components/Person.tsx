import React from 'react';
import {
  Box,
  Flex,
  Heading,
  Text,
  Image,
  Link,
  Button,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalCloseButton,
  ModalFooter,
  useDisclosure,
} from '@chakra-ui/react';
import {
  ImageField,
  Image as JssImage,
  LinkField,
  Link as JssLink,
  TextField,
  Text as JssText,
  RichText as JssRichText,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { isEditorActive } from '@sitecore-jss/sitecore-jss-nextjs/utils';

// Define the type of props that Hero will accept
export interface PersonFields {
  /** URL for the image */
  Image: ImageField;

  /** Person's full name */
  Name: TextField;

  /** Person's job role */
  JobRole: TextField;

  /** Person's company */
  Company: TextField;

  /** Person's Biography */
  Biography: TextField;

  /** Linkedin Link */
  Linkedin: LinkField;

  /** Twitter Link */
  Twitter: LinkField;
}

export type PersonProps = {
  params: { [key: string]: string };
  fields: PersonFields;
  isPeopleGrid?: boolean;
};

export const Default = (props: PersonProps): JSX.Element => {
  const { isOpen, onOpen, onClose } = useDisclosure();
  return (
    <Box mb={30} w={props.isPeopleGrid ? 300 : 260}>
      <Image
        as={JssImage}
        src={props.fields.Image?.value?.src}
        w={props.isPeopleGrid ? 300 : 260}
        borderRadius={15}
        mb={10}
        onClick={props.params?.LinkToBio == '1' ? onOpen : undefined}
        cursor="pointer"
        field={props.fields.Image}
        objectFit="cover"
        boxSize="300px"
        alt={props.fields.Image?.value?.alt}
      />
      {props.params?.LinkToBio == '1' && (
        <Button onClick={onOpen} variant="link">
          <Heading as="h3" size="md" mt={2}>
            <JssText field={props.fields.Name} />
          </Heading>
        </Button>
      )}
      {!props.params?.LinkToBio && (
        <Heading as="h3" size="md" mt={2}>
          <JssText field={props.fields.Name} />
        </Heading>
      )}
      <Text className="person-job-role" mb={0}>
        <JssText field={props.fields.JobRole} />
      </Text>
      <Text className="person-company" mb="13px">
        <JssText field={props.fields.Company} />
      </Text>
      {(isEditorActive() ||
        (props.params?.DisplaySocialLinks == '1' && props.fields.Linkedin?.value?.href !== '')) && (
        <Link
          as={JssLink}
          isExternal={props.fields.Linkedin?.value?.target == '_blank'}
          _hover={{ color: 'inherit', textDecoration: 'none' }}
          field={props.fields.Linkedin}
        />
      )}
      {(isEditorActive() ||
        (props.params?.DisplaySocialLinks == '1' &&
          props.fields.Linkedin?.value?.href !== '' &&
          props.fields.Twitter?.value?.href !== '')) && (
        <Box display="inline" fontSize="18px">
          {' '}
          /{' '}
        </Box>
      )}
      {(isEditorActive() ||
        (props.params?.DisplaySocialLinks == '1' && props.fields.Twitter?.value?.href !== '')) && (
        <Link
          as={JssLink}
          isExternal={props.fields.Twitter?.value?.target == '_blank'}
          _hover={{ color: 'inherit', textDecoration: 'none' }}
          field={props.fields.Twitter}
        />
      )}
      {isEditorActive() && (
        <Text fontSize="12px" mb={0}>
          <JssText field={props.fields.Biography} />
        </Text>
      )}
      {props.params?.LinkToBio == '1' && (
        <Modal isOpen={isOpen} onClose={onClose} isCentered={true}>
          <ModalOverlay />
          <ModalContent minW={{ base: '80%', lg: 800 }} minH={{ base: 100, lg: 200 }}>
            <ModalHeader></ModalHeader>
            <ModalCloseButton />
            <ModalBody>
              <Flex flexWrap="wrap">
                <Box w={{ base: '100%', md: 'inherit' }} px={10} mb={{ base: 10, md: 0 }}>
                  <Image src={props.fields.Image?.value?.src} w={200} borderRadius={15} />
                </Box>
                <Box w={{ base: '100%', md: '55%', lg: '60%' }}>
                  <Heading as="h2" fontSize="25px">
                    {props.fields.Name?.value}
                  </Heading>
                  <Text fontSize="18px" mb="15px" color="#4D4D4D">
                    {props.fields.JobRole?.value}
                  </Text>
                  <Text fontSize="18px" mb={0}>
                    {props.fields.Company?.value}
                  </Text>
                  <Text as={JssRichText} fontSize="18px" mb="15px" field={props.fields.Biography} />
                </Box>
              </Flex>
            </ModalBody>
            <ModalFooter></ModalFooter>
          </ModalContent>
        </Modal>
      )}
    </Box>
  );
};
