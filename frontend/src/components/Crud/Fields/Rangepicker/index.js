import React from 'react'
import { DatePicker } from 'antd';
import locale from "antd/es/date-picker/locale/ru_RU";

const { RangePicker } = DatePicker;

export default class Rangepicker extends React.Component
{
  render() {
    const { item } = this.props;
    const separator = this.props.separator || '+';
    const dateFormat = this.props.format || 'Y-m-d';
    // const value = this.props.value || ""; -> TODO: REPLACE TO SELECTED VALUE

    const handleOnChange = (momentObj, stringObj) => {
      const value = stringObj[0] + separator + stringObj[1];
      const put = {
        [item.key]: value
      };
      this.props.action(put);
    };

    return (
      <RangePicker format={dateFormat} separator={separator} locale={locale} onChange={(value, event) => handleOnChange(value, event)} />
    );

  }
}
