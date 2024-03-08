import { Meta, StoryObj } from '@storybook/react';
import { Default as Venue } from 'src/components/Basic Components/Venue';

const meta = {
  title: 'Components/Venue',
  component: Venue,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof Venue>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {
  name: 'Default',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Headline: {
        value: 'Headline',
      },
      HotelName: {
        value: 'Azure Breeze Resort',
      },
      HotelAddress: {
        value: '123 Seaside Avenue<br />Azure Breeze, CA 90210',
      },
      AdditionalInfoTitle: {
        value: 'Additional Info Title',
      },
      AdditionalInfoText: {
        value:
          '<p>Feel free to imagine this luxurious retreat nestled by the turquoise waters, where guests can unwind in hammocks under swaying palm trees</p>',
      },
      ButtonLink: {
        value: {
          href: 'https://www.sitecore.com',
          text: 'Visit website',
        },
      },
      VenueImages: [
        {
          name: 'Mock Item',
          url: 'https://placehold.co/450x400/000000/FFF/png?text=Image+1',
          fields: {},
        },
        {
          name: 'Mock Item',
          url: 'https://placehold.co/450x400/CC0000/FFF/png?text=Image+2',
          fields: {},
        },
        {
          name: 'Mock Item',
          url: 'https://placehold.co/450x400/0000FF/FFF/png?text=Image+3',
          fields: {},
        },
      ],
    },
  },
};
