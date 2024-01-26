import { Meta, StoryObj } from '@storybook/react';

import { Default } from './Hero';

const meta = {
  title: 'Components/Hero',
  component: Default,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof Default>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Primary: Story = {
  args: {
    params: {
      styles: '',
    },
    fields: {
      Title: {
        value: 'An event from the community for the community',
      },
      Date: {
        value: 'October 5-6th',
      },
      Description: {
        value:
          'Get ready to experience the ultimate Sitecore conference of the year at SUGCON North America in Minneapolis, Minnesota.',
      },
      CallToAction: {
        value: {
          target: '',
          text: 'Register now',
        },
      },
      Image: {
        value: {
          src: 'https://picsum.photos/1200/800',
        },
      },
    },
  },
};
