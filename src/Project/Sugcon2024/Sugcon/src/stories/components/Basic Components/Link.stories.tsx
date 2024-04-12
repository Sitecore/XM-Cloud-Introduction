import { Meta, StoryObj } from '@storybook/react';
import { Default as LinkComponent } from '../../../components/Basic Components/Link';
const meta = {
  title: 'Basic Components/Link',
  component: LinkComponent,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof LinkComponent>;
export default meta;
type Story = StoryObj<typeof meta>;

export const Link: Story = {
  name: 'Link',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Link: {
        value: {
          href: 'mailto:contact@sitecore.com?subject=SUGCON',
          text: 'Contact Us',
          linktype: 'mailto',
          style: '',
          url: 'mailto:contact@sitecore.com?subject=SUGCON',
          title: '',
        },
      },
    },
  },
};