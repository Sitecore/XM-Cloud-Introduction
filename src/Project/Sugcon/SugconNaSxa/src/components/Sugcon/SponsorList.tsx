import React from 'react';
import {
  Field,
  Text,
  RichText,
  ImageField,
  Image,
  RichTextField,
  Link,
  LinkField,
} from '@sitecore-jss/sitecore-jss-nextjs';

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
    SponsorDescription: RichTextField;
    SponsorUrlLink: LinkField;
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
      <div className={`component sponsor-list default ${props.params.styles}`}>
        <div className="component-content container">
          <div className="row">
            <div className="col-12 sponsor--default">
              <h2>
                <Text field={props.fields?.Category} />
              </h2>
              <div className="row">
                {props.fields?.Sponsors?.length == 0 ? (
                  <div>No Organizers</div>
                ) : (
                  props.fields?.Sponsors?.map((Sponsor) => {
                    return (
                      <div
                        key={Sponsor.fields.SponsorName.value}
                        className="col-6 col-md-4 col-lg-3 sponsor--default__block"
                      >
                        <div className="sponsor--default__img">
                          <Image field={Sponsor?.fields.SponsorLogo} />
                        </div>
                      </div>
                    );
                  })
                )}
              </div>
            </div>
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
      <div className={`component sponsor-list sponsor-platinum ${props.params.styles}`}>
        <div className="component-content container">
          <div className="row">
            <h2>
              <Text field={props.fields?.Category} />
            </h2>
            <div className="row">
            {props.fields?.Sponsors?.length == 0 ? (
              <div>No Organizers</div>
            ) : (
              props.fields?.Sponsors?.map((Sponsor) => {
                return (
                  <div key={Sponsor.fields.SponsorName.value} className="row">
                    <div className="col-12 col-md-6 sponsor--platinum__block">
                      <p className="sponsor-platinum__label">
                        <Text field={props?.fields?.Category} />
                      </p>
                      <h3 className="sponsor-platinum__name">
                        <Text field={Sponsor?.fields?.SponsorName} />
                      </h3>
                      <br />
                      <RichText field={Sponsor?.fields?.SponsorDescription} />
                      {Sponsor?.fields?.SponsorUrlLink?.value?.href ? (
                        <Link field={Sponsor?.fields?.SponsorUrlLink}></Link>
                      ) : (
                        ''
                      )}
                    </div>
                    <div className="col-12 col-md-6">
                      <div className="sponsor--platinum__img--outer">
                        <div className="sponsor--platinum__img">
                          <picture>
                            <Image field={Sponsor?.fields?.SponsorLogo} />
                          </picture>
                        </div>
                      </div>
                    </div>
                  </div>
                );
              })
            )}
            </div>
          </div>
        </div>
      </div>
    );
  }

  return <SponsorListDefaultComponent {...props} />;
};

export const Secondary = (props: SponsorListProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className={`component sponsor-list default ${props.params.styles}`}>
        <div className="component-content container">
          <div className="row">
            <div className="col-12 sponsor--secondary">
              <h2>
                <Text field={props.fields?.Category} />
              </h2>
              <div className="row">
                {props.fields?.Sponsors?.length == 0 ? (
                  <div>No Organizers</div>
                ) : (
                  props.fields?.Sponsors?.map((Sponsor) => {
                    return (
                      <div
                        key={Sponsor.fields.SponsorName.value}
                        className="col-12 col-md-4 col-lg-4 sponsor--secondary__block"
                      >
                        <div className="sponsor--secondary__img">
                          <Image field={Sponsor?.fields.SponsorLogo} />
                        </div>
                        <RichText field={Sponsor?.fields?.SponsorDescription} />
                        {Sponsor?.fields?.SponsorUrlLink?.value?.href ? (
                          <Link field={Sponsor?.fields?.SponsorUrlLink}></Link>
                        ) : (
                          ''
                        )}
                      </div>
                    );
                  })
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  return <SponsorListDefaultComponent {...props} />;
};
