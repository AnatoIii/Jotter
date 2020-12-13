import React from 'react'
import {Table, Button, Col, Input, Modal, Row, Select} from "antd";
import SendToAllocation from "./SendToAllocation";
import PlannedTrip from "./PlannedTrip";
import SendToManager from "./SendToManager";
import PlannedTripAndSendToManager from "./PlannedTripAndSendToManager";
import ArgumentDecline from "./ArgumentDecline";
import InappropriateMessage from "./InappropriateMessage";
import Visit from "./Visit";
import styles from "./style.module.scss"
import LidCard from "./LidCard";

export default class Request extends React.Component {
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
    const { open } = this.state
    const { title, size } = this.props

    const sources = [
      'Источник №1',
      'Источник №2',
      'Источник №3'
    ]

    const agencies = [
      'Агентство №1',
      'Агентство №2',
      'Агентство №3'
    ]

    const columns = [
      {
        title: '',
        dataIndex: 'button',
        align: 'center',
        render: (text) => {
          let result;

          switch (text) {
            case 'edit':
              result = <LidCard title="Редактировать" />
              break
            case 'add':
              result = <LidCard title="Добавить" />
              break
            default:
              result = null
              break
          }

          return result
        }
      },
      {
        title: 'Тип',
        dataIndex: 'type',
        align: 'left'
      },
      {
        title: 'Ф И О',
        dataIndex: 'fio',
        align: 'center'
      },
      {
        title: 'Телефон',
        dataIndex: 'phone',
        align: 'center'
      }
    ];

    const dataX = [
      {
        button: 'edit',
        type: 'Лид',
        fio: 'Иван',
        phone: '+7 (904) 123 45 67'
      },
      {
        button: false,
        type: 'Клиент',
        fio: 'Котов Эдуард Валентинович',
        phone: '+7 (904) 987 65 43'
      },
      {
        button: 'add',
        type: '',
        fio: '',
        phone: ''
      }
    ]

    return (
      <div style={{display:'inline'}}>
        <Button onClick={() => this.openToggle()} ghost size={size ??"large"} type="primary">{title}</Button>

        <Modal
          key="request"
          title="Карточка заявки"
          visible={ open }
          width={850}
          centered={true}
          onCancel={() => this.openToggle()}
          className={styles.modal}
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
          bodyStyle={{display: 'flex', 'flex-wrap': 'wrap'}}
        >
          <div style={{width: 600}}>
            <Row gutter={[5,5]}>
              <Col span="8">
                Название
              </Col>
              <Col span="14">
                <Input defaultValue={"Иван"}/>
              </Col>
            </Row>
            <Row gutter={[5,5]}>
              <Col span="8">
                Номер заявки
              </Col>
              <Col span="14">
                <Input
                  defaultValue={"100500874"}
                  disabled={true}
                />
              </Col>
            </Row>
            <Row gutter={[5,5]}>
              <Col span="8">
                Статус
              </Col>
              <Col span="14">
                <Input defaultValue={"Статус"} disabled={true} />
              </Col>
            </Row>
            <Row gutter={[5,5]}>
              <Col span="8">
                Источник обращения
              </Col>
              <Col span="14">
                <Select defaultValue={sources[0]} style={{width:'100%'}}>
                  {sources.map(source => (
                    <Select.Option key={source}>{source}</Select.Option>
                  ))}
                </Select>
              </Col>
            </Row>
            <Row gutter={[5,5]}>
              <Col span="8">
                Менеджер
              </Col>
              <Col span="14">
                <Input defaultValue={"Иван"} disabled={true}/>
              </Col>
            </Row>
            <Row gutter={[5,5]}>
              <Col span="8">
                Агентство
              </Col>
              <Col span="14">
                <Select defaultValue={agencies[0]} style={{width:'100%'}}>
                  {agencies.map(agency => (
                    <Select.Option key={agency}>{agency}</Select.Option>
                  ))}
                </Select>
              </Col>
            </Row>
            <Row gutter={[5,5]}>
              <Col span="8">
                Запланированный контакт
              </Col>
              <Col span="14">
                <Input defaultValue={"Встреча"} disabled={true}/>
              </Col>
            </Row>
            <Row gutter={[5,5]}>
              <Col span="8">
                Комментарий
              </Col>
              <Col span="14">
                <Input.TextArea placeholder="Введите комментарий" style={{width:'100%'}} rows={4} />
              </Col>
            </Row>
            <Row gutter={[5,5]}>
              <Col span="8">
                Причина отказа
              </Col>
              <Col span="14">
                <Input defaultValue={"Причина №1"} disabled={true}/>
              </Col>
            </Row>
          </div>
          <div style={{width: 200, display: 'flex', 'flex-direction':'column'}}>
            <SendToAllocation />
            <Button
              type="primary"
              size="large"
              ghost
            >
              Позвонить менеджеру
            </Button>
            <PlannedTrip />
            <SendToManager />
            <PlannedTripAndSendToManager />
            <ArgumentDecline />
            <InappropriateMessage />
            <Visit type="success" title="Клиент пришёл" />
          </div>
          <h3>Клиенты:</h3>
          <Table data={dataX} columns={columns} />
        </Modal>
      </div>
    );
  }
}
