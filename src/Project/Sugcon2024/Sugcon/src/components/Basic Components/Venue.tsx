import React from 'react';
import {
  LinkField,
  RichTextField,
  TextField,
  RichText as JssRichText,
  Text as JssText,
  Item,
} from '@sitecore-jss/sitecore-jss-nextjs';
import { Box, Flex, Heading, Icon, Image, Stack } from '@chakra-ui/react';
import { ButtonLink } from 'src/basics/ButtonLink';
import Slider from 'react-slick';
import styled from '@emotion/styled';
import { LayoutFlex } from 'components/Templates/LayoutFlex';
import { PaddingX } from 'components/Templates/LayoutConstants';
import clsx from 'clsx';

// Define the fields that the Venue component will accept
interface Fields {
  Headline: TextField;
  HotelName: TextField;
  HotelAddress: RichTextField;
  AdditionalInfoTitle: TextField;
  AdditionalInfoText: RichTextField;
  ButtonLink: LinkField;
  VenueImages: Item[];
}

export type VenueProps = {
  params: { [key: string]: string };
  fields: Fields;
};

/**
 * Configuration for the react-slick component
 * Documentation: https://react-slick.neostack.com/
 * Styles for this component are defined in the SliderWrapper styled component
 */
const settings = {
  dots: true,
  arrows: false,
  fade: true,
  infinite: true,
  autoplay: true,
  speed: 500,
  autoplaySpeed: 5000,
  slidesToShow: 1,
  slidesToScroll: 1,
  appendDots: (dots: React.ReactNode) => (
    <div style={{ bottom: '10px' }}>
      <ul>{dots}</ul>
    </div>
  ),
  customPaging: () => (
    <Icon viewBox="0 0 200 200">
      <path fill="currentColor" d="M 100, 100 m -75, 0 a 75,75 0 1,0 150,0 a 75,75 0 1,0 -150,0" />
    </Icon>
  ),
};

export const Default = (props: VenueProps): JSX.Element => {
  const [slider, setSlider] = React.useState<Slider | null>(null);
  const id = props?.params?.RenderingIdentifier || undefined;

  return (
    <LayoutFlex
      id={id}
      className={clsx('component', props?.params?.styles)}
      flexDirection={{ base: 'column', lg: 'row' }}
      px={{ base: '0px', lg: PaddingX.Desktop }}
    >
      <Flex
        direction="column"
        minW={{ base: '100%', lg: '50%' }}
        pr={{ base: PaddingX.Mobile, lg: PaddingX.Desktop }}
        pl={{ base: PaddingX.Mobile, lg: '0px' }}
      >
        <Heading as="h2" fontSize="3xl" mb="21px">
          <JssText field={props?.fields?.Headline} />
        </Heading>

        <VenueInformationBlock
          FieldTitle={props?.fields?.HotelName}
          FieldText={props?.fields?.HotelAddress}
        />

        <VenueInformationBlock
          FieldTitle={props?.fields?.AdditionalInfoTitle}
          FieldText={props?.fields?.AdditionalInfoText}
        />

        <ButtonLink field={props?.fields?.ButtonLink} />
      </Flex>

      <Flex minW={{ base: '100%', lg: '50%' }}>
        {props?.fields?.VenueImages != null && (
          <Box
            minWidth={{ base: '100%', lg: '50%' }}
            position={'relative'}
            as={SliderWrapper}
            mt={{ base: '10', lg: '0' }}
          >
            <Slider {...settings} ref={() => setSlider(slider)}>
              {props?.fields?.VenueImages?.map((image, index) => (
                <Image
                  src={image?.url}
                  key={index}
                  borderRadius={{ base: 'none', lg: '2xl' }}
                  width="full"
                  height="100%"
                  maxHeight="400px"
                  objectFit="cover"
                />
              ))}
            </Slider>
          </Box>
        )}
      </Flex>
    </LayoutFlex>
  );
};

interface VenueInformationBlockProps {
  FieldTitle: TextField;
  FieldText: RichTextField;
}

const VenueInformationBlock = ({
  FieldTitle,
  FieldText,
}: VenueInformationBlockProps): JSX.Element => {
  return (
    <Stack mb="36px">
      <Heading as="h3" fontSize="20px" mb="0">
        <JssText field={FieldTitle} />
      </Heading>
      <JssRichText field={FieldText} />
    </Stack>
  );
};

/**
 * Use a styled component to apply custom styles to the Slider component instead of including external styles
 * CSS sources:
 * - https://cdnjs.cloudflare.com/ajax/libs/slick-carousel/1.6.0/slick.min.css
 * - https://cdnjs.cloudflare.com/ajax/libs/slick-carousel/1.6.0/slick-theme.min.css
 */
const SliderWrapper = styled('div')`
  /* Slider */
  .slick-slider {
    margin-bottom: 8px;
    position: relative;
    display: block;
    box-sizing: border-box;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
    -webkit-touch-callout: none;
    -khtml-user-select: none;
    -ms-touch-action: pan-y;
    touch-action: pan-y;
    -webkit-tap-highlight-color: transparent;
    overflow: hidden;
  }

  .slick-list {
    position: relative;

    display: block;
    /* overflow: hidden; */

    margin: 0;
    padding: 0;
  }
  .slick-list:focus {
    outline: none;
  }
  .slick-list.dragging {
    cursor: pointer;
    cursor: hand;
  }

  .slick-slider .slick-track,
  .slick-slider .slick-list {
    -webkit-transform: translate3d(0, 0, 0);
    -moz-transform: translate3d(0, 0, 0);
    -ms-transform: translate3d(0, 0, 0);
    -o-transform: translate3d(0, 0, 0);
    transform: translate3d(0, 0, 0);
  }

  .slick-track {
    position: relative;
    top: 0;
    left: 0;

    display: block;
    margin-left: auto;
    margin-right: auto;
  }
  .slick-track:before,
  .slick-track:after {
    display: table;

    content: '';
  }
  .slick-track:after {
    clear: both;
  }
  .slick-loading .slick-track {
    visibility: hidden;
  }

  .slick-slide {
    display: none;
    float: left;

    height: 100%;
    min-height: 1px;
  }
  [dir='rtl'] .slick-slide {
    float: right;
  }
  .slick-slide img {
    display: block;
  }
  .slick-slide.slick-loading img {
    display: none;
  }
  .slick-slide.dragging img {
    pointer-events: none;
  }
  .slick-initialized .slick-slide {
    display: block;
  }
  .slick-loading .slick-slide {
    visibility: hidden;
  }
  .slick-vertical .slick-slide {
    display: block;

    height: auto;

    border: 1px solid transparent;
  }
  .slick-arrow.slick-hidden {
    display: none;
  }

  /* Dots */
  .slick-dotted.slick-slider {
    margin-bottom: 30px;
  }

  .slick-dots {
    position: absolute;
    bottom: -10px;
    display: block;
    width: 100%;
    padding: 0;
    margin: 0;
    list-style: none;
    text-align: center;
  }
  .slick-dots li {
    position: relative;
    color: white;
    display: inline-block;
    width: 10px;
    height: 10px;
    margin: 0 5px;
    padding: 0;
    cursor: pointer;
    transition: width 0.3s ease-in-out;
  }
  .slick-dots li button {
    font-size: 0;
    line-height: 0;
    display: block;
    width: 10px;
    height: 10px;
    padding: 5px;
    cursor: pointer;
    color: transparent;
    border: 0;
    outline: none;
    background: transparent;
  }
  .slick-dots li button:hover,
  .slick-dots li button:focus {
    outline: none;
  }
  .slick-dots li button:hover:before,
  .slick-dots li button:focus:before {
    opacity: 1;
  }
  .slick-dots li button:before {
    font-family: 'slick';
    font-size: 6px;
    line-height: 20px;
    position: absolute;
    top: 0;
    left: 0;
    width: 10px;
    height: 10px;
    content: '•';
    text-align: center;
    opacity: 0.25;
    color: white;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
  }
  .slick-dots li.slick-active button:before {
    opacity: 0.75;
    color: white;
  }

  .slick-dots li {
    width: 14px;
    margin: 0 2px;
    transition: width 0.3s ease-in-out;
  }

  .slick-dots .slick-active {
    color: gray;
    transition: width 0.3s ease-in-out;
  }
`;
