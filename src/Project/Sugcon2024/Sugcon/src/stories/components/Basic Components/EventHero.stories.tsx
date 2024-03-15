import { Meta, StoryObj } from '@storybook/react';
import { HeroEvent } from '../../../components/Basic Components/Hero';
const meta = {
  title: 'Basic Components/Hero',
  component: HeroEvent,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof HeroEvent>;
export default meta;
type Story = StoryObj<typeof meta>;
export const Event: Story = {
  name: 'Hero Event',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Headline: {
        value: 'An event from the community for the community',
      },
      EventDate: {
        value: 'October 5-6th',
      },
      Text: {
        value:
          'Get ready to experience the ultimate Sitecore conference of the year at SUGCON North America in Minneapolis, Minnesota.',
      },
      CallToAction: {
        value: {
          href: 'https://www.sitecore.com',
          anchor: 'Register now',
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
