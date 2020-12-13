import React from 'react'
import { Helmet } from 'react-helmet'
import {BreadcrumbsItem} from 'react-breadcrumbs-dynamic'
import CrudList from './List';
import CrudEdit from './Edit';

export default class Crud extends React.Component {

  render() {
    const { options, columns, modprops } = this.props;
    const { action } =  modprops.match.params;
    const id = (modprops.match.params.id) ? modprops.match.params.id : 0;

    let CrudLoader;
    let ColumnsFilter;
    switch(action) {
      case 'add':
      case 'edit':
      case 'view':
        ColumnsFilter = columns.filter(item => item.editor);
        CrudLoader = <CrudEdit id={id} options={options} mode={action} columns={ColumnsFilter} />
        break;
      default:
        ColumnsFilter = columns.filter(item => item.listing);
        CrudLoader = <CrudList options={options} columns={ColumnsFilter} page={id} />
        break;
    }
    return (
      <div>
        <Helmet title={options.title} />
        <BreadcrumbsItem to={`/${options.module}`}>{options.title}</BreadcrumbsItem>
        {CrudLoader}
      </div>
    );
  }

}
