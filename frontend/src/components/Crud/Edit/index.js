import React from 'react'
import {withRouter} from 'react-router-dom'
import {Helmet} from "react-helmet";
import {BreadcrumbsItem} from "react-breadcrumbs-dynamic";
import {notification} from 'antd'
import {getRaw, postRaw, putRaw} from 'services/crud'

import BigLoader from 'components/Application/BigLoader'
import FormBasic from 'components/Form/Basic'

class CrudEdit extends React.Component
{

  state = {
    loading: false,
    data: [],
  };

  componentDidMount() {
    const { id } = this.props;
    if(id !== 0) {
      this.getItem();
    }
  }

  componentWillUnmount() {
    this.setState = (state,callback) => {
      return false;
    };
  }

  getItem = async () => {
    const { id, options } = this.props;
    const { connector } = options;
    if(id === 0) {
      return;
    }

    this.setState({loading: true});

    const url = connector.view.replace("{id}", id);
    const result = await getRaw(url);

    if(result.status !== false) {
      const {data} = result;
      this.setState({
        data
      });
    } else {
      notification.error({
        message: 'Панель управления',
        description: result.message,
      });
    }
    this.setState({loading: false});
  };


  onSubmit = async (values) => {
    this.sendData(values, false);
  };

  onApply = async (values) => {
    this.sendData(values, true);
  };

  sendData = async (values, apply) => {
    const { mode } = this.props;
    if(mode === 'view') {
      return false;
    }

    const {id, options} = this.props;
    const {module, connector} = options;

    let url = connector.add;
    if (id !== 0) {
      url = connector.edit.replace("{id}", id);
    }

    const result = (id !== 0) ?
      await putRaw(url, values) :
      await postRaw(url, values);

    const {status, message} = result;
    if (status !== false) {

      notification.success({
        message: 'Панель управления',
        description: message,
      });

      // save button pressed -> redirect to list
      if(apply === false) {
        const to = '/' + module + '/list';
        this.props.history.push(to);
      }
      // apply button pressed from edit window -> update data
      else {
        const primaryKey = (options.primaryKey) ||  'id';
        const to = '/' + module + '/edit/' + result.data[primaryKey];
        this.props.history.push(to);
      }
    } else {
      notification.error({
        message: 'Панель управления',
        description: message,
      });
    }
  };

  onCancel = () => {
    this.props.history.push('/' + this.props.options.module)
  };

  render() {
    const { loading, data } = this.state;
    if (loading) {
      return (
        <BigLoader />
      );
    }

    const { id, columns, mode } = this.props;

    const pageTitle = (mode === 'view') ? 'Просмотр' : ((id === 0) ? 'Добавление' : 'Редактирование');
    const title = data.name ?? pageTitle;

    if(mode === 'view') {
      columns.map(c => {
        c.readonly = true;
        return c;
      })
    }

    return (
      <div>
        <Helmet title={title} />
        <BreadcrumbsItem to={`/contactPerson/${id}`}>{title}</BreadcrumbsItem>

        <div className="card">
          <div className="card-header">
            <div className="utils__title">
              <strong>{pageTitle}</strong>
            </div>
          </div>
          <div className="card-body">
            <FormBasic
              data={data}
              columns={columns}
              handlerCancel={this.onCancel}
              handlerSubmit={ (mode === 'edit' || mode === 'add') ? this.onSubmit : false}
              handlerApply={ (mode === 'edit' || mode === 'add') ? this.onApply : false}
            />
          </div>
        </div>
      </div>
    )

  }
}

export default withRouter(CrudEdit)
