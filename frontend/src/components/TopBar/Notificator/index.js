import React from 'react'
import { withRouter  } from 'react-router-dom'
import { connect } from 'react-redux'
import { compose } from "redux";
import {Menu, Dropdown, Badge, notification, Button} from 'antd'
import {BellFilled, EyeOutlined} from "@ant-design/icons";

import styles from './style.module.scss'
const mapStateToProps = ({ user }) => ({
  user: user
});

class Notificator extends React.Component {

  state = {
    handling: []
  };

  componentDidMount() {
    const {user} = this.props;
    if(typeof(EventSource) !== "undefined") {
      const eventSource = new EventSource(window.location.origin+':5002/.well-known/mercure?topic=' + encodeURIComponent('/notify/'+user.username));
      eventSource.onmessage = event => {
        this.openNotification(JSON.parse(event.data));
        this.getHandling();
      };
    } else {
      notification.error({
        message: 'Уведомления',
        description: 'Браузер не поддерживает eventSource. Получение уведомлений невозможно',
      });
    }
    this.getHandling();
  }

  componentWillUnmount() {
    this.setState = (state,callback) => {
      return false;
    };
  }

  getHandling = async () => {
    const path = '/api/directory/handling-by-current-user';
    const token = localStorage.getItem('token');
    const response = await fetch(path, {
      method: 'get',
      headers: new Headers({
        'Authorization': 'Bearer ' + token,
      })
    });
    if(!response || response.status !== 200) {
      return false;
    }
    const result = await response.json();
    const { data } = result;
    if(!data || data.length < 1) {
      return false;
    }

    this.setState({
      handling: data
    });
  };

  goHandling = (code) => {
    notification.destroy();
    const to = '/handling/view/' + code;
    this.props.history.push(to);
  };

  openNotification = (event) => {
    const key = `open${Date.now()}`;
    const { code, handlingChannel } = event;
    const btn = (
      <Button type="primary" size="small" icon={<EyeOutlined />} onClick={() => this.goHandling(code)}>
        Перейти
      </Button>
    );

    let sourceName;
    switch(handlingChannel) {
      case 'phone_call':
        sourceName = 'Телефонный звонок';
        break;
      case 'email':
        sourceName = 'E-mail';
        break;
      case 'paper_letter':
        sourceName = 'Письменное обращение';
        break;
      case 'personal_visit_office':
        sourceName = 'Личное обращение';
        break;
      case 'telegram':
        sourceName = 'Телеграм';
        break;
      default:
        sourceName = 'Неизвестен';
        break;
    }

    notification.config({
      duration: 120,
    });

    notification.open({
      message: 'Новое обращение',
      description:
        'Источник: ' + sourceName,
      btn,
      key
    });
  };

  render() {
    const { handling } = this.state;
    const count = handling.filter(item => {
      return item.handlingStatus.code === 'new'
    }).length;
    if(count < 1) {
      return false;
    }

    const menuItems = handling.length > 0 ? handling.map(item => {
      const { code, handlingChannel, handlingStatus, beginAt } = item;
      let icon = 'icmn-star-full';
      switch(handlingChannel.code) {
        case 'phone_call':
          icon = 'icmn-phone';
          break;
        case 'email':
          icon = 'icmn-mail';
          break;
        case 'paper_letter':
          icon = 'icmn-envelop';
          break;
        case 'personal_visit_office':
          icon = 'icmn-office';
          break;
        case 'telegram':
          icon = 'icmn-telegram';
          break;
        default:
          icon = 'icmn-star-full';
          break;
      }

      const badgeColor = handlingStatus.color.replace('#80A6FF', '#ff291d');
      const description = 'Источник: ' + handlingChannel.name;
      const badge = <span className="badge" style={{backgroundColor:badgeColor, color:'#FFF'}}>{handlingStatus.name}</span>;

      return (
        <Menu.Item onClick={() => this.goHandling(code)} className={styles.item} key={`notification-menu-item-${code}`}>
          <i className={`${styles.icon} ${icon}`} />
          <div className={styles.inner}>
            <div className={styles.title}>
              {beginAt} {badge}
            </div>
            <div className={styles.descr}>
              {description}
            </div>
          </div>
        </Menu.Item>
      )
    }) : [];

    const menu = <Menu selectable={false} className={styles.activity}>{menuItems}</Menu>

    return (
      <div>
        <Dropdown overlay={menu} trigger={['click']}>
          <div className={styles.dropdown}>
            <Badge count={count} offset={[-6, 1]} style={{fontSize:10, minWidth :15, height:17}}>
              <div className={styles.ringCircle}>
                <BellFilled className={styles.animateRing} style={{fontSize:20}} />
              </div>
            </Badge>
          </div>
        </Dropdown>
      </div>
    )
  }
}

export default compose(withRouter, connect(mapStateToProps))(Notificator)
