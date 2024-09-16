import { Meta, StoryObj } from '@storybook/react';
import { HeroJustificationLetter } from 'components/Basic Components/Hero';

const meta = {
  title: 'Basic Components/Hero',
  component: HeroJustificationLetter,
  parameters: {
    layout: 'fullscreen',
  },
  tags: ['autodocs'],
} satisfies Meta<typeof HeroJustificationLetter>;

export default meta;

type Story = StoryObj<typeof meta>;

export const JustificationLetter: Story = {
  name: 'Hero Justification Letter',
  args: {
    rendering: {
      componentName: 'JustificationLetter',
    },
    params: {
      styles: '',
    },
    fields: {
      Headline: {
        value: 'Justification Letter',
      },
      EventDate: {
        value: '',
      },
      Text: {
        value:
          'Donec mauris diam, finibus gravida dolor et, accumsan auctor augue. Aliquam erat volutpat. Nullam fringilla suscipit lectus.',
      },
      CallToAction: {
        value: {
          href: 'https://www.sitecore.com',
          text: 'Download letter',
        },
      },
      Image: {
        value: {
          src: 'assets/images/SUGCON-justification-letter-chatbox-artwork.svg',
        },
      },
    },
  },
};
