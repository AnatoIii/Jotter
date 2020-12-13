import React from 'react'

import {Row, Col} from "antd";

import {locale} from 'devextreme/localization';
import Scheduler, {Resource, View} from 'devextreme-react/scheduler';

import 'devextreme/dist/css/dx.common.css';
import 'devextreme/dist/css/dx.light.css';
import Notification from "../../components/Notification";


locale(navigator.language);

const groups = ['managerId'];

export default class Home extends React.Component {
  constructor(props) {
    super(props);
    this.notificationModal = React.createRef();
  }
  onAddingNewAppointment(data){
    alert('Adding new appointment');
  }
  onAppointmentFormOpening(data,ref) {
    let form = data.form;

    form.option('items', [
      {
        label: {
          text: 'Name'
        },
        isRequired: true,
        editorType: 'dxTextBox',
        dataField: 'name',
        editorOptions: {
          width: '100%',
          value: data.appointmentData.name
        }
      },
      {
        dataField: 'startDate',
        editorType: 'dxDateBox',
        editorOptions: {
          width: '100%',
          type: 'datetime'
        }
      },
      {
        name: 'endDate',
        dataField: 'endDate',
        editorType: 'dxDateBox',
        editorOptions: {
          width: '100%',
          type: 'datetime',
        }
      },
      {
        label: {
          text: 'Description'
        },
        editorType: 'dxTextArea',
        dataField: 'description',
        editorOptions: {
          width: '100%',
          value: data.appointmentData.description
        }
      },
      {
        editorType: 'dxTagBox',
        dataField: 'notifications',
        editorOptions: {
          dataSource:[12,3,45,3],
          onSelectionChanged: (e)=>{
            console.log(e.removedItems);
          }
        }
      },
      {
        name: 'button',
        itemType: 'button',
        buttonOptions:{
          text: 'ClickMe',
          onClick: ()=>{ref.current.showModal(data.appointmentData.guid)}
        },
        visible: data.appointmentData.notifications??false
      }
    ]);
  }

  render() {
    const dataEvent = [
      {
        color: '#f5222d',
        id: 1,
        text: 'Клиент не пришел',
        managerId: 1,
        startDate: new Date('2020-11-10T09:00:00'),
        endDate: new Date('2020-11-10T10:00:00'),
        notifications:[1,2,3,4]
      },
      {
        id: 2,
        color: '#52c41a',
        text: 'Встреча состоялась',
        managerId: 2,
        startDate: new Date('2020-11-10T10:00:00'),
        endDate: new Date('2020-11-10T11:00:00'),
      },
      {
        id: 3,
        color: '#faad14',
        text: 'Запланирована встреча с клиентом',
        managerId: 3,
        startDate: new Date('2020-11-10T12:00:00'),
        endDate: new Date('2020-11-10T12:30:00'),
      }
    ];

    return (
      <div>
        <Row gutter={[10, 10]}>
          <Col xl={24} xs={24}>
            <div id="scheduler" style={{marginTop: 10}}>
              <Notification ref={this.notificationModal} visible={false}/>
              <Scheduler
                dataSource={dataEvent}
                groups={groups}
                groupByDate={true}
                defaultCurrentView="week"
                height={1250}
                showCurrentTimeIndicator={true}
                locale="ru-RU"
                currentView="week"
                showAllDayPanel={true}
                onAppointmentFormOpening={(data)=>{this.onAppointmentFormOpening(data,this.notificationModal)}}
                onAppointmentAdding={this.onAddingNewAppointment}
              >
                <Resource
                  fieldExpr="id"
                  dataSource={dataEvent}
                  useColorAsDefault={true}
                />
                <View
                  type="day"
                  intervalCount={7}
                  name="Week"
                />
              </Scheduler>
            </div>
          </Col>
        </Row>
      </div>
    );
  }
}
