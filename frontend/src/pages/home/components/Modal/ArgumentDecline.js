import React from 'react'
import {Button, Col, Input, Modal, Row, Select} from "antd";

export default class ArgumentDecline extends React.Component {
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

    return (
      <div style={{display:'inline'}}>
        <Button
          type="danger"
          ghost
          size="large"
          onClick={() => this.openToggle()}
          style={{padding: '5px'}}
        >
          Аргументированный отказ
        </Button>

        <Modal
          key="argument-decline"
          title="Аргументированный отказ"
          visible={open}
          width={450}
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
            <Col span="9">
              Причина отказа
            </Col>
            <Col span="15">
              <Select placeholder="Выберите из списка" style={{width:'100%'}}>
                <Select.Option>Reason 1</Select.Option>
                <Select.Option>Reason 2</Select.Option>
              </Select>
            </Col>
          </Row>
          <Row gutter={[5,5]}>
            <Col span="9">
              Комментарий
            </Col>
            <Col span="15">
              <Input.TextArea placeholder="Введите комментарий" style={{width:'100%'}} rows={4} />
            </Col>
          </Row>
        </Modal>
      </div>
    );
  }
}
