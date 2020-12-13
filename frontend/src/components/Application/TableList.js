import React from 'react'
const TableList = ({data}) => {
  const tableRows = data.length > 0 ? data.map(row => {
    return (
      <tr key={`tableList-row-key-${row.key}`}>
        <td style={{borderColor: '#dfe2e7', padding:8, width:'40%'}}>{row.key}</td>
        <td style={{borderColor: '#dfe2e7', padding:8}}>{row.title}</td>
      </tr>
    )
  }) : [];

  return (
    <table border="1" style={{width:'100%',border:'1px solid #dfe2e7',borderCollapse:'collapsed'}}>
      <tbody>
        {tableRows}
      </tbody>
    </table>
  );
};
export default TableList;
