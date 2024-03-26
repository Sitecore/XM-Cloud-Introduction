import { SitecoreContext } from '@sitecore-jss/sitecore-jss-nextjs';

import { componentBuilder } from '../../src/temp/componentBuilder';
import config from '../../src/temp/config';
import { StoryContext } from '@storybook/react';
import React from 'react';

interface SitecoreRoute {
  // Empty
}

interface SitecoreContextType {
  route?: SitecoreRoute;
  // Can add other properties as needed
}

type PageState = 'normal' | 'edit' | 'preview';

interface LayoutDataType {
  sitecore: {
    context: {
      language: string;
      pageEditing: boolean;
      pageState: PageState;
    };
    route: SitecoreRoute;
  };
}

const pageStates = {
  normal: 'normal',
  edit: 'edit',
  preview: 'preview',
} as const;

function createLayoutDataForPageState(pageState: PageState): LayoutDataType {
  return {
    sitecore: {
      context: {
        language: config.defaultLanguage,
        pageEditing: pageState === pageStates.edit,
        pageState,
      },
      route: {},
    },
  };
}

const defaultLayoutDataMap = {
  [pageStates.normal]: createLayoutDataForPageState(pageStates.normal),
  [pageStates.edit]: createLayoutDataForPageState(pageStates.edit),
  [pageStates.preview]: createLayoutDataForPageState(pageStates.preview),
};

export function sitecoreContextDecorator(
  Story: React.FunctionComponent<any>,
  { args, parameters }: StoryContext
): JSX.Element {
  const { sitecoreContext, ...rest } = args as {
    sitecoreContext?: SitecoreContextType;
    [key: string]: any;
  };
  const { sitecorePageState } = parameters;

  const defaultLayoutData =
    defaultLayoutDataMap[sitecorePageState] || defaultLayoutDataMap[pageStates.normal];

  // Merge the default route with the route from args or parameters
  const routeData = {
    ...defaultLayoutData.sitecore.route,
    ...(sitecoreContext?.route || {}),
  };

  const layoutData = {
    ...defaultLayoutData,
    sitecore: {
      ...defaultLayoutData.sitecore,
      route: routeData,
    },
  };

  // Additional mock for ExperienceEditor.isActive() based on the pageState
  // This allows us to mock `isEditorActive()`
  // import { isEditorActive } from '@sitecore-jss/sitecore-jss-nextjs/utils';
  if (typeof (window as any) !== 'undefined') {
    // Ensure the global window object is only augmented, not overwritten
    (window as any).Sitecore = (window as any).Sitecore || {};
    (window as any).Sitecore.PageModes = (window as any).Sitecore.PageModes || {};
    // Set ChromeManager to an empty object if in 'edit' mode, null otherwise
    (window as any).Sitecore.PageModes.ChromeManager =
      sitecorePageState === pageStates.edit ? {} : null;
  }

  return (
    <SitecoreContext
      layoutData={layoutData}
      componentFactory={componentBuilder.getComponentFactory()}
    >
      <Story {...rest} />
    </SitecoreContext>
  );
}

