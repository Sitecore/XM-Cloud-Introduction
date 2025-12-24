// @ts-nocheck
// Client-safe component map for App Router

import { BYOCClientWrapper, NextjsContentSdkComponent, FEaaSClientWrapper } from '@sitecore-content-sdk/nextjs';
import { Form } from '@sitecore-content-sdk/nextjs';

import * as Navigation from 'src/components/templates/navigation/Navigation';
import * as Venue from 'src/components/events/venue/Venue';
import * as Sessions from 'src/components/events/sessions/Sessions';
import * as Agenda from 'src/components/events/agenda/Agenda';

export const componentMap = new Map<string, NextjsContentSdkComponent>([
  ['BYOCWrapper', BYOCClientWrapper],
  ['FEaaSWrapper', FEaaSClientWrapper],
  ['Form', Form],
  ['Navigation', { ...Navigation }],
  ['Venue', { ...Venue }],
  ['Sessions', { ...Sessions }],
  ['Agenda', { ...Agenda }],
]);

export default componentMap;
