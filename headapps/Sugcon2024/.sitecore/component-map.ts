// Below are built-in components that are available in the app, it's recommended to keep them as is

import { BYOCServerWrapper, NextjsContentSdkComponent, FEaaSServerWrapper } from '@sitecore-content-sdk/nextjs';
import { Form } from '@sitecore-content-sdk/nextjs';

// end of built-in components
import * as Title from 'src/components/title/Title';
import * as RowSplitter from 'src/components/row-splitter/RowSplitter';
import * as RichText from 'src/components/rich-text/RichText';
import * as Promo from 'src/components/promo/Promo';
import * as PartialDesignDynamicPlaceholder from 'src/components/partial-design-dynamic-placeholder/PartialDesignDynamicPlaceholder';
import * as PageContent from 'src/components/page-content/PageContent';
import * as Navigation from 'src/components/navigation/Navigation';
import * as IconLinkList from 'src/components/list-components/icon-link-list/IconLinkList';
import * as LinkList from 'src/components/link-list/LinkList';
import * as Image from 'src/components/image/Image';
import * as ContentBlock from 'src/components/content-block/ContentBlock';
import * as Container from 'src/components/container/Container';
import * as ColumnSplitter from 'src/components/column-splitter/ColumnSplitter';
import * as Logo from 'src/components/basic-components/logo/Logo';
import * as LinkButton from 'src/components/basic-components/link/Link.Button';
import * as Link from 'src/components/basic-components/link/Link';
import * as ErrorMessage from 'src/components/basic-components/error-message/ErrorMessage';
import * as LayoutConstants from 'src/components/Templates/LayoutConstants';
import * as LayoutFlex from 'src/components/Templates/layout-flex/LayoutFlex';
import * as Header from 'src/components/Templates/header/Header';
import * as Footer from 'src/components/Templates/footer/Footer';

export const componentMap = new Map<string, NextjsContentSdkComponent>([
  ['BYOCWrapper', BYOCServerWrapper],
  ['FEaaSWrapper', FEaaSServerWrapper],
  ['Form', Form],
  ['Title', { ...Title }],
  ['RowSplitter', { ...RowSplitter }],
  ['RichText', { ...RichText }],
  ['Promo', { ...Promo }],
  ['PartialDesignDynamicPlaceholder', { ...PartialDesignDynamicPlaceholder }],
  ['PageContent', { ...PageContent }],
  ['Navigation', { ...Navigation, componentType: 'client' }],
  ['IconLinkList', { ...IconLinkList }],
  ['LinkList', { ...LinkList }],
  ['Image', { ...Image }],
  ['ContentBlock', { ...ContentBlock }],
  ['Container', { ...Container }],
  ['ColumnSplitter', { ...ColumnSplitter }],
  ['Logo', { ...Logo }],
  ['Link', { ...LinkButton, ...Link }],
  ['ErrorMessage', { ...ErrorMessage }],
  ['LayoutConstants', { ...LayoutConstants }],
  ['LayoutFlex', { ...LayoutFlex }],
  ['Header', { ...Header }],
  ['Footer', { ...Footer }],
]);

export default componentMap;
