import React from 'react';
import { Link as ContentSdkLink, Text, LinkField, TextField } from '@sitecore-content-sdk/nextjs';
import { ComponentProps } from 'lib/component-props';

interface LinkListProps extends ComponentProps {
  fields: {
    /**
     * The Integrated graphQL query result. This illustrates the way to access the datasource children.
     */
    data: {
      datasource: {
        children: {
          results: Array<{
            field: {
              link: LinkField;
            };
          }>;
        };
        field: {
          title: TextField;
        };
      };
    };
  };
}

const LinkListItem = ({
  index,
  total,
  field,
}: {
  index: number;
  total: number;
  field: LinkField;
}) => {
  const classNames = [
    `item${index}`,
    index % 2 === 0 ? 'odd' : 'even',
    index === 0 ? 'first' : '',
    index === total - 1 ? 'last' : '',
  ]
    .filter(Boolean)
    .join(' ');

  return (
    <li className={classNames}>
      <div className="field-link">
        <ContentSdkLink field={field} />
      </div>
    </li>
  );
};

export const Default = ({ params, fields }: LinkListProps) => {
  const datasource = fields?.data?.datasource;
  const styles = `component link-list ${params.styles || ''}`.trim();
  const id = params.RenderingIdentifier;

  const renderContent = () => {
    if (!datasource) {
      return <h3>Link List</h3>;
    }

    const links = datasource.children.results
      .filter((element) => element?.field?.link)
      .map((element, index) => (
        <LinkListItem
          key={`${index}-${element.field?.link}`}
          index={index}
          total={datasource.children.results.length}
          field={element.field.link}
        />
      ));

    return (
      <>
        <Text tag="h3" field={datasource.field?.title} />
        <ul>{links}</ul>
      </>
    );
  };

  return (
    <div className={styles} id={id}>
      <div className="component-content">{renderContent()}</div>
    </div>
  );
};
