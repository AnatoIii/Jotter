import React from 'react'
import {Button, Col, DatePicker, Input, Modal, Row} from "antd";

export default class LidCard extends React.Component {
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
    const { title } = this.props;

    return (
      <div style={{display:'inline'}}>
        <Button
          type="primary"
          ghost
          onClick={() => {this.openToggle()}}
        >
          { title }
        </Button>

        <Modal
          key="lid-card"
          title="Карточка лида"
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
              Фамилия
            </Col>
            <Col span="12">
              <Input />
            </Col>
          </Row>
          <Row gutter={[5,5]}>
            <Col span="12">
              Имя
            </Col>
            <Col span="12">
              <Input defaultValue={"Иван"}/>
            </Col>
          </Row>
          <Row gutter={[5,5]}>
            <Col span="12">
              Отчество
            </Col>
            <Col span="12">
              <Input />
            </Col>
          </Row>
          <Row gutter={[5,5]}>
            <Col span="12">
              Дата рождения
            </Col>
            <Col span="12">
              <DatePicker />
            </Col>
          </Row>
          <Row gutter={[5,5]}>
            <Col span="12">
              Email
            </Col>
            <Col span="12">
              <Input defaultValue={"test@test.ru"}/>
            </Col>
          </Row>
          <Row gutter={[5,5]}>
            <Col span="12">
              Номер телефона
            </Col>
            <Col span="12">
              <Input defaultValue={"+7 (911) 321 45 65"}/>
            </Col>
          </Row>
        </Modal>
      </div>
    );
  }
}
