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
  
  Category: Field<string>;
  
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
        <div className="component-content">
          <div className="field-promoicon">
            Image
          </div>
          <div className="promo-text">
            <div>
              <div className="field-promotext">
                <Text className="image-caption" field={props.fields.Category} />
              </div>
            </div>
            <div className="field-promolink">
              Link
            </div>
          </div>
        </div>
      </div>
    );
  }

  return <PromoDefaultComponent {...props} />;
};