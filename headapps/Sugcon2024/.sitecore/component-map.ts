// @ts-nocheck
// Below are built-in components that are available in the app, it's recommended to keep them as is

import { BYOCServerWrapper, NextjsContentSdkComponent, FEaaSServerWrapper } from '@sitecore-content-sdk/nextjs';
import { Form } from '@sitecore-content-sdk/nextjs';

// end of built-in components
import * as LayoutConstants from 'src/components/templates/LayoutConstants';
import * as Navigation from 'src/components/templates/navigation/Navigation';
import * as HeaderMeta from 'src/components/templates/header/HeaderMeta';
import * as Header from 'src/components/templates/header/Header';
import * as Footer from 'src/components/templates/footer/Footer';
import * as RowSplitter from 'src/components/page-structure/row-splitter/RowSplitter';
import * as PartialDesignDynamicPlaceholder from 'src/components/page-structure/partial-design-dynamic-placeholder/PartialDesignDynamicPlaceholder';
import * as LayoutFlex from 'src/components/page-structure/layout-flex/LayoutFlex';
import * as Container from 'src/components/page-structure/container/Container';
import * as ColumnSplitter from 'src/components/page-structure/column-splitter/ColumnSplitter';
import * as TestimonialList from 'src/components/list-components/testimonial-list/TestimonialList';
import * as SponsorListingLogoWithPopup from 'src/components/list-components/sponsor-listing/SponsorListing.LogoWithPopup';
import * as SponsorListingLogoOnly from 'src/components/list-components/sponsor-listing/SponsorListing.LogoOnly';
import * as SponsorListingFullDetails from 'src/components/list-components/sponsor-listing/SponsorListing.FullDetails';
import * as SponsorListing from 'src/components/list-components/sponsor-listing/SponsorListing';
import * as SpeakersGrid from 'src/components/list-components/speakers-grid/SpeakersGrid';
import * as PeopleGrid from 'src/components/list-components/people-grid/PeopleGrid';
import * as LinkList from 'src/components/list-components/link-list/LinkList';
import * as IconLinkList from 'src/components/list-components/icon-link-list/IconLinkList';
import * as Accordion from 'src/components/list-components/accordion/Accordion';
import * as Venue from 'src/components/events/venue/Venue';
import * as Sessions from 'src/components/events/sessions/Sessions';
import * as EventTeaser from 'src/components/events/event-teaser/EventTeaser';
import * as Event from 'src/components/events/event/Event';
import * as Agenda from 'src/components/events/agenda/Agenda';
import * as VideoText from 'src/components/basic-components/video-text/VideoText';
import * as Title from 'src/components/basic-components/title/Title';
import * as TextImage from 'src/components/basic-components/text-image/TextImage';
import * as RichText from 'src/components/basic-components/rich-text/RichText';
import * as Promo from 'src/components/basic-components/promo/Promo';
import * as Person from 'src/components/basic-components/person/Person';
import * as PageTitle from 'src/components/basic-components/page-title/PageTitle';
import * as PageContent from 'src/components/basic-components/page-content/PageContent';
import * as Logo from 'src/components/basic-components/logo/Logo';
import * as LinkButton from 'src/components/basic-components/link/Link.Button';
import * as Link from 'src/components/basic-components/link/Link';
import * as Image from 'src/components/basic-components/image/Image';
import * as Hero from 'src/components/basic-components/hero/Hero';
import * as ErrorMessage from 'src/components/basic-components/error-message/ErrorMessage';
import * as ContentBlock from 'src/components/basic-components/content-block/ContentBlock';
import * as ActionBanner from 'src/components/basic-components/action-banner/ActionBanner';

export const componentMap = new Map<string, NextjsContentSdkComponent>([
  ['BYOCWrapper', BYOCServerWrapper],
  ['FEaaSWrapper', FEaaSServerWrapper],
  ['Form', Form],
  ['LayoutConstants', { ...LayoutConstants }],
  ['Navigation', { ...Navigation, componentType: 'client' }],
  ['HeaderMeta', { ...HeaderMeta, componentType: 'client' }],
  ['Header', { ...Header }],
  ['Footer', { ...Footer }],
  ['RowSplitter', { ...RowSplitter }],
  ['PartialDesignDynamicPlaceholder', { ...PartialDesignDynamicPlaceholder }],
  ['LayoutFlex', { ...LayoutFlex }],
  ['Container', { ...Container }],
  ['ColumnSplitter', { ...ColumnSplitter }],
  ['TestimonialList', { ...TestimonialList }],
  ['SponsorListing', { ...SponsorListingLogoWithPopup, ...SponsorListingLogoOnly, ...SponsorListingFullDetails, ...SponsorListing }],
  ['SpeakersGrid', { ...SpeakersGrid, componentType: 'client' }],
  ['PeopleGrid', { ...PeopleGrid }],
  ['LinkList', { ...LinkList }],
  ['IconLinkList', { ...IconLinkList }],
  ['Accordion', { ...Accordion }],
  ['Venue', { ...Venue, componentType: 'client' }],
  ['Sessions', { ...Sessions, componentType: 'client' }],
  ['EventTeaser', { ...EventTeaser }],
  ['Event', { ...Event, componentType: 'client' }],
  ['Agenda', { ...Agenda, componentType: 'client' }],
  ['VideoText', { ...VideoText }],
  ['Title', { ...Title }],
  ['TextImage', { ...TextImage }],
  ['RichText', { ...RichText }],
  ['Promo', { ...Promo }],
  ['Person', { ...Person, componentType: 'client' }],
  ['PageTitle', { ...PageTitle }],
  ['PageContent', { ...PageContent }],
  ['Logo', { ...Logo }],
  ['Link', { ...LinkButton, ...Link }],
  ['Image', { ...Image }],
  ['Hero', { ...Hero }],
  ['ErrorMessage', { ...ErrorMessage }],
  ['ContentBlock', { ...ContentBlock }],
  ['ActionBanner', { ...ActionBanner }],
]);

export default componentMap;
