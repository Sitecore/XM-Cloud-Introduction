import { Meta, StoryObj } from '@storybook/react';
import { Default as Agenda } from '../../../components/Events/Agenda';

const meta = {
    title: 'Events/Agenda',
    component: Agenda,
    tags: ['autodocs'],
    argTypes: {},
} satisfies Meta<typeof Agenda>;

export default meta;

type Story = StoryObj<typeof meta>;

export const AgendaStory: Story = {
    name: 'Agenda',
    args: {
        params: {
            styles: '',
        },
        fields: {
            SessionizeUrl: {
                value: "https://sessionize.com/api/v2/ekimnhe4/view/GridSmart" 
            }
        }
        
    }
};
