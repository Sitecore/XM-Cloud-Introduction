import { Meta, StoryObj } from '@storybook/react';
import { Story } from '@storybook/blocks';
import { LogoOnly } from 'src/components/Sponsors/SponsorListing';
import { Default as DefaultSponsorListing } from './SponsorListing.Default.stories';

const meta = {
  title: 'Sponsors/SponsorListing',
  
  component: LogoOnly,
  parameters: {
    layout: 'padded',
  },
  argTypes: {},
} satisfies Meta<typeof LogoOnly>;

export default meta;
type Story = StoryObj<typeof meta>;

export const SponsorListLogoOnly: Story = {
  name: 'Logo Only',
  args: DefaultSponsorListing.args,
};