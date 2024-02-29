import React from 'react';
import { ChakraProvider } from '@chakra-ui/react';
import theme from '../src/Theme';
import { StoryFn } from '@storybook/react';
// import our styles from theme
import '../src/assets/main.scss';

// Global decorator to apply the styles to all stories
export const decorators = [
  (StoryFn: StoryFn) => (
    <ChakraProvider theme={theme}>
      <StoryFn />
    </ChakraProvider>
  )
];
