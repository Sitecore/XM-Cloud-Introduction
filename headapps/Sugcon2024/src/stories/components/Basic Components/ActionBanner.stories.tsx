import { Meta, StoryObj } from '@storybook/react';
import { Default as ActionBanner } from '../../../components/Basic Components/ActionBanner';

const meta = {
  title: 'Basic Components/ActionBanner',
  component: ActionBanner,
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof ActionBanner>;

export default meta;

type Story = StoryObj<typeof meta>;

export const ActionBannerStory: Story = {
  name: 'ActionBanner',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Title: {
        value: 'FAQs',
      },
      Text: {
        value: 'Lorem Ipsum',
      },
      CallToAction: {
        value: {
          href: 'mailto:community@sitecore.com?subject=Sugcon sponsorship',
          text: 'Contact us',
          linktype: 'mailto',
          style: '',
          url: 'mailto:community@sitecore.com?subject=Sugcon sponsorship',
          title: '',
        },
      },
    },
  },
};
