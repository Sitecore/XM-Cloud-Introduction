'use client';
import React, { JSX } from 'react';
import {
  ComponentParams,
  ImageField,
  LinkField,
  TextField,
  RichTextField,
} from '@sitecore-content-sdk/nextjs';
import {
  Alert,
  AlertIcon,
} from '@chakra-ui/react';
import { FullDetails } from './SponsorListing.FullDetails';

export interface SponsorListingProps {
  params: ComponentParams;
  fields?: Fields | undefined;
}

interface Fields {
  Title: TextField;
  Sponsors: Sponsor[];
}

export type Sponsor = {
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