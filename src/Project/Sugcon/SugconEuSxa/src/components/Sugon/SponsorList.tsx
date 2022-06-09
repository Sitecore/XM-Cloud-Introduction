import React from 'react';
import { Field, Text, ImageField, Image } from '@sitecore-jss/sitecore-jss-nextjs';

interface Fields {
  Category: Field<string>;
  Sponsors: SponsorProps[];
}

type SponsorListProps = {
  params: { [key: string]: string };
  fields: Fields;
};

interface SponsorProps {
  fields: {
    SponsorName: Field<string>;
    SponsorUrl: Field<string>;
    SponsorLogo: ImageField;
  };
}

const SponsorListDefaultComponent = (props: SponsorListProps): JSX.Element => (
  <div className={`component sponsor-list ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Sponsor List Default!</span>
    </div>
  </div>
);

export const Default = (props: SponsorListProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className="container component">
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
                  <div key={Sponsor.fields.SponsorName.value} className="SponsorBlock">
                    <Text field={Sponsor.fields.SponsorName} />
                    <div className="SponsorImage">
                      <Image field={Sponsor?.fields.SponsorLogo} />
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

  return <SponsorListDefaultComponent {...props} />;
};

export const Platinum = (props: SponsorListProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className="container component">
        <h3>
          <Text field={props.fields?.Category} />
        </h3>
        {props.fields?.Sponsors?.length == 0 ? (
          <div>No Organizers</div>
        ) : (
          props.fields?.Sponsors?.map((Sponsor) => {
            return (
              <div key={Sponsor.fields.SponsorName.value} className="row">
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
