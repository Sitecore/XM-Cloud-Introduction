export const enum HeaderHeights {
  Mobile = '75px',
  Desktop = '111px',
}

export const enum Template {
  MaxWidth = '1440px',
}

export const enum PaddingX {
  Mobile = '25px',
  Desktop = '35px',
}

export const enum PaddingY {
  Mobile = '15px',
  Desktop = '25px',
}

type SizeConfig = {
  [key: string]: string;
};

export const responsiveButtonSizeConfig: SizeConfig = {
  base: 'sm',
  md: 'sm',
  lg: 'sm',
  xl: 'lg',
};
