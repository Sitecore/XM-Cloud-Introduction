import React, { useState } from 'react';
import {
  LinkField,
  Link as SitecoreLink,
  Text,
  TextField,
  useSitecoreContext,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { Box, Flex, UnorderedList, ListItem, Button } from '@chakra-ui/react';
import Link from 'next/link';

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

export type NavigationProps = {
  params?: { [key: string]: string };
  fields: Fields;
  handleClick: (event?: React.MouseEvent<HTMLElement>) => void;
  relativeLevel: number;
};

const homeFields: Fields = {
  Id: 'home',
  DisplayName: 'Home',
  Title: { value: 'Home', editable: 'Home' },
  NavigationTitle: { value: 'Home', editable: 'Home' },
  Href: '/',
  Querystring: '',
  Children: [],
  Styles: ['level1'], // Add any specific styles if needed
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
  const [isOpenMenu, setOpenMenu] = useState(false);
  const { sitecoreContext } = useSitecoreContext();
  const styles =
    props.params != null
      ? `${props.params.GridParameters ?? ''} ${props.params.Styles ?? ''}`.trimEnd()
      : '';
  const id = props.params != null ? props.params.RenderingIdentifier : null;

  console.log(props);

  if (!Object.values(props.fields).length) {
    return (
      <Box className={`component navigation ${styles}`} id={id || undefined}>
        <Box className="component-content">[Navigation]</Box>
      </Box>
    );
  }

  const handleToggleMenu = (event?: React.MouseEvent<HTMLElement>, flag?: boolean): void => {
    if (event && sitecoreContext?.pageEditing) {
      event.preventDefault();
    }
    if (flag !== undefined) {
      setOpenMenu(flag);
    } else {
      setOpenMenu(!isOpenMenu);
    }
  };

  const homeItem = (
    <NavigationList
      key="home"
      fields={homeFields}
      handleClick={(event: React.MouseEvent<HTMLElement>) => handleToggleMenu(event, false)}
      relativeLevel={1}
    />
  );

  const registerNow = (
    <Link href="/register" passHref>
      <Button variant="primary">Register now</Button>
    </Link>
  );

  const list = [
    homeItem, // Add the homeItem as the first element in the list
    ...Object.values(props.fields)
      .filter((element) => element)
      .map((element: Fields, key: number) => (
        <NavigationList
          key={`${key}${element.Id}`}
          fields={element}
          handleClick={(event: React.MouseEvent<HTMLElement>) => handleToggleMenu(event, false)}
          relativeLevel={1}
        />
      )),
    registerNow,
  ];

  return (
    <Box className={`component navigation ${styles}`} id={id || undefined}>
      <Box as="label" className="menu-mobile-navigate-wrapper">
        <input
          type="checkbox"
          className="menu-mobile-navigate"
          checked={isOpenMenu}
          onChange={() => handleToggleMenu()}
        />
        <Box className="menu-humburger" />
        <Box className="component-content">
          <nav>
            <UnorderedList className="clearfix">{list}</UnorderedList>
          </nav>
        </Box>
      </Box>
    </Box>
  );
};

const NavigationList = (props: NavigationProps) => {
  const { sitecoreContext } = useSitecoreContext();
  const [isActive, setIsActive] = useState(false);
  const classNameList = props.fields.Styles?.concat('rel-level' + props.relativeLevel).join(' ');

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
    <ListItem
      className={`${classNameList} ${isActive ? 'active' : ''}`}
      key={props.fields.Id}
      tabIndex={0}
    >
      <Flex
        className={`navigation-title ${children.length ? 'child' : ''}`}
        onClick={() => setIsActive(!isActive)}
      >
        <SitecoreLink
          field={getLinkField(props)}
          editable={sitecoreContext.pageEditing}
          onClick={props.handleClick}
        >
          {getNavigationText(props)}
        </SitecoreLink>
      </Flex>
    </ListItem>
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
