import React from 'react'
import {connect} from 'react-redux'
// import {Link} from 'react-router-dom'
import {Menu, Dropdown, Avatar} from 'antd'
import {UserOutlined} from '@ant-design/icons';
import styles from './style.module.scss'
class ProfileMenu extends React.Component {

  logout = () => {
    const {dispatch} = this.props
    dispatch({
      type: 'user/LOGOUT',
    })
  }

  render() {
    const {user} = this.props;

    const menu = (
      <Menu selectable={false}>
        <Menu.Item>
          Your username: <strong>{user.username}</strong>
        </Menu.Item>
        <Menu.Divider />
        <Menu.Item>
          <div>
            Your telegram chat id: <strong>{user.chatId || '-'}</strong>
          </div>
        </Menu.Item>
        <Menu.Divider />
        <Menu.Item>
          <a href="!#" onClick={this.logout}>
            <i className={`${styles.menuIcon} icmn-exit`} />
            Log out
          </a>
        </Menu.Item>
      </Menu>
    )

    const A = <Avatar className={styles.avatar} shape="square" size="large" icon={<UserOutlined />} />

    return (
      <Dropdown overlay={menu} trigger={['click']}>
        <div className={styles.dropdown}>
          { A }
        </div>
      </Dropdown>
    )
  }
}

export default connect(
  ({user}) => ({user})
)(ProfileMenu)
