import { Meta, StoryObj } from '@storybook/react';
import { Default as SpeakersGrid } from '../../../components/List Components/SpeakersGrid';
import '../../../assets/sass/components/people-grid/index.scss';
const meta = {
  title: 'Components/SpeakersGrid',
  component: SpeakersGrid,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof SpeakersGrid>;
export default meta;
type Story = StoryObj<typeof meta>;
export const Teaser: Story = {
  name: 'Speakers Grid',
  args: {
    params: {
      Columns: '4',
      Alphabetize: '1',
      LinkToBio: '1',
      DisplaySocialLinks: '0',
      Styles: '',
    },
    fields: {
      Headline: {
        value: 'Speakers',
      },
      SessionsListTitle: {
        value: 'Sessions',
      },
      SessionizeSpeakerUrl: {
        value: '/speakers.html',
      },
    },
  },
};
