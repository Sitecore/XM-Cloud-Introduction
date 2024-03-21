import { Meta, StoryObj } from '@storybook/react';
import { Default as ErrorMessage } from '../../../components/Basic Components/ErrorMessage';
const meta = {
  title: 'Basic Components/Error Message',
  component: ErrorMessage,
  parameters: {},
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof ErrorMessage>;
export default meta;
type Story = StoryObj<typeof meta>;
export const ErrorMsg: Story = {
  name: 'Error Message',
  args: {
    params: {},
    fields: {
      Text: {
        value: "The page you've requested could not be found, please check the URL and try again.",
      },
      Headline: {
        value: 'Page Not Found',
      },
      StatusCode: {
        value: '404',
      },
    },
  },
};
