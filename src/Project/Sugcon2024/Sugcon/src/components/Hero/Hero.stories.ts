import { Meta } from '@storybook/react';
//import { Meta, StoryObj } from '@storybook/react';

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
/*type Story = StoryObj<typeof meta>;


export const Primary: Story = {
  args: {
    title: 'An event from the community for the community',
    date: 'October 5-6th',
    description:
      'Get ready to experience the ultimate Sitecore conference of the year at SUGCON North America in Minneapolis, Minnesota.',
    buttonText: 'Register now',
    imageUrl: 'https://picsum.photos/1200/800',
    callToActionLink: '',
    
  },
};*/
