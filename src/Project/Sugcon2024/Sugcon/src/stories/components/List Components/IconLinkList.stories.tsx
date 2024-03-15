import { Meta, StoryObj } from '@storybook/react';
import { Default as IconLinkList } from 'src/components/List Components/IconLinkList';

const meta = {
  title: 'List Components/IconLinkList',
  component: IconLinkList,
  parameters: {
    layout: 'padded',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof IconLinkList>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {
  name: 'Default',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Title: {
        value: 'Icon Link List',
      },
      items: [
        {
            fields: {
                Link: { value: { src:"https://www.sitecore.com"} },
                Icon: { 
                  fields: {
                    CssClass: { value: "" }
                  }
                 }
            }
        }
      ]
    },
  },
};
