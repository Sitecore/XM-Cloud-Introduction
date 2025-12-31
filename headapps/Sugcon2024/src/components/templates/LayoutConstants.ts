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

export const layoutFlexStyles = {
  width: 'full',
  maxW: Template.MaxWidth,
  px: { base: PaddingX.Mobile, lg: PaddingX.Desktop },
  py: { base: PaddingY.Mobile, lg: PaddingY.Desktop },
  my: { base: '10px', lg: '20px' },
  mx: 'auto',
} as const;

type SizeConfig = {
  [key: string]: string;
};

export const responsiveButtonSizeConfig: SizeConfig = {
  base: 'lg',
  md: 'lg',
  lg: 'sm',
  xl: 'lg',
};
