import React from 'react';
import {withRouter} from "react-router";
import {Link} from 'react-router-dom'
import {Row, Col, notification, Button, Table, Pagination, Popconfirm, Input, Badge} from 'antd'
import {PlusOutlined, EditOutlined, EyeOutlined, DeleteOutlined, QuestionCircleOutlined, SearchOutlined } from '@ant-design/icons';
import {list as serviceList, drop as serviceDrop, getRaw} from 'services/crud'
import Filter from 'components/Crud/Filter'
import './style.css'

class CrudList extends React.Component {
  state = {
    pageCurrent: 1,
    pageTotal: 1,
    pageLimit: 10,
    pageSort: false,
    data: [],
    filter: {},
    loading: false,
    error: null,
    search: null,

    columns: [],
    dictionary:{},
  };

  componentDidMount() {
    // initialize data
    this.initialize();
    // prepare columns
    this.initializeCols();
  }

  componentDidUpdate(prevProps) {
    if (this.props.page !== prevProps.page || this.props.forceUpdate !== prevProps.forceUpdate) {
      this.initialize();
    }
  }

  componentWillUnmount() {
    this.setState = (state,callback) => {
      return false;
    };
  }

  initialize = () => {
    // storage load pagination limit
    const storageGetLimit = this.storageGetLimit();
    const pageLimit = storageGetLimit ? parseInt(storageGetLimit, 10) : 10;
    let { page } = this.props;
    page = parseInt(page);
    if(page === 0) {
      page = 1;
    }

    // storage load filters
    const storageGetFilter = this.storageGetFilter();
    const filter = storageGetFilter ?? {};

    // storage load search
    const storageGetSearch = this.storageGetSearch();
    const search = storageGetSearch ?? null;

    this.setState({
      pageLimit,
      filter,
      search,
      pageCurrent: page ?? 1,
    }, () => {
      this.getList();
    });
  };

  getDictionary = async (c) => {
    const response = await getRaw(c.list);
    if (response.status !== false) {
        c.data = response.data
    }
    return c;
  };

  initializeCols = async () => {

    const { columns, options } = this.props;
    const dictionary = {};
    await Promise.all(columns.filter(c => c.list && !Array.isArray(c.list)).map(async (c) => {
      const cc = await this.getDictionary(c);
      dictionary[cc.key] = cc.data;
    }));

    this.setState({
      dictionary
    });

    const { isEdit, isView, isDelete, module } = options;
    const primaryKey = (options.primaryKey) ||  'id';

    const columnsParsed = columns.map(item => {
      let render;
      const itemAlign = item.align ?? 'left';
      switch(item.type) {
        case 'datetime':
          render = (text) => {
            if(text.indexOf(' ') !== -1) {
              const dateSplit = text.split(' ');
              return (
                <div className="datetime-container table-display-overlay" style={{textAlign:itemAlign}}>
                  <p className="datetime-container-date">{dateSplit[0]}</p>
                  <p className="datetime-container-time">{dateSplit[1]}</p>
                </div>
              );
            } else {
              return (
                <div className="table-display-overlay" style={{textAlign:itemAlign}}>
                  { text }
                </div>
              )
            }
          };
          break;

        case 'checkbox':
        case 'switch':
          render = (text) => {
            let badge;
            let checkboxLabel;
            if(item.values && item.values.indexOf('|') !== -1) {
              const checkboxLabelExplode = item.values.split("|");
              checkboxLabel = text ? checkboxLabelExplode[1] : checkboxLabelExplode[0];
            } else {
              checkboxLabel = text ? 'Да' : 'Нет';
            }
            if(item.invert) {
              badge = (!text) ? 'success' : 'danger';
            } else {
              badge = (text) ? 'success' : 'danger';
            }

            return (
              <div className="table-display-overlay" style={{textAlign:itemAlign}}>
                <span className={`font-size-12 badge badge-${badge}`}>{checkboxLabel}</span>
              </div>
            )
          };
          break;

        case 'select':

          if(!Array.isArray(item.list)) {
            const { dictionary } = this.state;
            item.list = (dictionary[item.key]) ?? [];
          }

          render = (text) => {
            let selectValue = null;
            if(!item.list || item.list.length < 1) {
              return null;
            }

            item.list.forEach(listItem => {
              const currentKey = listItem.key.list ?? listItem.key;
              if(currentKey === text) {
                selectValue = listItem.title;
                if(listItem.badge) {
                  const listItemBadgeColor = listItem.color ?? 'transparent';
                  selectValue = (
                    <Badge className="site-badge-count-109" count={selectValue} style={{ backgroundColor: listItemBadgeColor }} />
                  )
                }
              }
            });

            return (
              <div className="table-display-overlay" style={{textAlign:itemAlign}}>
                { selectValue }
              </div>
            );
          };
          break;
        default:
          render = (text) => (
            <div className="table-display-overlay" style={{textAlign:itemAlign}}>
              { text }
            </div>
          );
          break;
      }

      if(item.overlay) {
        render = (text) => {
          return (
            <div className="table-display-overlay" style={{textAlign:itemAlign}}>
              { item.overlay(text) }
            </div>
          )
        }
      }

      const currentKey = item.key.list ? item.key.list : item.key;
      return {...item, ...{
        dataIndex: currentKey,
        key: currentKey,
        render,
        sorter: item.sorting ?? false,
        // align: item.align ? item.align : 'left'
        align: 'center', // cds rules: header align - center only
      }};
    });

    // columns edit
    let actionColumn = [];
    const getActionButtons = (row) => {
      let actionButtons = [];

      if(isEdit && isEdit !== false) {
        const editButton = (row) => {
          const isEditButtonTitle = (options.isEdit.title) ? options.isEdit.title : 'Редактировать';
          const isEditButtonIcon = (options.isEdit.icon) ? options.isEdit.icon : <EditOutlined/>;
          const editCustomUrl = options.isEdit.url;
          const isEditButtonUrl = (options.isEdit.url) ? editCustomUrl.replace("{id}", row[primaryKey]) : '/' + module + '/edit/' + row[primaryKey];
          return <Link key={`action-button-edit-${row[primaryKey]}`} to={isEditButtonUrl}><Button icon={isEditButtonIcon} type="primary" className="mr-1">{isEditButtonTitle}</Button></Link>
        };
        actionButtons.push(editButton(row));
      }

      if(isView && (!isEdit || isEdit === false)) {
        const viewButton = (row) => {
          const isViewButtonTitle = (options.isView.title) ? options.isView.title : 'Просмотр';
          const isViewButtonIcon = (options.isView.icon) ? options.isView.icon : <EyeOutlined/>;
          const viewCustomUrl = options.isView.url;
          const isViewButtonUrl = (options.isView.url) ? viewCustomUrl.replace("{id}", row[primaryKey]) : '/' + module + '/view/' + row[primaryKey];
          return <Link key={`action-button-view-${row[primaryKey]}`} to={isViewButtonUrl}><Button icon={isViewButtonIcon} type="primary" className="mr-1">{isViewButtonTitle}</Button></Link>
        };
        actionButtons.push(viewButton(row));
      }

      if(isDelete && isDelete !== false) {
        const deleteButton = (row) => {
          const isDeleteButtonTitle = (options.isDelete.title) ? options.isDelete.title : 'Удалить';
          const isDeleteButtonIcon = (options.isDelete.icon) ? options.isDelete.icon : <DeleteOutlined />;

          return (
            <Popconfirm
              key={`action-button-delete-${row[primaryKey]}`}
              title="Вы действительно хотите удалить элемент?"
              onConfirm={e => this.onDelete(e, row)}
              icon={<QuestionCircleOutlined style={{ color: 'red' }} />}
              okText="Да"
              cancelText="Нет"
            >
              <Button icon={isDeleteButtonIcon} type="danger">{isDeleteButtonTitle}</Button>
            </Popconfirm>
          );
        };
        actionButtons.push(deleteButton(row));
      }

      if(options.actionButtons) {
        const xtraActions = options.actionButtons(row);
        actionButtons.push(xtraActions);
      }
      return actionButtons;
    };

    if((isEdit && isEdit !== false) || (isView && isView !== false) || (isDelete && isDelete !== false) || options.actionButtons) {
      actionColumn = [{
        title: 'Действия',
        key: 'listAction',
        align: 'center',
        width:200,
        render: row => {
          return <div>{getActionButtons(row)}</div>
        },
      }];
    }

    this.setState({
      columns: [...columnsParsed, ...actionColumn]
    });
  };

  getList = async (body) => {
    this.setState({loading: true});

    const { pageCurrent, filter, pageLimit, pageSort, search } = this.state;

    body = body || {};
    body.page = pageCurrent;
    body.perPage = pageLimit;
    if(pageSort) {
      body = {...body, ...pageSort};
    }

    if(filter && Object.keys(filter).length > 0) {
      const bodyFilter = {};
      Object.keys(filter).filter((keyName) => {
        return (filter[keyName] !== "")
      }).forEach((key) => {
        bodyFilter['filter[' + key + ']'] = filter[key];
      });
      body = {...body, ...bodyFilter};
    }

    if(search) {
      body.search = search;
    }

    const {connector} = this.props.options;

    const crudList = await serviceList(connector.list, body);
    const {status, message} = crudList;
    if(status !== false) {
      const {list, paginator} = crudList;
      this.setState({
        data: (status) ? list : [],
        pageTotal: (status) ? paginator.totalItems : 1,
      })
    } else {
      notification.error({
        message: 'Панель управления',
        description: message,
      });
    }
    this.setState({loading: false});
    return false;
  };

  onDelete = async (e, row) => {
    const {connector, isDelete} = this.props.options;
    if(isDelete &&  isDelete === true) {
      const primaryKey = this.props.options.primaryKey ||  'id';
      const url = connector.delete.replace("{id}", row[primaryKey]);
      const crudDrop = serviceDrop(url);
      if(crudDrop.status !== false) {
        notification.success({
          message: 'Панель управления',
          description: crudDrop.message,
        });
        this.getList(); // update crud list
      } else {
        notification.error({
          message: 'Панель управления',
          description: crudDrop.message,
        });
      }
    }
  };

  onPaginate = page => {
    const { options } = this.props;
    const to = '/' + options.module + '/list/' + page;
    this.props.history.push(to);
  };

  onPaginateLimit = (current, size) => {
    this.setState({
      pageCurrent: current,
      pageLimit: size
    }, ()=> {
      this.storageSetLimit(size);
      this.initialize();
    });
  };

  storageKey = (subkey) => {
    const {options} = this.props;
    const {module} = options;
    return 'CRUD-' + subkey + '-' + module;
  };

  storageSetLimit = (size) => {
    const key = this.storageKey('page-limit');
    localStorage.setItem(key, size);
    return true;
  };

  storageGetLimit = () => {
    const key = this.storageKey('page-limit');
    return localStorage.getItem(key);
  };

  storageSetFilter = (filter) => {
    const key = this.storageKey('filter');
    try {
      const data = JSON.stringify(filter);
      localStorage.setItem(key, data);
    } catch (e) {
      return false;
    }
    return true;
  };

  storageGetFilter = () => {
    const key = this.storageKey('filter');
    let data =  localStorage.getItem(key);
    if(data === false) {
      return false
    }
    try {
      return JSON.parse(data);
    } catch (e) {
      return false;
    }
  };

  storageSetSearch = (size) => {
    const key = this.storageKey('search');
    localStorage.setItem(key, size);
    return true;
  };

  storageGetSearch = () => {
    const key = this.storageKey('search');
    return localStorage.getItem(key);
  };

  onTableChange = (pagination, filters, sorter) => {
    let pageSort = false;
    if(sorter.order) {
      const a = 'order[' + sorter.field + ']';
      const b = (sorter.order === 'ascend') ? 'asc' : 'desc';
      pageSort = {
        [a]: b
      };
    }
    this.setState({
      pageSort,
    },()=>{
      this.getList();
    });
  };

  // IMPORTANT
  filterHandler = (value) => {
    this.setState({
      filter: value,
    },() => {
      this.storageSetFilter(value);

      // RESET PAGER IF FILTER ASSIGN
      const { pageCurrent } = this.state;
      if(pageCurrent === 1) {
        this.getList();
      } else {
        const { options } = this.props;
        const to = '/' + options.module;
        this.props.history.push(to);
      }

    });
  };

  onSearch = () => {
    // SET SEARCH
    const { search } = this.state;
    this.storageSetSearch(search);

    // RESET PAGER IF SEARCH ASSIGN
    const { pageCurrent } = this.state;
    if(pageCurrent === 1) {
      this.getList();
    } else {
      const { options } = this.props;
      const to = '/' + options.module;
      this.props.history.push(to);
    }
  };

  onSearchSet = (search) => {
    this.setState({
      search
    })
  };

  render() {

    const { data, loading, filter, search, columns } = this.state;

    const { options } = this.props;
    const { module } = options; // {title} commented

    let rowlightier = '';
    if(options.rowlightier && options.rowlightier !== false) {
      rowlightier = (record) => {
        const inValue = record[options.rowlightier.field];
        let colorSet;
        if(options.rowlightier.colors[inValue]) {
          colorSet = options.rowlightier.colors[inValue];
        } else {
          colorSet = options.rowlightier.colors.default;
        }
        return colorSet;
      }
    }

    let isAddButton = null;
    if(options.isAdd && options.isAdd !== false) {
      const isAddButtonTitle = (options.isAdd.title) ? options.isAdd.title : 'Добавить';
      const isAddButtonIcon = (options.isAdd.icon) ? options.isAdd.icon : <PlusOutlined />;
      const isAddButtonUrl = (options.isAdd.url) ? options.isAdd.url : '/' + module + '/add';
      isAddButton = <Link to={isAddButtonUrl}><Button icon={isAddButtonIcon} className="mr-1" type="success">{isAddButtonTitle}</Button></Link>;
    }

    const filterHandler = this.filterHandler;
    const FilterColumns = (columns.length > 0) ? columns.filter(item => item.filter) : [];
    const FilterContainer = (FilterColumns && FilterColumns.length > 0) ? (
      <Filter data={filter} fields={FilterColumns} action={filterHandler} />
    ): null;

    const topButton = options.topButton ? options.topButton : null;
    const topButtonContainer = (topButton || isAddButton || FilterContainer) ? (
      <div style={{marginBottom:10}}>
        {topButton}{isAddButton}{' '}{FilterContainer}
      </div>
    ) : null;

    const searchContainer = (options.search) ? (
      <Row gutter={[10,10]}>
        <Col span={21}>
          <Input style={{width:'100%'}} value={search} onChange={(e) => this.onSearchSet(e.target.value)} onKeyDown={(e) => {
            if (e.key === 'Enter') {
              this.onSearch();
            }
          }} allowClear={true} />
        </Col>
        <Col span={3}>
          <Button style={{width:'100%'}} icon={<SearchOutlined />} type="primary" onClick={this.onSearch} className="mr-1" />
        </Col>
      </Row>
    ) : null;

    const pageSizeOptions = ['10', '20', '50', '100'];

    return (
      <div>

        { topButtonContainer }
        { searchContainer }

        <div className="tableCard">
          <Table
            bordered
            size="small"
            loading={loading}
            className="utils__scrollTable"
            scroll={{ x: '100%' }}
            columns={columns}
            dataSource={data}
            pagination={false}
            onChange={this.onTableChange}
            rowClassName={rowlightier}
          />
          <div className="text-right" style={{marginTop:10, marginBottom:10}}>
            <Pagination current={this.state.pageCurrent} onChange={this.onPaginate} onShowSizeChange={this.onPaginateLimit} total={this.state.pageTotal} pageSize={this.state.pageLimit} showSizeChanger pageSizeOptions={pageSizeOptions}	 />
          </div>
        </div>
      </div>
    )

  }
}
export default withRouter(CrudList)
