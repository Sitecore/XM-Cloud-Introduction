import { Text, Field, ImageField, Image } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

type SponsorListProps = ComponentProps & {
  fields: {
    Category: Field<string>;
    Sponsors: SponsorProps[];
  };
};
export type SponsorProps = ComponentProps & {
  fields: {
    SponsorName: Field<string>;
    SponsorUrl: Field<string>;
    SponsorLogo: ImageField;
  };
};
const SponsorListDefaultComponent = (props: SponsorListProps): JSX.Element => (
  <div className={`component promo ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Promo</span>
    </div>
  </div>
);

export const Platinum = (props: SponsorListProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className="container-fluid container-md mx-auto w-md-100">
        {props.fields?.Sponsors?.length == 0 ? (
          <div>No Organizers</div>
        ) : (
          props.fields?.Sponsors?.map((Sponsor) => {
            return (
              <div key={Sponsor.fields.SponsorName.value} className="row sponsorTeaser">
                <div className="col-12 col-md-6">
                  <p className="sponsorLabel">
                    <Text field={props?.fields?.Category} />
                  </p>
                  <h2 className="sponsorName">
                    <Text field={Sponsor?.fields?.SponsorName} />
                  </h2>
                  <br />
                  <Text field={Sponsor?.fields?.SponsorUrl} />
                  <br />
                </div>
                <div className="col-12 col-md-6 sponsorTeaserImg ">
                  <div className="sponsorTeaserImg">
                    <Image field={Sponsor?.fields?.SponsorLogo} />
                  </div>
                </div>
              </div>
            );
          })
        )}
      </div>
    );
  }

  return <SponsorListDefaultComponent {...props} />;
};

export const Default = (props: SponsorListProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className="container-fluid container-md mx-auto w-md-100">
        <div className="row">
          <div className='col-12 className="SponsorImage2"'>
            <h3>
              <Text field={props.fields?.Category} />
            </h3>
            {props.fields?.Sponsors?.length == 0 ? (
              <div>No Organizers</div>
            ) : (
              props.fields?.Sponsors?.map((Sponsor) => {
                return (
                  <div key={Sponsor.fields.SponsorName.value} className="SponsorImage2">
                    <Image field={Sponsor?.fields?.SponsorLogo} />
                  </div>
                );
              })
            )}
          </div>
        </div>
      </div>
    );
  }

  return <SponsorListDefaultComponent {...props} />;
};
