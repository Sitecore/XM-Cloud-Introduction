import React from 'react';
import { Text, 
         Field, 
         Image as JssImage,
         ImageField,
         Link as JssLink,
         LinkField,
         withDatasourceCheck } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

interface Fields {
  Category: Field<string>;
  
}

type SponsorListProps = {
  params: { [key: string]: string };
  fields: Fields;
};

const SponsorList = (props: SponsorListProps): JSX.Element => (
  <div className={`component sponsorlist ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">SponsorList Default</span>
    </div>
  </div>
);

export const Default = (props: SponsorListProps): JSX.Element => {
  if (props.fields) {
    return (
      <div className="container">
        <div className='row'>
          <div className='col-12 SponsorTeaser2' >
          
            <h3><Text field={props.fields?.Category} /></h3>
           
          </div>
        </div>
      </div>
    );
  }

  return <SponsorList {...props} />;
};






