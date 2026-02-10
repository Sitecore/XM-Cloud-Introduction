'use client';
import React, { JSX } from 'react';
import {
  Image as ContentSdkImage,
  Text as ContentSdkText,
  Link as ContentSdkLink,
  RichText as ContentSdkRichText,
} from '@sitecore-content-sdk/nextjs';
import {
  Heading,
  Image,
  Box,
  SimpleGrid,
  Link as ChakraLink,
} from '@chakra-ui/react';
import { LayoutFlex } from 'components/page-structure/layout-flex/LayoutFlex';
import { Default, SponsorListingProps } from './SponsorListing';

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
              {/* Rendering Title with ContentSdkText component */}
              <ContentSdkText field={props.fields.Title} />
            </Heading>
          )}
          <SimpleGrid columns={{ base: 1, md: 2, lg: 2, xl: 2, '2xl': 2 }} spacing={20} w="full">
            {props.fields.Sponsors.map((sponsor) => (
                <Box key={sponsor.fields.SponsorName.value}>
                  <Box
                    justifyContent="center"
                    alignItems="center"
                    height="150px"
                    className="fullWidthSponsorImage"
                  >
                    <ContentSdkImage field={sponsor.fields.SponsorLogo} as={Image} width="auto" />
                  </Box>
                  {/* Render sponsor name */}
                  <Heading as={'h3'} size={'lg'}>
                    <ContentSdkText field={sponsor.fields.SponsorName} />
                  </Heading>
                  {/* Render sponsor bio */}
                  <ContentSdkRichText field={sponsor.fields.SponsorBio} />
                  {/* Render sponsor URL as a link */}
                  <ChakraLink as={ContentSdkLink} field={sponsor.fields.SponsorURL} />
                </Box>
            ))}
          </SimpleGrid>
        </LayoutFlex>
      </>
    );
  }

  // If no sponsor fields are provided, render default view
  return <Default {...props} />;
};