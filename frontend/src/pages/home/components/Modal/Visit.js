import React from 'react'
import {Button, Col, DatePicker, Input, InputNumber, Modal, Row, Select} from "antd";

export default class Visit extends React.Component {
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
    const { type, title } = this.props

    const managers = [
      'Менеджер 1',
      'Менеджер 2',
      'Менеджер 3'
    ]

    const clients = [
      'Иван',
      'Николай',
      'Сергей',
      'Олег'
    ]

    return (
      <div style={{display:'inline'}}>
        <Button
          type={type ?? "primary"}
          onClick={() => {this.openToggle()}}
          size="large"
        >
          { title }
        </Button>

        <Modal
          key="visit"
          title="Визит №12345"
          visible={open}
          width={600}
          heigh={363}
          centered={true}
          onCancel={() => this.openToggle()}
          footer={[
            <Row key="footer-buttons" justify="space-between">
              <Button key="save" type="primary" ghost size="large" onClick={() => this.openToggle()}>
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
          <Row gutter={[5,5]}>
            <Col span="12">
              Менеджер визита
            </Col>
            <Col span="12">
              <Select placeholder={'Выберите менеджера'} style={{width:'100%'}}>
                {managers.map(manager => (
                  <Select.Option key={manager}>{manager}</Select.Option>
                ))}
              </Select>
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
