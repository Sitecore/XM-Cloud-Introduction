'use client';
import React, { useState, useCallback, JSX } from 'react';
import {
  Image as ContentSdkImage,
  Text as ContentSdkText,
  Link as ContentSdkLink,
  RichText as ContentSdkRichText,
} from '@sitecore-content-sdk/nextjs';
import {
  Heading,
  Image,
  Stack,
  Link as ChakraLink,
  Box,
  useDisclosure,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalCloseButton,
  ModalBody,
  HStack,
  SimpleGrid,
  Alert,
  AlertIcon,
} from '@chakra-ui/react';
import { LayoutFlex } from 'components/page-structure/layout-flex/LayoutFlex';
import type { Sponsor, SponsorListingProps } from './SponsorListing';

/**
 * LogoWithPopup Variant
 * Rendering variant that displays sponsor logos only, and optionally provides a modal for each logo.
 * @param {SponsorListingProps} props - Props object containing data and configurations for the component.
 * @returns {JSX.Element} - Returns JSX element representing the LogoOnly component.
 */
export const LogoWithPopup = (props: SponsorListingProps): JSX.Element => {
  const { isOpen, onOpen, onClose } = useDisclosure();
  const [selectedSponsor, setSelectedSponsor] = useState<Sponsor | null>(null);

  const handleOpenModal = useCallback(
    (sponsor: Sponsor) => {
      setSelectedSponsor(sponsor);
      onOpen();
    },
    [onOpen]
  );

  const handleCloseModal = useCallback(() => {
    onClose();
    setSelectedSponsor(null);
  }, [onClose]);

  // If props contain fields data, render sponsor logos
  if (props.fields) {
    return (
      <>
        <LayoutFlex direction="column">
          {props.fields.Title && (
            <Heading as={'h2'} size={'xl'} marginBottom={10}>
              {/* Rendering Title with JssText component */}
              <ContentSdkText field={props.fields.Title} />
            </Heading>
          )}
          <SimpleGrid columns={{ base: 1, md: 2, lg: 4, xl: 4, '2xl': 4 }} spacing={20} w="full">
            {props.fields.Sponsors.map((sponsor, index) => (
              <Box key={index} display="flex" justifyContent="center" alignItems="center">
                <ContentSdkImage
                  field={sponsor.fields.SponsorLogo}
                  as={Image}
                  maxheight={70}
                  width={'auto'}
                  onClick={() => handleOpenModal(sponsor)}
                  cursor="pointer"
                />
              </Box>
            ))}
          </SimpleGrid>
        </LayoutFlex>
        {selectedSponsor && (
          <RenderModal isOpen={isOpen} onClose={handleCloseModal} sponsor={selectedSponsor} />
        )}
      </>
    );
  }

  // If props do not contain fields data, render fallback (cannot use Default from server component)
  const id = props.params.RenderingIdentifier;
  return (
    <div className={`component ${props.params.styles}`} id={id ?? undefined}>
      <div className="component-content">
        <Alert status="warning">
          <AlertIcon />
          No variant or datasource selected for SponsorListing component
        </Alert>
      </div>
    </div>
  );
};

interface RenderModalProps {
  isOpen: boolean;
  onClose: () => void;
  sponsor: Sponsor;
}

/**
 * Renders a modal component displaying information about a sponsor.
 * @param props - Props containing isOpen, onClose, and sponsor details.
 * @returns A modal component displaying sponsor information.
 */
function RenderModal({ isOpen, onClose, sponsor }: RenderModalProps): JSX.Element {
  return (
    <Modal isOpen={isOpen} onClose={onClose} size={'6xl'} isCentered>
      <ModalOverlay />

      <ModalContent>
        <ModalCloseButton size="lg" />
        <ModalBody p="8">
          <HStack>
            <ContentSdkImage field={sponsor.fields.SponsorLogo} width="800" />
            <Stack gap={8} ml={16}>
              <Heading as={'h2'} size={'lg'}>
                <ContentSdkText field={sponsor.fields.SponsorName} />
              </Heading>
              <ContentSdkRichText field={sponsor.fields.SponsorBio} />
              <ChakraLink as={ContentSdkLink} field={sponsor.fields.SponsorURL}>
                Visit sponsor site
              </ChakraLink>
            </Stack>
          </HStack>
        </ModalBody>
      </ModalContent>
    </Modal>
  );
}