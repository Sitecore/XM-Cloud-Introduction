import React from 'react';
import {
  ComponentParams,
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
  Heading,
  Image,
  Stack,
  Link as ChakraLink,
  Alert,
  AlertIcon,
  Box,
  useDisclosure,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalCloseButton,
  ModalBody,
  HStack,
  SimpleGrid,
} from '@chakra-ui/react';
import { LayoutFlex } from 'components/Templates/LayoutFlex';

export interface SponsorListingProps {
  params: ComponentParams;
  fields?: Fields | undefined;
}

interface Fields {
  Title: TextField;
  Sponsors: Sponsor[];
}

type Sponsor = {
  fields: {
    SponsorName: TextField;
    SponsorLogo: ImageField;
    SponsorBio: RichTextField;
    SponsorURL: LinkField;
  };
};

export const ErrorMessage = (): JSX.Element => (
  <Alert status="warning">
    <AlertIcon />
    No variant or datasource selected for SponsorListing component
  </Alert>
);

/**
 * Default component for the SponsorListing component.
 * The inner content of the wrapper depends on the variant.
 * @param {SponsorListingWrapperProps} props - Props for the SponsorListingWrapper component.
 * @returns {JSX.Element} The rendered SponsorListingWrapper component.
 */
export const Default = (props: SponsorListingProps): JSX.Element => {
  const id = props.params.RenderingIdentifier;

  if (props.fields) {
    return FullDetails(props);
  }

  // Fallback if no variant or datasource is selected
  return (
    <div className={`component ${props.params.styles}`} id={id ? id : undefined}>
      <div className="component-content">
        <ErrorMessage />
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
      <>
        <LayoutFlex direction="column">
          {props.fields.Title && (
            <Heading as={'h2'} size={'xl'} marginBottom={10}>
              {/* Rendering Title with JssText component */}
              <JssText field={props.fields.Title} />
            </Heading>
          )}
          <SimpleGrid 
            columns={{ base: 1, md: 2, lg: 2, xl: 2, '2xl': 2 }}
            spacing={20}
            w="full"
            >
            {props.fields.Sponsors.map((sponsor) => (  
              <>
                <Box>
                  <Box
                    justifyContent='center'
                    alignItems='center'
                    height='150px'
                    className='fullWidthSponsorImage'
                  >
                    <JssImage 
                      field={sponsor.fields.SponsorLogo} 
                      as={Image} 
                      width='auto'
                    />
                  </Box>
                  
                  {/* Render sponsor name */}
                  <Heading as={'h3'} size={'lg'}>
                    <JssText field={sponsor.fields.SponsorName} />
                  </Heading>
                  {/* Render sponsor bio */}
                  <JssRichText field={sponsor.fields.SponsorBio} />
                  {/* Render sponsor URL as a link */}
                  <ChakraLink as={JssLink} field={sponsor.fields.SponsorURL} />
                </Box>
              </>
            ))}
          </SimpleGrid>
        </LayoutFlex>  
      </>
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
export const LogoWithPopup = (props: SponsorListingProps): JSX.Element => {
  const { isOpen, onOpen, onClose } = useDisclosure();

  // If props contain fields data, render sponsor logos
  if (props.fields) {
    return (
      <>
        <LayoutFlex direction="column">
          {props.fields.Title && (
            <Heading as={'h2'} size={'xl'} marginBottom={10}>
              {/* Rendering Title with JssText component */}
              <JssText field={props.fields.Title} />
            </Heading>
          )}
          <SimpleGrid 
            columns={{ base: 1, md: 2, lg: 4, xl: 4, '2xl': 4 }}
            spacing={20}
            w="full"
            >
            {props.fields.Sponsors.map((sponsor) => (  
              <Box 
                display= 'flex'
                justifyContent='center'
                alignItems='center'
              >
                <JssImage 
                  field={sponsor.fields.SponsorLogo} 
                  as={Image} 
                  maxHeight={70} 
                  width={'auto'} 
                  onClick={onOpen}
                />
                {/* Render modal for the sponsor */}
                {RenderModal(isOpen, onClose, sponsor)}
              </Box>
            ))}
          </SimpleGrid>
        </LayoutFlex>  
      </>
    );
  }

  // If props do not contain fields data, render default component
  return <Default {...props} />;
};

/**
 * LogoOnly Variant
 * Rendering variant that displays sponsor logos only, and optionally provides a modal for each logo.
 * @param {SponsorListingProps} props - Props object containing data and configurations for the component.
 * @returns {JSX.Element} - Returns JSX element representing the LogoOnly component.
 */
export const LogoOnly = (props: SponsorListingProps): JSX.Element => {
  // If props contain fields data, render sponsor logos
  if (props.fields) {
    return (
      <>
        <LayoutFlex direction="column">
          {props.fields.Title && (
            <Heading as={'h2'} size={'xl'} marginBottom={10}>
              {/* Rendering Title with JssText component */}
              <JssText field={props.fields.Title} />
            </Heading>
          )}
          <SimpleGrid 
            columns={{ base: 1, md: 3, lg: 6, xl: 6, '2xl': 6 }}
            spacing={20}
            w="full"
            >
            {props.fields.Sponsors.map((sponsor) => (  
              <Box 
                display= 'flex'
                justifyContent='center'
                alignItems='center'
              >
                <JssImage field={sponsor.fields.SponsorLogo} as={Image} maxHeight={70} width={'auto'} />
              </Box>
            ))}
          </SimpleGrid>
        </LayoutFlex>  
      </>
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
            <JssImage field={sponsor.fields.SponsorLogo} width="800" />
            <Stack gap={8} ml={16}>
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
