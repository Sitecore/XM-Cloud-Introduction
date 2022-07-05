import React from 'react';
import { Text, Field, ImageField, LinkField, Link, Image } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

interface Fields {
  heading: Field<string>;
  Organisers: OrganizerProps[];
}

type OrganiserListProps = ComponentProps & {
  params: { [key: string]: string };
  fields: Fields;
};

interface OrganizerProps {
  fields: {
    Image: ImageField;
    Name: Field<string>;
    LinkedIn: Field<string>;
    Twitter: Field<string>;
    LinkedInLink: LinkField;
    TwitterLink: LinkField;
  };
}

const OrganiserList = (props: OrganiserListProps): JSX.Element => (
  <div>
    <p>OrganiserList Component</p>
    <Text field={props.fields.heading} />
  </div>
);

export const Default = (props: OrganiserListProps): JSX.Element => {
  console.log(props);
  if (props.fields) {
    return (
      <div className={`component organiser-list ${props.params.styles}`}>
        <div className="component-content container">
          <div className="row">
            {props.fields?.Organisers?.length == 0 ? (
              <div>No Organisers</div>
            ) : (
              props.fields?.Organisers?.map((Organizer) => {
                return (
                  <div
                    key={Organizer.fields.Name.value}
                    className="col-6 col-md-6 col-lg-3 organizer"
                  >
                    <div className="card">
                      <p>
                        <Image field={Organizer?.fields?.Image} />
                      </p>
                      <div className="cardBody">
                        <h3>
                          <Text field={Organizer?.fields?.Name} />
                        </h3>
                        <p>
                          <Link field={Organizer?.fields?.LinkedInLink}></Link> /
                          <Link field={Organizer?.fields?.TwitterLink}></Link>
                        </p>
                      </div>
                    </div>
                  </div>
                );
              })
            )}
          </div>
        </div>
      </div>
    );
  }

  return <OrganiserList {...props} />;
};
