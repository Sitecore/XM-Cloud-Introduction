import React from 'react';
import {
  Image as JssImage,
  Link as JssLink,
  ImageField,
  Field,
  LinkField,
  Text,
} from '@sitecore-jss/sitecore-jss-nextjs';

interface Fields {
  PromoIcon: ImageField;
  PromoText: Field<string>;
  PromoLink: LinkField;
  PromoText2: Field<string>;
}

type PromoProps = {
  params: { [key: string]: string };
  fields: Fields;
};

const PromoDefaultComponent = (props: PromoProps): JSX.Element => (
  <div className={`component promo ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">Promo</span>
    </div>
  </div>
);

export const Default = (props: PromoProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className={`component promo ${props.params.styles}`}>
        <div className="component-content card">
          <div className="card-img-top">
            <JssImage field={props.fields.PromoIcon} />
          </div>
          <div className="card-body">
            <div className="card-text">
              <Text className="image-caption" field={props.fields.PromoText} />
            </div>
            <div className="card-text">
              <JssLink field={props.fields.PromoLink} className="btn btn-light" />
            </div>
          </div>
        </div>
      </div>
    );
  }

  return <PromoDefaultComponent {...props} />;
};

export const WithText = (props: PromoProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className={`component promo ${props.params.styles}`}>
        <div className="component-content">
          <div className="field-promoicon">
            <JssImage field={props.fields.PromoIcon} />
          </div>
          <div className="promo-text">
            <div>
              <div className="field-promotext">
                <Text className="promo-text" field={props.fields.PromoText} />
              </div>
            </div>
            <div className="field-promotext">
              <Text className="promo-text" field={props.fields.PromoText2} />
            </div>
          </div>
        </div>
      </div>
    );
  }

  return <PromoDefaultComponent {...props} />;
};
