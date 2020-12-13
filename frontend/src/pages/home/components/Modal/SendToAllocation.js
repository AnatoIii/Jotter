import React from 'react'
import {Button, Col, InputNumber, Modal, Row, Select} from "antd";

export default class SendToAllocation extends React.Component {
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

    const offices = [
      'Офис №1',
      'Офис №2',
      'Офис №3',
    ];

    return (
      <div style={{display:'inline'}}>
        <Button
          type="primary"
          ghost
          size="large"
          onClick={() => {this.openToggle()}}
          style={{height:'55px', 'line-height': '1.2'}}
        >
          Отправить в<br />распределение
        </Button>

        <Modal
          key="send-to-allocation"
          title="Отправить в распределение"
          visible={open}
          width={450}
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
              Офис
            </Col>
            <Col span="12">
              <Select defaultValue={offices[0]} style={{width:'100%'}}>
                {offices.map(office => (
                  <Select.Option key={office}>{office}</Select.Option>
                ))}
              </Select>
            </Col>
          </Row>
        </Modal>
      </div>
    )
  }
}
