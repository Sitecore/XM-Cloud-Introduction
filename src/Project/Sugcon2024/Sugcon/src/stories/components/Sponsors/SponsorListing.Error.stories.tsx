import { Meta, StoryObj } from '@storybook/react';
import { Default as SponsorListing } from 'src/components/Sponsors/SponsorListing';

const meta = {
  title: 'Sponsors/SponsorListing',
  component: SponsorListing,
  parameters: {
    layout: 'padded',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof SponsorListing>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Error: Story = {
  name: 'No variant or datasource',
  args: {
    params: {
      styles: '',
    },
  },
};
