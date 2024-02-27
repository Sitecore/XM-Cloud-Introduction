import React from 'react';
import {
  Link as JssLink,
  Text,
  LinkField,
  TextField,
  Field,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { fab } from '@fortawesome/free-brands-svg-icons';
// eslint-disable-next-line @typescript-eslint/no-var-requires
const { library } = require('@fortawesome/fontawesome-svg-core');

library.add(fas, far, fab);

type IconLinkListProps = {
  params: { [key: string]: string };
  fields: {
    Title: TextField;
    items: IconLink[];
  };
};

type IconLink = {
  fields: {
    Link: LinkField;
    Icon: Icon;
  };
};

type Icon = {
  fields: {
    CssClass: TextField;
  };
};

type IconLinkListItemProps = {
  key: string;
  index: number;
  total: number;
  field: LinkField;
  icon: TextField;
};

const IconLinkListItem = (props: IconLinkListItemProps) => {
  let className = `item${props.index}`;
  className += (props.index + 1) % 2 == 0 ? ' even' : ' odd';
  if (props.index == 0) {
    className += ' first';
  }
  if (props.index + 1 == props.total) {
    className += ' last';
  }

  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  // @ts-ignore
  const iconprop: IconProp = props?.icon?.value?.toString() ?? '';

  if (props?.icon?.value) {
    return (
      <li className={className}>
        <div className="field-link">
          <JssLink field={props.field} className="icon-list-link">
            <FontAwesomeIcon icon={iconprop} />
          </JssLink>
        </div>
      </li>
    );
  }

  return (
    <li className={className}>
      <div className="field-link">
        <JssLink field={props.field} />
      </div>
    </li>
  );
};

export const Default = (props: IconLinkListProps): JSX.Element => {
  const styles = `component icon-link-list ${props.params.styles}`.trimEnd();
  const id = props.params.RenderingIdentifier;

  if (props?.fields?.items?.length > 0) {
    const list = props?.fields?.items
      .filter((element: IconLink) => element?.fields?.Link)
      .map((element: IconLink, key: number) => (
        <IconLinkListItem
          index={key}
          key={`${key}${element.fields.Link}`}
          total={props.fields.items.length}
          field={element.fields.Link}
          icon={element.fields.Icon?.fields?.CssClass}
        />
      ));

    return (
      <div className={styles} id={id ? id : undefined}>
        <div className="component-content">
          <Text tag="h3" field={props?.fields?.Title} />
          <ul>{list}</ul>
        </div>
      </div>
    );
  }

  return (
    <div className={styles} id={id ? id : undefined}>
      <div className="component-content">
        <h3>Icon Link List</h3>
      </div>
    </div>
  );
};
