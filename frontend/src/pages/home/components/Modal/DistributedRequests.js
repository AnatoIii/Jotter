import React from 'react'
import {Table, Button, Col, Modal, Row } from "antd";

export default class DistributedRequests extends React.Component {
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

    const columns = [
      {
        title: 'Номер заявки',
        dataIndex: 'number',
        align: 'center'
      },
      {
        title: 'Клиенты',
        dataIndex: 'clients',
        align: 'center',
        render: ( info ) => {
          return info.name + ' ' + info.phone
        }
      },
      {
        title: '',
        dataIndex: 'button',
        align: 'center',
        render: (text) => {
          return (text) ?
            <Button
              size="small"
              ghost
              type="primary"
              title="Забрать себе"
            >Забрать себе</Button> : null
        }
      }
    ];

    const dataX = [
      {
        number: '10050064',
        clients: {
          name: 'Иванов И.И.',
          phone: '+79045514478'
        },
        button: true,
      },
      {
        number: '10050064',
        clients: {
          name: 'Котов Э.В.',
          phone: '+79045514478'
        },
        button: false,
      }
    ]

    return (
      <div style={{display:'inline'}}>
        <Button
          type="primary"
          ghost
          size="large"
          onClick={() => {this.openToggle()}}
        >
          Распределяющиеся заявки
        </Button>

        <Modal
          key="distributed-request"
          title="Поиск заявки"
          visible={open}
          width={450}
          heigh={352}
          centered={true}
          onCancel={() => this.openToggle()}
          footer={[
            <Row key="footer-buttons" justify="end">
              <Button key="close" danger size="large" onClick={() => this.openToggle()}>
                Закрыть
              </Button>
            </Row>
          ]}
        >
          <Row gutter={[5,5]}>
            <Col span="12">
              <Table bordered dataSource={dataX} columns={columns}/>
            </Col>
          </Row>
        </Modal>
      </div>
    );
  }
}
