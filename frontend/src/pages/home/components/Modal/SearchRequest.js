import React from 'react'
import {Table, Button, Col, Modal, Row } from "antd";
import Request from "./Request";

export default class SearchRequest extends React.Component {
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
        title: '',
        dataIndex: 'button',
        align: 'center',
        render: (text) => {
          return (text) ? <Request size="small" title="Редактировать" /> : null
        }
      },
      {
        title: 'Название заявки',
        dataIndex: 'title',
        align: 'center'
      },
      {
        title: 'Источник обращения',
        dataIndex: 'source',
        align: 'center'
      },
      {
        title: 'Менеджер заявки',
        dataIndex: 'manager',
        align: 'center'
      },
      {
        title: 'Статус заявки',
        dataIndex: 'status',
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
        title: 'Состояние заявки',
        dataIndex: 'condition',
        align: 'center'
      }
    ];

    const dataX = [
      {
        key: 1,
        button: true,
        title: 'Название 1',
        source: 'Call-центр',
        manager: 'Петров В.В.',
        status: 'Первичный звонок',
        clients: {
          name: 'Иванов И.И.',
          phone: '+79045514478'
        },
        condition: 'Открыто'
      },
      {
        key: 1,
        button: false,
        title: 'Название 2',
        source: 'Call-центр',
        manager: 'Петров В.В.',
        status: 'Повторный звонок',
        clients: {
          name: 'Котов Э.В.',
          phone: '+79045514478'
        },
        condition: 'Открыто'
      },
    ]

    return (
      <div style={{display:'inline'}}>
        <Button
          type="primary"
          ghost
          size="large"
          onClick={() => {this.openToggle()}}
        >
          Поиск заявки
        </Button>

        <Modal
          key="search-request"
          title="Поиск заявки"
          visible={open}
          width={970}
          heigh={500}
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
              <Button type="primary" ghost size="small">Найти</Button>{' '}
              <Request size="small" title="Добавить заявку" />
            </Col>
          </Row>
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
