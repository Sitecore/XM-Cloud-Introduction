import { Meta, StoryObj } from '@storybook/react';
import { Default as Sessions } from '../../../components/Events/Sessions';

const meta = {
    title: 'Events/Sessions',
    component: Sessions,
    tags: ['autodocs'],
    argTypes: {},
} satisfies Meta<typeof Sessions>;

export default meta;

type Story = StoryObj<typeof meta>;

export const SessionsStory: Story = {
    name: 'Sessions',
    args: {
        params: {
            styles: '',
        },
        fields: {
            SessionizeUrl: {
                value: "https://sessionize.com/api/v2/ekimnhe4/view/Sessions" 
            }
        }
        
    }
};
