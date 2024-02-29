import React from 'react';
import {
  ComponentParams,
  ComponentRendering,
  ImageField,
  LinkField,
  TextField,
  Image as JssImage,
  Link as JssLink,
  RichText as JssRichText,
  Text as JssText,
  RichTextField,
} from '@sitecore-jss/sitecore-jss-nextjs';
import {
  Flex,
  Heading,
  Image,
  Stack,
  Link as ChakraLink,
  Card,
  CardHeader,
  CardBody,
  Alert,
  AlertIcon,
  Box,
  Button,
  useDisclosure,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalCloseButton,
  ModalBody,
  HStack,
} from '@chakra-ui/react';

interface SponsorListingProps {
  rendering: ComponentRendering & { params: ComponentParams };
  params: ComponentParams;
  fields: Fields;
}

interface Fields {
  Title: TextField;
  Sponsors: Sponsor[];
}

type SponsorListingWrapperProps = SponsorListingProps & {
  children: React.ReactNode | React.ReactNode[];
};

type Sponsor = {
  fields: {
    SponsorName: TextField;
    SponsorLogo: ImageField;
    SponsorBio: RichTextField;
    SponsorURL: LinkField;
  };
};

export const Default = (props: SponsorListingProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  return (
    <div className={`component ${props.params.styles}`} id={id ? id : undefined}>
      <div className="component-content">
        <Alert status="warning">
          <AlertIcon />
          No variant selected for SponsorListing component
        </Alert>
      </div>
    </div>
  );
};
/**
 * Wrapper component for the SponsorListing component.
 * The inner content of the wrapper depends on the variant.
 * @param {SponsorListingWrapperProps} props - Props for the SponsorListingWrapper component.
 * @returns {JSX.Element} The rendered SponsorListingWrapper component.
 */
const SponsorListingWrapper = (props: SponsorListingWrapperProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  return (
    <div className={`component ${props.params.styles}`} id={id ? id : undefined}>
      <div className="component-content">
        <Box w={{ base: '100vw', md: '80vw' }} my="20" mx={{ base: '20px', md: 'auto' }}>
          <Card variant={'unstyled'}>
            <CardHeader>
              {/* Rendering Title if it exists */}
              {props.fields.Title && (
                <Heading as={'h1'} size={'xl'}>
                  {/* Rendering Title with JssText component */}
                  <JssText field={props.fields.Title} />
                </Heading>
              )}
            </CardHeader>
            <CardBody as={Flex}>
              {/* Rendering children components */}
              <Stack
                direction={{ base: 'column', md: 'row' }}
                gap={32}
                justifyContent={'space-between'}
                w={'full'}
              >
                {props.children}
              </Stack>
            </CardBody>
          </Card>
        </Box>
      </div>
    </div>
  );
};

/**
 * FullDetails Variant
 *
 * This function renders a detailed listing of sponsors with their logos, names, bios, and URLs.
 * If no sponsor fields are provided, it renders a default view.
 *
 * @param props - SponsorListingProps object containing fields for rendering sponsor details.
 * @returns A JSX Element representing the detailed listing of sponsors or a default view.
 */
export const FullDetails = (props: SponsorListingProps): JSX.Element => {
  // Check if sponsor fields are provided
  if (props.fields) {
    return (
      // Render sponsor listing wrapper with provided props
      <SponsorListingWrapper {...props}>
        {/* Map through each sponsor and render their details */}
        {props.fields.Sponsors.map((sponsor, index) => (
          <Stack gap={8} key={index}>
            {/* Render sponsor logo */}
            <Image as={JssImage} field={sponsor.fields.SponsorLogo} />

            {/* Render sponsor name */}
            <Heading as={'h2'} size={'lg'}>
              <JssText field={sponsor.fields.SponsorName} />
            </Heading>

            {/* Render sponsor bio */}
            <JssRichText field={sponsor.fields.SponsorBio} />

            {/* Render sponsor URL as a link */}
            <ChakraLink as={JssLink} field={sponsor.fields.SponsorURL}>
              Visit sponsor site
            </ChakraLink>
          </Stack>
        ))}
      </SponsorListingWrapper>
    );
  }

  // If no sponsor fields are provided, render default view
  return <Default {...props} />;
};

/**
 * LogoOnly Variant
 * Rendering variant that displays sponsor logos only, and optionally provides a modal for each logo.
 * @param {SponsorListingProps} props - Props object containing data and configurations for the component.
 * @returns {JSX.Element} - Returns JSX element representing the LogoOnly component.
 */
export const LogoOnly = (props: SponsorListingProps): JSX.Element => {
  const { isOpen, onOpen, onClose } = useDisclosure();

  // If props contain fields data, render sponsor logos
  if (props.fields) {
    return (
      <SponsorListingWrapper {...props}>
        {props.fields.Sponsors.map((sponsor, index) => (
          <React.Fragment key={index}>
            {/* Render sponsor logo as a button */}
            <Button onClick={onOpen} variant="unstyled">
              <JssImage field={sponsor.fields.SponsorLogo} />
            </Button>

            {/* Render modal for the sponsor */}
            {RenderModal(isOpen, onClose, sponsor)}
          </React.Fragment>
        ))}
      </SponsorListingWrapper>
    );
  }

  // If props do not contain fields data, render default component
  return <Default {...props} />;
};

/**
 * Renders a modal component displaying information about a sponsor.
 * @param isOpen - A boolean indicating whether the modal is open or closed.
 * @param onClose - A function to be called when the modal is closed.
 * @param sponsor - An object representing the sponsor's details.
 * @returns A modal component displaying sponsor information.
 */
function RenderModal(isOpen: boolean, onClose: () => void, sponsor: Sponsor) {
  return (
    <Modal isOpen={isOpen} onClose={onClose} size={'6xl'} isCentered>
      <ModalOverlay />

      <ModalContent>
        <ModalCloseButton size="lg" />
        <ModalBody p="8">
          <HStack>
            <JssImage field={sponsor.fields.SponsorLogo} width="1024" />
            <Stack gap={8}>
              <Heading as={'h2'} size={'lg'}>
                <JssText field={sponsor.fields.SponsorName} />
              </Heading>
              <JssRichText field={sponsor.fields.SponsorBio} />
              <ChakraLink as={JssLink} field={sponsor.fields.SponsorURL}>
                Visit sponsor site
              </ChakraLink>
            </Stack>
          </HStack>
        </ModalBody>
      </ModalContent>
    </Modal>
  );
}
