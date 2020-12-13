import React from 'react'
import {Select, notification} from 'antd'
import {getRaw} from "services/crud";

const { Option, OptGroup  } = Select;

export default class Selector extends React.Component
{
  state = {
    dictionary: [],
    isLoaded: false,
    error: null
  };

  componentDidMount() {
    this.getDictionary();
  }

  componentWillUnmount() {
    this.setState = (state,callback) => {
      return false;
    };
  }

  getDictionary = async () => {
    this.setState({
      isLoaded: false,
    });

    const { item } = this.props;
    const { list } = item;

    if(Array.isArray(list)) {
      this.setState({
        isLoaded: true,
        dictionary: list
      });
    } else {
      const response = await getRaw(list);
      if(response.status !== false) {
        this.setState({
          isLoaded: true,
          dictionary: response.data
        });

      } else {
        notification.error({
          message: 'Панель управления',
          description: 'Не удалось загрузить внешний справочник',
        });
      }
    }
  };

  handleOnChange = (value) => {
    const { item } = this.props;
    const currentKey = item.key.edit ? item.key.edit : item.key;
    const put = {
      [currentKey]: value
    };
    this.props.action(put);
  };

  render() {
    const {item} = this.props;
    const {error, isLoaded, dictionary} = this.state;

    if (error) {
      return <div>Ошибка: {error.message}</div>;
    } else if (!isLoaded) {
      return <div>Загрузка...</div>;
    } else {

      const { value } = this.props;

      const mode = (item && item.mode) ? item.mode : '';
      const options = dictionary.length > 0 ?
        dictionary.map(d => {
          if(d.items && d.items.length > 0) {
            return (
              <OptGroup label={d.title} key={d.key}>
                {
                  d.items.map(children => (
                    <Option value={children.key} key={children.key}>{children.title}</Option>
                  ))
                }
              </OptGroup>
            )
          } else {
            return  (
              <Option value={d.key} key={d.key}>{d.title}</Option>
            )
          }
        }) : null;

      return (
        <Select
          id={item.key}
          showSearch
          mode={mode}
          style={{width: '100%'}}
          placeholder={item.title}
          value={value}
          optionFilterProp="children"
          onChange={(value) => this.handleOnChange(value)}
        >
          {options}
        </Select>
      );

    }
  }
}
