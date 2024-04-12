import {
  ComponentParams,
  ComponentRendering,
  LinkField,
  Placeholder,
  Link as SitecoreLink,
  Text,
  TextField,
  useSitecoreContext,
} from '@sitecore-jss/sitecore-jss-nextjs';
import {
  Box,
  Flex,
  Link,
  UnorderedList,
  ListItem,
  Button,
  IconButton,
  VisuallyHidden,
  useDisclosure,
  Collapse,
  Show,
} from '@chakra-ui/react';
import clsx from 'clsx';
import { ChevronDownIcon, ChevronUpIcon } from '@chakra-ui/icons';
import {
  HeaderHeights,
  PaddingX,
  PaddingY,
} from 'template/LayoutConstants';

export interface Fields {
  Id: string;
  DisplayName?: string;
  Title?: TextField;
  NavigationTitle: TextField;
  Href: string;
  Querystring: string;
  Children?: Fields[];
  Styles: string[];
}


export type NavigationProps = {
  params?: ComponentParams;
  fields: Fields;
  rendering: ComponentRendering;
  className?: string;
};

interface NavigationItemProps {
  element: Fields;
  key?: number;
  onToggle?: () => void;
  pageEditing: boolean | undefined;
}

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

export const Default = (props: NavigationProps): JSX.Element => {
  const {
    sitecoreContext: { pageEditing },
  } = useSitecoreContext();

  const { isOpen, onToggle } = useDisclosure();

  if (!Object.values(props.fields).length) {
    return (
      <Box
        className={clsx(
          'component',
          'navigation',
          props?.params?.GridParameters,
          props?.params?.Styles
        )}
        id={props.rendering.uid}
      >
        <Box className="component-content">[Navigation]</Box>
      </Box>
    );
  }

  const navigationMenuItems = [homeFields, ...Object.values(props.fields)];

  interface RenderNavigationProps {
    onToggle?: () => void;
  }

  const renderNavigation = ({
    onToggle = () => {
      /* intentionally left blank */
    },
  }: RenderNavigationProps = {}): JSX.Element => (
    <ResponsiveNavigation
      navigationMenuItems={navigationMenuItems}
      onToggle={onToggle}
      pageEditing={pageEditing}
      rendering={props.rendering}
    />
  );

  return (
    <>
      {/* Desktop Nav Items */}
      <Show above="lg">{renderNavigation()}</Show>

      {/* Mobile Content */}
      <Show below="lg">
        {/* Hamburgler */}
        <MobileHamburgerIconButton isOpen={isOpen} onToggle={onToggle} />

        {/* Mobile Nav Items */}
        <Collapse in={isOpen} animateOpacity>
          <Box
            id="mobile-menu"
            p={4}
            bg="white"
            pos="absolute"
            top={HeaderHeights.Mobile}
            left={0}
            right={0}
            w="full"
            maxH={`calc(100vh - ${HeaderHeights.Mobile})`}
            overflowY="scroll"
            shadow="lg"
            borderBlockEnd="1px"
            borderBlockEndColor="sugcon.gray.300"
          >
            {renderNavigation({ onToggle })}
          </Box>
        </Collapse>
      </Show>
    </>
  );
};

interface ResponsiveNavigationProps {
  navigationMenuItems: Fields[];
  onToggle?: () => void;
  pageEditing: boolean | undefined;
  rendering: ComponentRendering;
}

const ResponsiveNavigation = ({
  navigationMenuItems,
  onToggle,
  pageEditing,
  rendering
}: ResponsiveNavigationProps): JSX.Element => {
  return (
    <Box as="nav" role="navigation" aria-label="SUGCON" ml={{ base: 0, lg: 'auto' }}>
      <UnorderedList
        display={{ base: 'block', lg: 'flex' }}
        flexWrap="wrap"
        mb="0"
        pl="0"
        ml="0"
        textAlign={{ base: 'center', lg: 'unset' }}
        className="navigationContainer"
      >
        {navigationMenuItems
          .filter((element) => element)
          .map((element, key) => {
            const hasChildren = !!element?.Children?.length;
            return (
              <ListItem
                key={key}
                display="flex"
                alignItems="center"
                justifyContent="center"
                position="relative"
                mr={{ base: '0', lg: PaddingX.Desktop }}
                mb={{ base: PaddingY.Desktop, lg: '0' }}
              >
                {hasChildren ? (
                  <NavigationItemWithChildren
                    element={element}
                    onToggle={onToggle}
                    pageEditing={pageEditing}
                  />
                ) : (
                  <NavigationItem element={element} onToggle={onToggle} pageEditing={pageEditing} />
                )}
              </ListItem>
            );
          })}
        <ListItem>
          <Placeholder name="button-link" rendering={rendering} />
        </ListItem>
      </UnorderedList>
    </Box>
  );
};

const NavigationItemWithChildren = ({
  element,
  onToggle,
  pageEditing,
}: NavigationItemProps): JSX.Element => {
  const { isOpen, getDisclosureProps, getButtonProps } = useDisclosure();

  const buttonProps = getButtonProps();
  const disclosureProps = getDisclosureProps();
  return (
    <Flex flexDirection="column">
      <Button
        {...buttonProps}
        variant="navButtonLink"
        size={{ base: 'navButtonLinkSm', lg: 'navButtonLinkLg' }}
        mr={{ base: '-23px', lg: '0' }}
        rightIcon={isOpen ? <ChevronUpIcon /> : <ChevronDownIcon />}
        className={clsx(element?.Styles)}
      >
        {getNavigationText(element)}
      </Button>
      <Box position={{ base: 'relative', lg: 'absolute' }} top="100%" width="3xs" minWidth="3xs">
        <Collapse id={disclosureProps.id} in={isOpen} animateOpacity>
          <UnorderedList
            py="5px"
            px="0"
            mx="0"
            mt={{ base: '10px', lg: '0' }}
            rounded="md"
            border={{ base: '0', lg: '1px' }}
            backgroundColor={{ base: 'gray.100', lg: 'white' }}
            borderColor={{ base: 'unset', lg: 'sugcon.gray.300' }}
          >
            {!!element?.Children?.length &&
              element.Children.map((child, key) =>
                renderChildNavigationItem({ element: child, key, onToggle, pageEditing })
              )}
          </UnorderedList>
        </Collapse>
      </Box>
    </Flex>
  );
};

const NavigationItem = ({ element, onToggle, pageEditing }: NavigationItemProps): JSX.Element => {
  return (
    <Link
      as={SitecoreLink}
      className={clsx(element?.Styles)}
      field={getLinkField(element)}
      editable={pageEditing}
      variant="navLink"
      size={{ base: 'sm', lg: 'lg' }}
      onClick={onToggle}
    >
      {getNavigationText(element)}
    </Link>
  );
};

// Render-function for rendering a single Sub-Menu child element as a ListItem with a Link.
const renderChildNavigationItem = ({
  element,
  key,
  onToggle,
  pageEditing,
}: NavigationItemProps): JSX.Element => (
  <ListItem key={key} width="100%">
    <Link
      as={SitecoreLink}
      variant="navLink"
      size={{ base: 'sm', lg: 'lg' }}
      className={clsx(element?.Styles)}
      field={getLinkField(element)}
      editable={pageEditing}
      display="block"
      px="12px"
      py="6px"
      width="100%"
      _hover={{
        base: { backgroundColor: 'unset' },
        lg: { backgroundColor: 'sugcon.gray.100' },
      }}
      onClick={onToggle}
    >
      {getNavigationText(element)}
    </Link>
  </ListItem>
);

interface MobileHamburgerIconButtonProps {
  isOpen?: boolean;
  onToggle?: () => void;
}

const MobileHamburgerIconButton = ({
  isOpen,
  onToggle,
}: MobileHamburgerIconButtonProps): JSX.Element => {
  const ariaText = `${isOpen ? 'Close' : 'Open'} Mobile Menu`;

  return (
    <IconButton
      display={{ base: 'flex', lg: 'none' }}
      h="30px"
      width="35px"
      ml="auto"
      variant="link"
      onClick={onToggle}
      aria-label={ariaText}
      aria-controls="mobile-menu"
      aria-expanded={isOpen}
      aria-haspopup="true"
      icon={
        <>
          <VisuallyHidden>{ariaText}</VisuallyHidden>
          <div id="mobile-hamburger-icon" className={clsx({ open: isOpen })}>
            <span />
            <span />
            <span />
            <span />
          </div>
        </>
      }
    />
  );
};

// Constructs a LinkField object with href, title, and querystring properties from an element.
const getLinkField = (element: Fields): LinkField => ({
  value: {
    href: element?.Href,
    title: getLinkTitle(element),
    querystring: element?.Querystring,
  },
});

// Returns the most appropriate title string from an element, based on available fields.
const getLinkTitle = (element: Fields): string | undefined =>
  element?.NavigationTitle?.value?.toString() ||
  element?.Title?.value?.toString() ||
  element?.DisplayName;

// Renders a Text component with the first available title field or returns the display name.
const getNavigationText = (element: Fields): JSX.Element | string => {
  const field = element?.NavigationTitle || element?.Title;
  return field ? <Text field={field} /> : element?.DisplayName || '';
};
