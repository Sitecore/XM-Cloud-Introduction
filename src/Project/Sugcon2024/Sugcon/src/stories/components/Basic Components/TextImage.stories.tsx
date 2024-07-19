import { Meta, StoryObj } from '@storybook/react';
import { Default as TextImage } from '../../../components/Basic Components/TextImage';
const meta = {
  title: 'Basic Components/TextImage',
  component: TextImage,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof TextImage>;
export default meta;
type Story = StoryObj<typeof meta>;
export const TextAndImage: Story = {
  name: 'Text and Image',
  args: {
    rendering: {
      componentName: 'TextAndImage',
    },
    params: {
      styles: '',
    },
    fields: {
      Text: {
        value:
          'The Sitecore User Group Conference is a community event, which brings the develop community together and provides the perfect platform to gain knowledge and get inspired by the potential within Sitecore Experience Cloud, Experience Commerce and more.',
      },
      Image: {
        value: {
          src: 'images/urn_aaid_sc_US.png',
          alt: '',
          width: '1600',
          height: '1065',
        },
      },
      Headline: {
        value: 'An Event from the Community for the Community',
      },
    },
  },
};
