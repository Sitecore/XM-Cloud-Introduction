'use client';
import React, { useState, JSX } from 'react';
import { Link, LinkField, Text, TextField, useSitecore } from '@sitecore-content-sdk/nextjs';
import { ComponentProps } from 'lib/component-props';

interface Fields {
  Id: string;
  DisplayName: string;
  Title: TextField;
  NavigationTitle: TextField;
  Href: string;
  Querystring: string;
  Children: Array<Fields>;
  Styles: string[];
}

interface NavigationListItemProps {
  fields: Fields;
  handleClick: (event?: React.MouseEvent<HTMLElement>) => void;
  relativeLevel: number;
}

interface NavigationProps extends ComponentProps {
  fields: Fields;
}

const getTextContent = (fields: Fields): JSX.Element | string => {
  if (fields.NavigationTitle) return <Text field={fields.NavigationTitle} />;
  if (fields.Title) return <Text field={fields.Title} />;
  return fields.DisplayName;
};

const getLinkField = (fields: Fields): LinkField => ({
  value: {
    href: fields.Href,
    title:
      fields.NavigationTitle?.value?.toString() ??
      fields.Title?.value?.toString() ??
      fields.DisplayName,
    querystring: fields.Querystring,
  },
});

const NavigationListItem: React.FC<NavigationListItemProps> = ({
  fields,
  handleClick,
  relativeLevel,
}) => {
  const [isActive, setIsActive] = useState(false);
  const { page } = useSitecore();

  const classNames = [...fields.Styles, `rel-level${relativeLevel}`, isActive ? 'active' : ''].join(
    ' '
  );

  const hasChildren = fields.Children?.length > 0;
  const children = hasChildren
    ? fields.Children.map((fields, index) => (
        <NavigationListItem
          key={`${index}-${fields.Id}`}
          fields={fields}
          handleClick={handleClick}
          relativeLevel={relativeLevel + 1}
        />
      ))
    : null;

  return (
    <li className={classNames} key={fields.Id} tabIndex={0}>
      <div
        className={`navigation-title ${hasChildren ? 'child' : ''}`}
        onClick={() => setIsActive(!isActive)}
      >
        <Link field={getLinkField(fields)} editable={page.mode.isEditing} onClick={handleClick}>
          {getTextContent(fields)}
        </Link>
      </div>
      {hasChildren && <ul className="clearfix">{children}</ul>}
    </li>
  );
};

export const Default = ({ params, fields }: NavigationProps) => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const { page } = useSitecore();
  const { styles, RenderingIdentifier: id } = params;

  if (!Object.values(fields).length) {
    return (
      <div className={`component navigation ${styles}`} id={id}>
        <div className="component-content">[Navigation]</div>
      </div>
    );
  }

  const handleToggleMenu = (event?: React.MouseEvent<HTMLElement>, forceState?: boolean) => {
    if (event && page.mode.isEditing) {
      event.preventDefault();
    }

    setIsMenuOpen(forceState ?? !isMenuOpen);
  };

  const navigationItems = Object.values(fields)
    .filter(Boolean)
    .map((item: Fields, index) => (
      <NavigationListItem
        key={`${index}-${item.Id}`}
        fields={item}
        handleClick={(event) => handleToggleMenu(event, false)}
        relativeLevel={1}
      />
    ));

  return (
    <div className={`component navigation ${styles}`} id={id}>
      <label className="menu-mobile-navigate-wrapper">
        <input
          type="checkbox"
          className="menu-mobile-navigate"
          checked={isMenuOpen}
          onChange={() => handleToggleMenu()}
        />
        <div className="menu-humburger" />
        <div className="component-content">
          <nav>
            <ul className="clearfix">{navigationItems}</ul>
          </nav>
        </div>
      </label>
    </div>
  );
};
