import { extendTheme, ThemeConfig } from '@chakra-ui/react';
import { defineStyleConfig } from '@chakra-ui/react';

const Button = defineStyleConfig({
  baseStyle: {
    borderRadius: 'full',
  },
  sizes: {
    md: {
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
        bg: 'white', // Example hover background color
        color: '#EB1F1F', // Hover state text color (if you want to change it)
      },
    },
    secondary: {
      bg: 'white',
      color: '#374086',
      _hover: {
        // Hover state
        bg: '#374086', // Example hover background color
        color: 'white', // Hover state text color (if you want to change it)
      },
    },
  },
  // The default size and variant values
  defaultProps: {
    size: 'md',
    variant: 'primary',
  },
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
  },
  fonts,
  styles,
});

export default theme;
