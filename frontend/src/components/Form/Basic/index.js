import React from 'react'
import {Input, Button, Form, Checkbox, Switch, List} from 'antd'

import Selector from "components/Crud/Fields/Selector";
import CkEditor4 from "components/Crud/Fields/CkEditor4";

const {TextArea} = Input;

class FormBasic extends React.Component
{
  formRef = React.createRef();

  constructor(props) {
    super(props);
    this.formHandler = this.formHandler.bind(this);
  }

  onSubmit = (e) => {
    e.preventDefault();
    const values = this.formRef.current.getFieldsValue();
    this.props.handlerSubmit(values);
  };

  onApply = (e) => {
    e.preventDefault();
    const values = this.formRef.current.getFieldsValue();
    this.props.handlerApply(values);
  };

  onCancel = () => {
    this.props.handlerCancel();
  };

  // IMPORTANT
  formHandler = (value) => {
    const form = this.formRef.current;
    form.setFieldsValue(value);
  };

  render() {
    const formHandler = this.formHandler;
    const { columns, data } = this.props;

    const fields = columns.map(function(item) {
      let formElem;
      const currentKey = item.key.edit ? item.key.edit : item.key;
      let dataValue = (data && data[currentKey]) ? data[currentKey] : '';
      const elemDisabled = (item.readonly && item.readonly !== false);
      switch(item.type) {
        case 'select':
          if(item.mode && item.mode === 'multiple') {
            if(dataValue === '') {
              dataValue = [];
            }
          }
          formElem = <Selector view="editor" item={item} onChange={formHandler} value={dataValue} action={formHandler} disabled={elemDisabled} />
          break;
        case 'textarea':
          formElem = <TextArea rows={3} disabled={elemDisabled} />
          break;
        case 'switch':
          if(item.values && item.values.indexOf('|') !== -1) {
            const checkboxLabelExplode = item.values.split("|");
            dataValue = !!(dataValue);
            formElem = <Switch checkedChildren={checkboxLabelExplode[1]} unCheckedChildren={checkboxLabelExplode[0]} defaultChecked={dataValue} disabled={elemDisabled} />
          } else {
            formElem = <Switch defaultChecked={dataValue} disabled={elemDisabled} />
          }
          break;
        case 'checkbox':
          formElem = <Checkbox checked={dataValue} disabled={elemDisabled} />
          break;
        case 'list':
          formElem = <List size="small" bordered dataSource={dataValue} renderItem={item => (<List.Item>{item}</List.Item>)} />
          break;
        case 'wysiwyg':
          const itemOptions = item.options ?? {};
          formElem = <CkEditor4 item={item} options={itemOptions} value={dataValue} action={formHandler} />
          break;
        default:
          formElem = <Input size="default" disabled={elemDisabled} />
          break;
      }

      item.gridsize = item.gridsize || 12;
      item.required = item.required || false;
      item.requiredText = item.requiredText || 'Поле обязательное для заполнения';

      return (
        <div className={`col-lg-${item.gridsize}`} key={`formElemBlock-${currentKey}`}>
          <div className="form-group">
            <Form.Item label={item.title} name={currentKey} initialValue={dataValue} rules={[{required: item.required, message: item.requiredText}]}>
              {formElem}
            </Form.Item>
          </div>
        </div>
      );
    });

    const { handlerSubmit, handlerApply, handlerCancel } = this.props;

    return (
      <Form layout="vertical" ref={this.formRef}>
        <div className="row">
          {fields}
        </div>

        { (handlerSubmit || handlerApply || handlerCancel) && (
        <div className="row">
          <div className="col-lg-12">
            <div className="form-actions">
              { handlerSubmit && (
                <Button key="buttonSubmit" type="success" className="mr-2" htmlType="submit" onClick={e => this.onSubmit(e)}>
                  Сохранить
                </Button>
              )}

              { handlerApply && (
                <Button key="buttonApply" type="primary" className="mr-2" onClick={e => this.onApply(e)}>
                  Применить
                </Button>
              )}

              { handlerCancel && (
                <Button key="buttonCancel" className="mr-2" onClick={() => this.onCancel()}>
                  Отмена
                </Button>
              )}
            </div>
          </div>
        </div>
        )}
      </Form>
    )
  }
}

export default FormBasic
