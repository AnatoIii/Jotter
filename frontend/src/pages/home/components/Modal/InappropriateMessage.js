import React from 'react'
import {Button, Modal, Row} from "antd";


export default class InappropriateMessage extends React.Component {
  state = {
    open: false
  }

  openToggle = () => {
    const { open } = this.state
    this.setState({
      open: !open
    })
  };

  render() {
    const { open } = this.state

    return (
      <div style={{display:'inline'}}>
        <Button
          type="danger"
          ghost
          size="large"
          onClick={() => this.openToggle()}
        >
          Нецелевое обращение
        </Button>

        <Modal
          key="inappropriate-message"
          title="Подтвердить действие"
          visible={open}
          width={450}
          centered={true}
          onCancel={() => this.openToggle()}
          footer={[
            <Row
              key="footer-buttons"
              justify="space-between"
            >
              <Button
                key="save"
                size="large"
                type="success"
                ghost
                onClick={() => this.openToggle()}
              >
                Подтвердить
              </Button>
              <Button
                key="close"
                size="large"
                danger
                onClick={() => this.openToggle()}
              >
                Отмена
              </Button>
            </Row>
          ]}
        >
          Текст уведомления для подтверждения действия
        </Modal>
      </div>
    );
  }
}
