import { Meta, StoryObj } from '@storybook/react';
import { Default as SponsorListing } from 'src/components/Sponsors/SponsorListing';

const meta = {
  title: 'Components/SponsorListing',
  component: SponsorListing,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof SponsorListing>;

export default meta;
type Story = StoryObj<typeof meta>;

export const SponsorListFullDetails: Story = {
  name: 'Error',
  args: {
    params: {
      styles: '',
    },
  },
};
