import React from 'react'
import {DownCircleOutlined, UpCircleOutlined} from "@ant-design/icons";

export default class ContainerBlock extends React.Component {
  state = {
    open: true,
  };

  openToggle = () => {
    const { open } = this.state;
    this.setState({
      open: !open
    })
  };

  render() {
    const { title, data } = this.props;
    const { open } = this.state;

    const upDownIcon = open ? <UpCircleOutlined /> : <DownCircleOutlined />;

    return (
      <div className="containerBlock">
        <div className="containerBlock-header" onClick={() => this.openToggle()} style={{
          cursor: 'pointer'
        }}>
          <strong>{title}</strong>
          <span className="pull-right">{upDownIcon}</span>
        </div>
        { (open === true) && (
          <div className="containerBlock-body">
            { data }
          </div>
        )}
      </div>
    );
  }
}
