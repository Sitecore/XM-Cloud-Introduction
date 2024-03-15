import { Meta, StoryObj } from '@storybook/react';
import { Story } from '@storybook/blocks';
import { LogoWithPopup } from 'src/components/Sponsors/SponsorListing';
import { Default as DefaultSponsorListing } from './SponsorListing.Default.stories';

const meta = {
  title: 'Sponsors/SponsorListing',
  
  component: LogoWithPopup,
  parameters: {
    layout: 'padded',
  },
  argTypes: {},
} satisfies Meta<typeof LogoWithPopup>;

export default meta;
type Story = StoryObj<typeof meta>;

export const SponsorListLogoWithPopup: Story = {
  name: 'Logo with pop-up',
  args: DefaultSponsorListing.args,
};