export enum HeaderHeights {
  Mobile = '75px',
  Desktop = '111px',
}

export enum Template {
  MaxWidth = '1440px',
}

export enum PaddingX {
  Mobile = '25px',
  Desktop = '35px',
}

export enum PaddingY {
  Mobile = '15px',
  Desktop = '25px',
}

type SizeConfig = {
  [key: string]: string;
};

export const responsiveButtonSizeConfig: SizeConfig = {
  base: 'lg',
  md: 'lg',
  lg: 'sm',
  xl: 'lg',
};
