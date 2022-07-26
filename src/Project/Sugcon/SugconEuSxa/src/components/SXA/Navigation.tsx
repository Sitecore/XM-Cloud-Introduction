import React, { useState } from 'react';
import { Link, LinkField, Text, TextField } from '@sitecore-jss/sitecore-jss-nextjs';

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

type NavigationProps = {
  params?: { [key: string]: string };
  fields: Fields;
};

const getNavigationText = function (props: NavigationProps): JSX.Element | string {
  let text;

  if (props.fields.NavigationTitle) {
    text = <Text field={props.fields.NavigationTitle} />;
  } else if (props.fields.Title) {
    text = <Text field={props.fields.Title} />;
  } else {
    text = props.fields.DisplayName;
  }

  return text;
};

const getLinkField = (props: NavigationProps): LinkField => ({
  value: {
    href: props.fields.Href,
    title: props.fields.DisplayName,
    querystring: props.fields.Querystring,
  },
});

const Navigation = (props: NavigationProps): JSX.Element => {
  const [isNavCollapsed, setIsNavCollapsed] = useState(true);
  const handleNavCollapse = () => setIsNavCollapsed(!isNavCollapsed);

  if (!Object.values(props.fields).length) {
    return (
      <div className={`component navigation`}>
        <div className="component-content">[Navigation]</div>
      </div>
    );
  }

  const list = Object.values(props.fields)
    .filter((element) => element)
    .map((element: Fields, key: number) => (
      <NavigationList key={`${key}${element.Id}`} fields={element} />
    ));

  const styles =
    props.params != null ? `${props.params.GridParameters} ${props.params.Styles}` : null;

  return (
    <div className={`component navigation ${styles}`}>
      <div className="component-content d-flex justify-content-end">
        <nav className="navbar navbar-expand-lg navbar-light">
          <button
            className="navbar-toggler ms-auto mb-3"
            type="button"
            data-toggle="collapse"
            data-target="#navbar"
            aria-controls="navbar"
            aria-expanded={!isNavCollapsed ? true : false}
            aria-label="Toggle navigation"
            onClick={handleNavCollapse}
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className={`${isNavCollapsed ? 'collapse ' : ''} navbar-collapse`} id="navbar">
            <div className="navbar-nav d-flex flex-row me-auto justify-content-end">
              <ul
                className={`${
                  isNavCollapsed
                    ? 'd-flex flex-row me-auto justify-content-end'
                    : 'd-flex flex-column'
                } nav-item`}
              >
                {list}
              </ul>
            </div>
          </div>
        </nav>
      </div>
    </div>
  );
};

const NavigationList = (props: NavigationProps) => {
  let title;
  if (props.fields.NavigationTitle) {
    title = props.fields.NavigationTitle.value?.toString();
  } else if (props.fields.Title) {
    title = props.fields.Title.value?.toString();
  } else {
    title = props.fields.DisplayName;
  }

  let children: JSX.Element[] = [];
  if (props.fields.Children && props.fields.Children.length) {
    children = props.fields.Children.map((element: Fields, index: number) => (
      <NavigationList key={`${index}${element.Id}`} fields={element} />
    ));
  }

  return (
    <li className={props.fields.Styles.join(' ')} key={props.fields.Id}>
      <div className="navigation-title">
        <Link className="nav-link nav-item" field={getLinkField(props)} title={title}>
          {getNavigationText(props)}
        </Link>
      </div>
      {children.length > 0 ? <ul className="clearfix">{children}</ul> : null}
    </li>
  );
};

export default Navigation;
