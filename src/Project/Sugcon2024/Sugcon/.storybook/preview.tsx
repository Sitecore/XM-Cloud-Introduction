import React from 'react';
import { ChakraProvider } from '@chakra-ui/react';
import theme from '../src/Theme'; // Adjust the path to your Chakra theme if you have a custom one
import { StoryFn } from '@storybook/react';

// Global decorator to apply the styles to all stories
export const decorators = [
  (StoryFn: StoryFn) => (
    <ChakraProvider theme={theme}>
      <StoryFn />
    </ChakraProvider>
  ),
];
