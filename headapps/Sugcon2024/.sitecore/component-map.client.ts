// @ts-nocheck
// Client-safe component map for App Router

import { BYOCClientWrapper, NextjsContentSdkComponent, FEaaSClientWrapper } from '@sitecore-content-sdk/nextjs';
import { Form } from '@sitecore-content-sdk/nextjs';

import * as Navigation from 'src/components/templates/navigation/Navigation';
import * as SpeakersGrid from 'src/components/list-components/speakers-grid/SpeakersGrid';
import * as Venue from 'src/components/events/venue/Venue';
import * as Sessions from 'src/components/events/sessions/Sessions';
import * as Event from 'src/components/events/event/Event';
import * as Agenda from 'src/components/events/agenda/Agenda';
import * as Person from 'src/components/basic-components/person/Person';
import * as LinkButton from 'src/components/basic-components/link/Link.Button';
import * as Link from 'src/components/basic-components/link/Link';

export const componentMap = new Map<string, NextjsContentSdkComponent>([
  ['BYOCWrapper', BYOCClientWrapper],
  ['FEaaSWrapper', FEaaSClientWrapper],
  ['Form', Form],
  ['Navigation', { ...Navigation }],
  ['SpeakersGrid', { ...SpeakersGrid }],
  ['Venue', { ...Venue }],
  ['Sessions', { ...Sessions }],
  ['Event', { ...Event }],
  ['Agenda', { ...Agenda }],
  ['Person', { ...Person }],
  ['Link', { ...LinkButton, ...Link }],
]);

export default componentMap;
