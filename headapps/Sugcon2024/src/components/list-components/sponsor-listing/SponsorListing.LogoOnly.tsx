'use client';
import React, { JSX } from 'react';
import {
  Image as ContentSdkImage,
  Text as ContentSdkText,
} from '@sitecore-content-sdk/nextjs';
import {
  Heading,
  Image,
  Box,
  SimpleGrid,
} from '@chakra-ui/react';
import { LayoutFlex } from 'components/page-structure/layout-flex/LayoutFlex';
import { Default, SponsorListingProps } from './SponsorListing';

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
              <ContentSdkText field={props.fields.Title} />
            </Heading>
          )}
          <SimpleGrid columns={{ base: 1, md: 3, lg: 6, xl: 6, '2xl': 6 }} spacing={20} w="full">
            {props.fields.Sponsors.map((sponsor, index) => (
              <Box key={index} display="flex" justifyContent="center" alignItems="center">
                <ContentSdkImage
                  field={sponsor.fields.SponsorLogo}
                  as={Image}
                  maxHeight={70}
                  width={'auto'}
                />
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