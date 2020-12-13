import React from 'react'
import {Button, Col, DatePicker, Input, InputNumber, Modal, Row, Select} from "antd";

export default class PlannedTrip extends React.Component {
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

    const clients = [
      'Иван',
      'Николай',
      'Сергей',
      'Олег'
    ]

    return (
      <div style={{display:'inline'}}>
        <Button
          type="primary"
          size="large"
          ghost
          onClick={() => {this.openToggle()}}
        >
          Запланировать визит
        </Button>

        <Modal
          key="planed-trip"
          title="Запланировать визит"
          visible={open}
          width={450}
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
              Дата и время
            </Col>
            <Col span="12">
              <DatePicker showTime />
            </Col>
          </Row>
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
              Клиент
            </Col>
            <Col span="12">
              <Select defaultValue={clients[0]} style={{width:'100%'}}>
                {clients.map(client => (
                  <Select.Option key={client}>{client}</Select.Option>
                ))}
              </Select>
            </Col>
          </Row>
          <Row gutter={[5,5]}>
            <Col span="12">
              Номер телефона клиента
            </Col>
            <Col span="12">
              <Input disabled={true} defaultValue={"+7 (911) 321 45 65"}/>
            </Col>
          </Row>
        </Modal>
      </div>
    );
  }
}
