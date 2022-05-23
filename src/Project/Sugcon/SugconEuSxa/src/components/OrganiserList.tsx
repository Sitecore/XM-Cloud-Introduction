import { Text, Field,ImageField,Image,LinkField, Link,withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type OrganizerTeaserProps = ComponentProps & {
  fields: {
   // Image: ImageField;
   // Name: Field<string>;
   // LinkedIn: Field<string>;
   // Twitter: Field<string>;
   // LinkedInLink: LinkField;
   // TwitterLink: LinkField;
   Organizers: OrganizerProps[];
  };
};
export type OrganizerProps = ComponentProps & {
  fields: {
    Image: ImageField;
    Name: Field<string>;
    LinkedInLink: LinkField;
    TwitterLink: LinkField;
  }
};

const OrganizerTeaser = ({fields}: OrganizerTeaserProps): JSX.Element => (
<div className="container-fluid container-md mx-auto w-md-100">
  <div className='row'>
      {fields?.Organizers?.length == 0 ? (
          <div>No Organizers</div>
        ) : (
          fields?.Organizers?.map((Organizer) => {
              return (
                <div className='col-12 col-md-3 organizer' >
                  <div className='card'>
                    <p><Image field={Organizer?.fields?.Image}/></p>
                    <div className='cardBody'>
                      <h3><Text field={Organizer?.fields?.Name} /></h3>
                      <p>
                        <Link field={Organizer?.fields?.LinkedInLink}></Link><br/>
                        <Link field={Organizer?.fields?.TwitterLink}></Link>
                      </p>
                    </div>
                  </div>
                </div>
              )
            }
          )
        )
      }

  </div>
</div>
);

export default withDatasourceCheck()<OrganizerTeaserProps>(OrganizerTeaser);
