import React from 'react';
import { Field, LinkField, Link, RichText as JssRichText } from '@sitecore-jss/sitecore-jss-nextjs';
import { ComponentProps } from 'lib/component-props';

interface Fields {
  AnnouncementText: Field<string>;
  AnnouncementLink: LinkField;
}

type AnnouncementBarProps = ComponentProps & {
  params: { [key: string]: string };
  fields: Fields;
};

const AnnouncementBar = (props: AnnouncementBarProps): JSX.Element => (
  <div className={`component announcementbar ${props.params.styles}`}>
    <div className="component-content">
      <span className="is-empty-hint">AnnouncementBar Default view</span>
    </div>
  </div>
);

export const Default = (props: AnnouncementBarProps): JSX.Element => {
  if (props.fields) {
    const styles = `${props.params.GridParameters} ${props.params.Styles}`.trimEnd();

    return (
      <div className={`component announcementbar ${styles}`}>
        <div className="container">
          <div className="row">
            <div className="container component">
              <div className="row innercomponent">
                <div className="col-md-7 col-lg-8">
                  <JssRichText field={props.fields.AnnouncementText} />
                </div>
                <div className="col-auto cta-container">
                  <Link field={props.fields.AnnouncementLink}></Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

    );
  }
  return <AnnouncementBar {...props} />;
};
