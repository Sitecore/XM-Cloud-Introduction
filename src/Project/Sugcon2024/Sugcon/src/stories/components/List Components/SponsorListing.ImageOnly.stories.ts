import { Meta, StoryObj } from '@storybook/react';
import { LogoOnly } from 'src/components/Sponsors/SponsorListing';
import { Default as DefaultSponsorListing } from '../List Components/SponsorListing.stories';

const meta = {
  title: 'Components/SponsorListing',
  component: LogoOnly,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof LogoOnly>;

export default meta;
type Story = StoryObj<typeof meta>;

export const SponsorListLogoOnly: Story = {
  name: 'Logo Only',
  args: DefaultSponsorListing.args,
};
