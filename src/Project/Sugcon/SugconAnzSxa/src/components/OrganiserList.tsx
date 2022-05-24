import React from 'react';
import { Text, 
         Field, 
         ImageField,
         LinkField,
         withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

interface Fields {
  heading: Field<string>;
  Organizers: OrganizerProps[];
}

type OrganiserListProps = ComponentProps & {
  params: { [key: string]: string };
  fields: Fields;
};

export type OrganizerProps = {
  fields: {
    Image: ImageField;
    Name: Field<string>;
    LinkedIn: Field<string>;
    Twitter: Field<string>;
    LinkedInLink: LinkField;
    TwitterLink: LinkField;
  }
};

const OrganiserList = (props: OrganiserListProps): JSX.Element => (
  <div>
    <p>OrganiserList Component</p>
    <Text field={props.fields.heading} />
  </div>
);

export const Default = (props: OrganiserListProps ): JSX.Element => {
  if (props.fields) {
    return (
      
      <div className="container component"> 
      Organiser List: {props.fields?.Organizers?.length}
        {props.fields?.Organizers?.length == 0 ? ( 
          <div>No Organisers</div>
          ) : ( 
            props.fields?.Organizers?.map((Organizer) => { 
              return (
                <div className="row">
                  Next
                  <div className="col-12">
                    Hello You: <Text field= {Organizer.fields.Name}/>
                  </div>
                </div>
              )
                
            }
            ) 
          )
        }
      </div>
      
    );
  }

  return <OrganiserList {...props} />;
};

