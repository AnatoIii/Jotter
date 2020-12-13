import React from 'react'
import {Table, Badge, Spin} from "antd";
import { getRaw } from 'services/crud'
import {DownCircleOutlined, UpCircleOutlined} from "@ant-design/icons";

export default class TableCard extends React.Component {
  state = {
    data: [],
    open: true,
    loading: true,
  };

  componentDidMount() {
    this.dataLoad();
  }

  componentWillUnmount() {
    this.setState = (state,callback) => {
      return false;
    };
  }

  dataLoad = async () => {
    this.setState({
      loading: true,
    });

    const { url } = this.props;
    if(this.props.data && !url) {
      const open = this.props.data.length < 4;
      this.setState({
        data: this.props.data,
        open,
        loading: false
      });
      return true;
    }

    const result = await getRaw(url);
    const { data, status } = result;

    if (status !== false) {
      data.map(i => {
        if(!i.key && i.code) {
          i.key = i.code;
        }
        return i;
      });

      const open = data.length < 4;
      this.setState({
        data,
        open
      })
    }
    this.setState({
      loading: false,
    })
  };

  openToggle = () => {
    const { data, open } = this.state;
    if(data.length < 1) {
      return false;
    }
    this.setState({
      open: !open
    })
  };

  render() {
    const { title, columns } = this.props;
    const { data, open, loading } = this.state;

    const columnsParsed = columns.map(col => {
      col.key = 'td-' + col.dataIndex;
      if (col.dataIndex === 'createdAt' || col.dataIndex === 'beginAt') {
        col.render = (text) => {
          if(text.indexOf(' ') !== -1) {
            const dateSplit = text.split(' ');
            return (
              <div className="datetime-container table-display-overlay" style={{textAlign:'center'}}>
                <p className="datetime-container-date">{dateSplit[0]}</p>
                <p className="datetime-container-time">{dateSplit[1]}</p>
              </div>
            );
          } else {
            return (
              <div className="table-display-overlay" style={{textAlign:'center'}}>
                { text }
              </div>
            )
          }
        }
      }
      return col;
    });

    const dataCount = data.length;
    const cursor = dataCount > 0 ? 'pointer' : 'default';
    const upDownIcon = dataCount > 0 ? (open ? <UpCircleOutlined /> : <DownCircleOutlined />) : null;
    const pagination = (dataCount > 10);

    const countBadge = (loading === true) ?
      <Spin size="small" style={{marginLeft:10}} /> :
      (dataCount > 0) ?
        <Badge size="small" count={dataCount} style={{ backgroundColor: '#52c41a' }} /> :
        <span style={{color:'#777', fontStyle:'italic'}}>Элементы отсутствуют</span>;

    return (
      <div className="tableBlock">
        <div className="containerBlock-header" onClick={() => this.openToggle()} style={{
          cursor
        }}>
          <strong>{title}</strong> {countBadge} <span className="pull-right">{upDownIcon}</span>
        </div>
        { (dataCount > 0 && open === true) && (
          <Table
            bordered
            size="small"
            dataSource={data}
            columns={columnsParsed}
            pagination={pagination} />
        )}
      </div>
    );
  }
}
