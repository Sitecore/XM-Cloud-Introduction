import { extendTheme, ThemeConfig } from '@chakra-ui/react';
import { defineStyleConfig } from '@chakra-ui/react';
import { createMultiStyleConfigHelpers } from '@chakra-ui/styled-system';
import { PaddingX, PaddingY, Template } from 'components/Templates/LayoutConstants';

const Button = defineStyleConfig({
  baseStyle: {
    borderRadius: 'full',
    textDecoration: 'none',
  },
  sizes: {
    // Mobile
    sm: {
      fontSize: '14px',
      px: '16px',
      py: '8px',
    },
    // Tablet
    md: {
      fontSize: '16px',
      px: '24px',
      py: '12px',
    },
    // Desktop
    lg: {
      fontSize: '18px',
      px: '42px',
      py: '24px',
    },
  },
  // Two variants: outline and solid
  variants: {
    primary: {
      bg: '#EB1F1F',
      color: 'white',
      _hover: {
        // Hover state
        bg: 'white',
        color: '#EB1F1F',
        textDecoration: 'none',
      },
    },
    secondary: {
      bg: 'white',
      color: 'sugcon.blue',
      _hover: {
        bg: 'sugcon.blue',
        color: 'white',
      },
    },
    // Variant for Primary Navigation to make a Button look like a Link
    navButtonLink: {
      fontWeight: 'bold',
      color: 'gray.800',
      textDecoration: 'none',
      p: '0',
      transition: 'unset',
      minHeight: 'unset',
      height: 'unset',
      _hover: {
        color: '#878787',
        textDecoration: 'none',
      },
    },
  },
  // The default size and variant values
  defaultProps: {
    size: 'lg',
    variant: 'primary',
  },
});

const Link = defineStyleConfig({
  baseStyle: {
    fontSize: '18px',
    color: 'sugcon.blue',
    textDecoration: 'underline',
    _hover: {
      textDecoration: 'none',
    },
  },
  sizes: {
    sm: {
      fontSize: '16px',
    },
    lg: {
      fontSize: '18px',
    },
  },
  variants: {
    white: {
      color: 'white',
      fontSize: '18px',
    },
    smallWhite: {
      color: 'white',
      fontSize: '16px',
    },
    navLink: {
      fontSize: '18px',
      fontWeight: 'bold',
      color: 'sugcon.800',
      textDecoration: 'none',
    },
  },
  defaultProps: {
    size: 'lg',
  },
});

const LayoutFlex = defineStyleConfig({
  baseStyle: {
    width: 'full',
    maxW: Template.MaxWidth,
    px: { base: PaddingX.Mobile, lg: PaddingX.Desktop },
    py: { base: PaddingY.Mobile, lg: PaddingY.Desktop },
    my: { base: '10px', lg: '20px' },
    mx: 'auto',
  },
  sizes: {},
  variants: {},
  defaultProps: {},
});

const helpers = createMultiStyleConfigHelpers(['container', 'item']);

const List = helpers.defineMultiStyleConfig({
  baseStyle: {},
  sizes: {},
  variants: {
    reset: {
      item: {
        p: '0',
        m: '0',
      },
      container: {
        backgroundColor: 'green',
        p: '0',
        m: '0',
        marginInlineStart: '0',
      },
    },
  },
  defaultProps: {},
});

const config: ThemeConfig = {
  initialColorMode: 'light',
  useSystemColorMode: false,
};

const colors = {
  sugcon: {
    50: '#f7fafc',
    100: '#edf2f7',
    200: '#e2e8f0',
    300: '#cbd5e0',
    400: '#a0aec0',
    500: '#718096',
    600: '#4a5568',
    700: '#2d3748',
    800: '#1a202c',
    900: '#171923',
    blue: '#28327D',
    red: {
      100: '#AB0000',
    },
    gray: {
      100: '#f0f0f0',
      200: '#f2f2f2',
      300: '#cccccc',
      400: '#999999',
      500: '#707070',
      600: '#333333',
    },
  },
};

const fonts = {
  body: 'Mulish, sans-serif',
  heading: 'Mulish, sans-serif',
};

const styles = {
  global: {
    'html, body': {
      fontFamily: 'Mulish, sans-serif',
    },
  },
};

const theme = extendTheme({
  config,
  colors,
  components: {
    Button,
    Link,
    LayoutFlex,
    List,
  },
  fonts,
  styles,
});

export default theme;
