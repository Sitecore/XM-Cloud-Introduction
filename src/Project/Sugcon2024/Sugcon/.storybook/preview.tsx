import React from 'react';
import { ChakraProvider } from '@chakra-ui/react';
import theme from '../src/Theme';
import { StoryFn } from '@storybook/react';
// import our styles from theme
import '../src/assets/main.scss';
import { sitecoreContextDecorator } from './decorators/sitecoreContextDecorator';
import { MINIMAL_VIEWPORTS, INITIAL_VIEWPORTS } from '@storybook/addon-viewport';

// Common styles to apply to each `viewport`
const commonPreviewStyles = {
  height: '80%',
  position: 'absolute',
  top: '10px',
};

// Global storybook preview config object
const preview = {
  parameters: {
    viewport: {
      viewports: {
        ...MINIMAL_VIEWPORTS,
        ...INITIAL_VIEWPORTS,
        small: {
          name: 'Breakpoint - Small',
          styles: {
            width: '480px',
            ...commonPreviewStyles,
          },
        },
        medium: {
          name: 'Breakpoint - Medium',
          styles: {
            width: '768px',
            ...commonPreviewStyles,
          },
        },
        large: {
          name: 'Breakpoint - Large',
          styles: {
            width: '1024px',
            ...commonPreviewStyles,
          },
        },
        xlarge: {
          name: 'Breakpoint - X-Large',
          styles: {
            width: '1440px',
            ...commonPreviewStyles,
          },
        },
      },
    },
  },
  decorators: [
    sitecoreContextDecorator,
    (StoryFn: StoryFn) => (
      <ChakraProvider theme={theme}>
        <StoryFn />
      </ChakraProvider>
    ),
  ],
};

export default preview;
