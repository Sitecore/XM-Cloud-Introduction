import { Meta, StoryObj } from '@storybook/react';
import { Default as Accordion } from '../../../components/List Components/Accordion';

const meta = {
  title: 'List Components/Accordion',
  component: Accordion,
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof Accordion>;

export default meta;

type Story = StoryObj<typeof meta>;

const element = {
  fields: {
    Title: {
      value:
        'Donec mauris diam, finibus gravida dolor et, accumsan auctor augue. Aliquam erat volutpat. Nullam fringilla suscipit lectus?',
    },
    Text: {
      value:
        'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer a metus imperdiet ante tempor lacinia id id turpis. Sed vel diam sit amet mauris eleifend suscipit. Mauris odio risus, consequat in aliquet eget, tempus id mi. Quisque vestibulum ac enim fermentum gravida.',
    },
  },
};

export const Event: Story = {
  name: 'Accordion',
  args: {
    rendering: {
      componentName: 'Event',
    },
    params: {
      styles: '',
    },
    fields: {
      Headline: {
        value: 'FAQs',
      },
      Elements: new Array(4).fill(element),
    },
  },
};
