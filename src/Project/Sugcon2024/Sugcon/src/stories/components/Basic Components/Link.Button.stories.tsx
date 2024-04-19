import { Meta, StoryObj } from '@storybook/react';
import { Button as ButtonLink } from '../../../components/Basic Components/Link';
const meta = {
  title: 'Basic Components/Link',
  component: ButtonLink,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof ButtonLink>;
export default meta;
type Story = StoryObj<typeof meta>;

export const Button: Story = {
  name: 'Button',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Link: {
        value: {
          href: 'https://www.google.com',
          text: 'Register Now',
          linktype: 'external'
        },
      },
    },
  },
};

