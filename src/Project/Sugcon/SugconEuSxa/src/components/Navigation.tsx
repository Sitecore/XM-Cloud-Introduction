import React, { useState } from 'react';
import {
  Link,
  LinkField,
  Text,
  TextField,
  useSitecoreContext,
} from '@sitecore-jss/sitecore-jss-nextjs';

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
  handleClick: (event?: React.MouseEvent<HTMLElement>) => void;
  relativeLevel: number;
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
    title: getLinkTitle(props),
    querystring: props.fields.Querystring,
  },
});

export const Default = (props: NavigationProps): JSX.Element => {
  const [isOpenMenu, openMenu] = useState(false);
  const { sitecoreContext } = useSitecoreContext();
  const styles =
    props.params != null
      ? `${props.params.GridParameters ?? ''} ${props.params.Styles ?? ''}`.trimEnd()
      : '';
  const id = props.params != null ? props.params.RenderingIdentifier : null;

  if (!Object.values(props.fields).length) {
    return (
      <div className={`component navigation ${styles}`} id={id ? id : undefined}>
        <div className="component-content">[Navigation]</div>
      </div>
    );
  }

  const handleToggleMenu = (event?: React.MouseEvent<HTMLElement>, flag?: boolean): void => {
    if (event && sitecoreContext?.pageEditing) {
      event.preventDefault();
    }

    if (flag !== undefined) {
      return openMenu(flag);
    }

    openMenu(!isOpenMenu);
  };

  const list = Object.values(props.fields)
    .filter((element) => element)
    .map((element: Fields, key: number) => (
      <NavigationList
        key={`${key}${element.Id}`}
        fields={element}
        handleClick={(event: React.MouseEvent<HTMLElement>) => handleToggleMenu(event, false)}
        relativeLevel={1}
      />
    ));

  return (
    <>
      {/* <div className={`component navigation ${styles}`} id={id ? id : undefined}>
        <label className="menu-mobile-navigate-wrapper">
          <input
            type="checkbox"
            className="menu-mobile-navigate"
            checked={isOpenMenu}
            onChange={() => handleToggleMenu()}
          />
          <div className="menu-humburger" />
          <div className="component-content">
            <nav>
              <ul className="clearfix">{list}</ul>
            </nav>
          </div>
        </label>
      </div> */}

      <div className={`component navigation ${styles}`}>
        <div className="component-content d-flex justify-content-end">
          <nav className="navbar navbar-expand-lg navbar-light">
            <button
              className="navbar-toggler ms-auto mb-3"
              type="button"
              data-toggle="collapse"
              data-target="#navbar"
              aria-controls="navbar"
              aria-expanded={!isOpenMenu ? true : false}
              aria-label="Toggle navigation"
              onClick={handleToggleMenu}
            >
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className={`${!isOpenMenu ? 'collapse ' : ''} navbar-collapse`} id="navbar">
              <div className="navbar-nav d-flex flex-row me-auto justify-content-end">
                <ul
                  className={`${
                    !isOpenMenu
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
    </>
  );
};

const NavigationList = (props: NavigationProps) => {
  const { sitecoreContext } = useSitecoreContext();
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
      <NavigationList
        key={`${index}${element.Id}`}
        fields={element}
        handleClick={props.handleClick}
        relativeLevel={props.relativeLevel + 1}
      />
    ));
  }

  return (
    <>
      <li className={props.fields.Styles.join(' ')} key={props.fields.Id} tabIndex={0}>
        <div className="navigation-title">
          <Link
            className="nav-link nav-item"
            field={getLinkField(props)}
            title={title}
            editable={sitecoreContext.pageEditing}
            onClick={props.handleClick}
          >
            {getNavigationText(props)}
          </Link>
        </div>
        {children.length > 0 ? <ul className="clearfix">{children}</ul> : null}
      </li>
    </>
  );
};

const getLinkTitle = (props: NavigationProps): string | undefined => {
  let title;
  if (props.fields.NavigationTitle?.value) {
    title = props.fields.NavigationTitle.value.toString();
  } else if (props.fields.Title?.value) {
    title = props.fields.Title.value.toString();
  } else {
    title = props.fields.DisplayName;
  }

  return title;
};
