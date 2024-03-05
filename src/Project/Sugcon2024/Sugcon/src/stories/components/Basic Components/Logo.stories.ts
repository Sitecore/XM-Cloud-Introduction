import { Meta, StoryObj } from '@storybook/react';
import { Default as LogoComponent } from '../../../components/Basic Components/Logo';
const meta = {
  title: 'Components/Logo',
  component: LogoComponent,
  parameters: {
    layout: 'centered',
  },
  tags: ['autodocs'],
  argTypes: {},
} satisfies Meta<typeof LogoComponent>;
export default meta;
type Story = StoryObj<typeof meta>;
export const Logo: Story = {
  name: 'Logo',
  args: {
    params: {
      styles: '',
    },
    fields: {
      Image: {
        value: {
          src: 'https://picsum.photos/400/200',
          alt: 'Logo',
          width: '200',
          height: '50',
        },
      },
      TargetUrl: {
        value: {
          href: 'https://www.sitecore.com',
          anchor: 'Home',
        },
      },
      ImageCaption: {
        value: 'SUGCON 2024 Logo',
      }
    },
  },
};
