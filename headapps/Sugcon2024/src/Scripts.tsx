'use client';
import { JSX } from 'react';
import { EditingScripts } from '@sitecore-content-sdk/nextjs';
import CdpPageView from 'components/content-sdk/CdpPageView';

const Scripts = (): JSX.Element => {
  return (
    <>
      <CdpPageView />
      <EditingScripts />
    </>
  );
};

export default Scripts;
