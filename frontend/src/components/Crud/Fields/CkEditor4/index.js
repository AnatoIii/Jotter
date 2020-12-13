import React from 'react'
import CKEditor from 'ckeditor4-react';

export default class CkEditor4 extends React.Component
{
  handleOnChange = (value) => {
    const { item } = this.props;
    const currentKey = item.key.edit ? item.key.edit : item.key;
    const put = {
      [currentKey]: value
    };
    this.props.action(put);
  };

  onEditorChange = (evt) => {
    const value = evt.editor.getData();
    const { item } = this.props;
    const currentKey = item.key.edit ? item.key.edit : item.key;
    const put = {
      [currentKey]: value
    };
    this.props.action(put);
  };

  render() {
    const { item, value, options } = this.props;
    return (
      <div>
        { (typeof value !== "undefined") && (
          <CKEditor
            onBeforeLoad = { ( CKEDITOR ) => CKEDITOR.on('dialogDefinition', async (event)=> {
              // if(options && options.placeholders) {
                if ('placeholder' === event.data.name) {
                  let input = event.data.definition.getContents('info').get('name');
                  input.items = options.placeholders;
                }
              // }
            }) }
            key={item.key}
            data={value.toString()}
            onChange={evt => this.onEditorChange( evt )}
            config={{language: 'ru'}}
          />
        )}
      </div>
    );
  }
}
