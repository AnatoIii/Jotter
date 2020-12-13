import React from 'react'
import {Form, Input, Button, notification} from 'antd'
import {Helmet} from 'react-helmet'
import {login} from 'services/user';
import styles from './style.module.scss'
import {connect} from "react-redux";
import {LockOutlined, UserOutlined} from "@ant-design/icons";
import background from '../../../assets/images/login.jpg';

class Login extends React.Component {
  formRef = React.createRef();

  state = {
    loading: false,
  };

  onFinish = async (data) => {
    this.setState({ loading: true });
    const result = await login(data);
    this.setState({ loading: false });

    if (result.successful) {
      const {dispatch} = this.props;
      dispatch({
        type: 'user/LOAD_CURRENT_ACCOUNT',
      })
    } else {
      notification.warning({
        message: 'Log in',
        description: result.message ?? 'Cannot log in',
      })
    }
  };

  registerLink = () => {
    window.location.replace("/register");
  }

  render() {
    const {loading} = this.state;
    return (
      <div className={styles.loginMain}>
        <div className={styles.loginImage}>
          <img src={background} alt="background" />
        </div>
        <Helmet title="Log in"/>
        <div className={styles.block}>
          <h3>Login</h3>
          <Form layout="vertical" ref={this.formRef} onFinish={this.onFinish}>
            <Form.Item name="email" rules={[{required: true, message: 'Please enter email'}]}>
              <Input prefix={<UserOutlined className="site-form-item-icon" />} placeholder="Username" size="default"/>
            </Form.Item>
            <Form.Item name="password" rules={[{required: true, message: 'Please enter password'}]}>
              <Input prefix={<LockOutlined className="site-form-item-icon" />} type="password" placeholder="Password" size="default" type="password"/>
            </Form.Item>
            <div className="form-actions">
              <Button
                loading={loading}
                type="primary"
                className="width-150 mr-4"
                htmlType="submit"
                id="login_button"
              >
                Log in
              </Button>
              <Button
                type="primary"
                className="width-150 mr-4"
                htmlType="button"
                onClick={() => this.registerLink()}
              >
                Register
              </Button>
            </div>
          </Form>
        </div>
      </div>
    )
  }
}
export default connect(
  ({user}) => ({user})
)(Login)
