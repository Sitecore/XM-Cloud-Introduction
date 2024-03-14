import { Meta, StoryObj } from '@storybook/react';
import { Default as VideoText } from '../../../components/Basic Components/VideoText';
const meta = {
  title: 'Basic Components/VideoText',
  component: VideoText,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof VideoText>;
export default meta;
type Story = StoryObj<typeof meta>;
export const VideoAndText: Story = {
  name: 'Video and Text',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Headline: {
        value: 'Sitecore Sessions',
      },
      Text: {
        value:
          "Megan Jensen leads a panel discussion with Perficient's Newest Sitecore MVPs: Emily Lord, Eric Sanner and Tiffany Laster about their experience in becoming new MVPs",
      },
      YoutubeVideoId: {
        value: 'uPyuI5UtmJc',
      },
      TextHeadline: {
        value: 'Episode 4',
      },
    },
  },
};
