import React from 'react'
import {Input , Switch, Button, Drawer, Badge, DatePicker} from 'antd';
import {FilterOutlined, CheckOutlined, CloseOutlined } from '@ant-design/icons';
import moment from "moment";
import Selector from "components/Crud/Fields/Selector";
import 'moment/locale/ru';
import locale from "antd/es/date-picker/locale/ru_RU";

const { RangePicker } = DatePicker;

export default class Filter extends React.Component
{
  state = {
    filterStore: {},
    filter: {},
    filterVisible: false,
  };

  componentDidMount() {
    const { data } = this.props;
    this.setState({
      filter: data
    })
  }

  componentDidUpdate(prevProps) {
    if (this.props.data !== prevProps.data && this.props.data !== false) {
      const { data } = this.props;
      this.setState({
        filter: data
      })
    }
  }

  filterShow = () => {
    this.setState({
      filterVisible: true,
    });
  };

  filterClose = () => {
    this.setState({
      filterVisible: false,
    });
  };

  onSubmit = () => {
    const { filter } = this.state;
    this.props.action(filter);
  };

  onReset = (key) => {
    key = key ?? false;
    const { filter } = this.state;
    if(key) {
      delete filter[key];
    }

    this.setState({
      filter: key ? filter : {},
    },()=>{
      this.props.action(key ? filter : {});
    });
  };

  // IMPORTANT
  onChange = (key, value) => {
    if(!key) {
      return false;
    }
    const { filter } = this.state;
    filter[key] = value;
    this.setState({
      filter
    });
  };

  render() {
    const { fields } = this.props;
    const { filter } = this.state;

    const FilterFields = fields.map(item =>  {
      const valueApply = filter[item.key] ?? '';
      const value = filter[item.key] ?? '';
      const valueEqual = value !== '' && value === valueApply;

      const colorStyle = valueEqual ? { color: '#52c41a'} : {};
      const borderStyle = valueEqual ? {borderColor: '#52c41a'} : {};

      let formElem;
      switch (item.type) {
        case 'bool':
          formElem = (
            <Switch />
          );
          break;
        case 'datetime':
          let dateValue = value;
          if (value) {
            const dateValues = value.split('-');
            dateValue = [moment(dateValues[0], 'DD.MM.YYYY'), moment(dateValues[1], 'DD.MM.YYYY')];
          }
          formElem = (
            <RangePicker value={dateValue} format="DD.MM.YYYY" separator="-" locale={locale} onChange={(momentObj, stringObj) => {
              const value = (stringObj[0] && stringObj[1]) ? stringObj[0] + '-' + stringObj[1] : '';
              this.onChange(item.key, value);
            }} />
          );
          break;
        case 'select':
          formElem = (
            <Selector view="editor" item={item} onChange={this.onChange} value={value} action={(obj) => {
              const objValues = Object.entries(obj)[0];
              this.onChange(objValues[0], objValues[1])
            }} />
          );
          break;
        default:
          formElem = (
            <Input style={borderStyle} value={value} onChange={(e) => this.onChange(item.key, e.target.value)} allowClear={true} />
          );
          break;
      }
      return (
        <div key={item.key} style={{marginBottom:10}}>
          <div style={{marginBottom:5}}>
            <label style={colorStyle}>{item.title}</label>
            <small className="pull-right" style={{cursor: 'pointer', borderBottom:'1px dashed'}} onClick={() => this.onReset(item.key)}>Очистить</small>
          </div>
          <div>{ formElem }</div>
        </div>
      );
    });

    let filtersCount = 0;
    if(filter) {
      filtersCount = Object.keys(filter).filter((keyName) => {
        return (filter[keyName] !== "")
      }).length;
    }

    // const buttonApplyDisabled = Object.entries(data).toString() === Object.entries(filter).toString();
    // const buttonClearDisabled = Object.entries(data).length === 0 && Object.entries(filter).length === 0;

    const buttonApplyDisabled = false;
    const buttonClearDisabled = false;

    return (
      <span>
        <Badge count={filtersCount} style={{backgroundColor:'#52c41a'}}>
          <Button type="primary" onClick={this.filterShow}>
            <FilterOutlined /> Фильтр
          </Button>
        </Badge>
        <Drawer
          title="Фильтр"
          width={window.innerWidth > 900 ? 320 : window.innerWidth - 100}
          onClose={this.filterClose}
          visible={this.state.filterVisible}
          bodyStyle={{ paddingBottom: 80 }}
        >

          { FilterFields }
          <div>
            <Button type="primary" className="mr-2" onClick={() => this.onSubmit()} disabled={buttonApplyDisabled}>
              <CheckOutlined /> Применить
            </Button>
            <Button type="danger" className="mr-2" onClick={() => this.onReset()}  disabled={buttonClearDisabled}>
              <CloseOutlined /> Сбросить
            </Button>
          </div>

        </Drawer>
      </span>
    );
  }
};
