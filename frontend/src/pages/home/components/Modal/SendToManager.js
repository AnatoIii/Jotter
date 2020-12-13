import React from 'react'
import {Button, Col, InputNumber, Modal, Row, Select} from "antd";

export default class SendToManager extends React.Component {
  state = {
    open: false
  }

  openToggle = () => {
    const { open } = this.state;
    this.setState({
      open: !open
    })
  };

  render() {
    const { open } = this.state;

    const managers = [
      'Менеджер 1',
      'Менеджер 2',
      'Менеджер 3'
    ]

    return (
      <div style={{display:'inline'}}>
        <Button
          type="primary"
          size="large"
          ghost
          onClick={() => {this.openToggle()}}
        >
          Отправить менеджеру
        </Button>

        <Modal
          key="send-to-manager"
          title="Отдать менеджеру"
          visible={open}
          width={400}
          heigh={218}
          centered={true}
          onCancel={() => {this.openToggle()}}
          footer={[
            <Row key="footer-buttons" justify="space-between">
              <Button key="save" type="primary" size="large" ghost onClick={() => this.openToggle()}>
                Сохранить
              </Button>
              <Button key="close" danger size="large" onClick={() => this.openToggle()}>
                Закрыть
              </Button>
            </Row>
          ]}
        >
          <Row gutter={[5,5]}>
            <Col span="12">
              Номер заявки
            </Col>
            <Col span="12">
              <InputNumber disabled={true} defaultValue={213456987} style={{width:'100%'}}/>
            </Col>
          </Row>
          <Row gutter={[5,5]}>
            <Col span="12">
              Менеджер заявки
            </Col>
            <Col span="12">
              <Select placeholder={'Выберите менеджера'} style={{width:'100%'}}>
                {managers.map(manager => (
                  <Select.Option key={manager}>{manager}</Select.Option>
                ))}
              </Select>
            </Col>
          </Row>
        </Modal>
      </div>
    );
  }
}
