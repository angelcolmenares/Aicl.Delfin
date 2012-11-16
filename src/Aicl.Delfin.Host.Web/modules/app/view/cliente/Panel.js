/*
 * File: app/view/cliente/Panel.js
 *
 * This file was generated by Sencha Architect version 2.1.0.
 * http://www.sencha.com/products/architect/
 *
 * This file requires use of the Ext JS 4.1.x library, under independent license.
 * License of Sencha Architect does not include license for Ext JS 4.1.x. For more
 * details see http://www.sencha.com/license or contact license@sencha.com.
 *
 * This file will be auto-generated each and everytime you save your project.
 *
 * Do NOT hand edit this file.
 */

Ext.define('App.view.cliente.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.clientepanel',

    border: '',
    padding: '',
    style: 'border:0',
    ui: 'default-framed',
    width: 950,
    bodyBorder: true,
    title: '',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    name: 'MainToolbar',
                    dock: 'top',
                    margin: 2,
                    style: 'border:0;padding:0;',
                    layout: {
                        padding: 4,
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'button',
                            action: 'new',
                            iconCls: 'new_document',
                            tooltip: 'Crear nuevo cliente'
                        },
                        {
                            xtype: 'textfield',
                            width: 300,
                            name: 'FindCliente',
                            fieldLabel: '',
                            hideLabel: true,
                            emptyText: 'Nit/NombreCliente'
                        },
                        {
                            xtype: 'button',
                            action: 'find',
                            iconCls: 'open_document',
                            tooltip: 'Buscar cliente por Nit o Nombre'
                        },
                        {
                            xtype: 'button',
                            action: 'save',
                            iconCls: 'save_document',
                            tooltip: 'Guardar cambios'
                        },
                        {
                            xtype: 'button',
                            action: 'remove',
                            iconCls: 'remove',
                            tooltip: 'Borrar Cliente'
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'form',
                    fieldDefaults: {
                        msgTarget: 'side',
                        labelWidth: 80,
                        labelAlign: 'right'
                    },
                    name: 'ClienteForm',
                    margin: 10,
                    padding: '2 2 0 2',
                    ui: 'default-framed',
                    bodyBorder: false,
                    bodyPadding: '2 50 2 50',
                    title: '',
                    items: [
                        {
                            xtype: 'hiddenfield',
                            anchor: '100%',
                            name: 'Id',
                            fieldLabel: 'Nit'
                        },
                        {
                            xtype: 'textfield',
                            anchor: '50%',
                            name: 'Nit',
                            fieldLabel: 'Nit',
                            allowBlank: false,
                            enforceMaxLength: true,
                            maxLength: 16,
                            maxLengthText: 'The maximum length for this field is 16'
                        },
                        {
                            xtype: 'textfield',
                            anchor: '80%',
                            name: 'Nombre',
                            fieldLabel: 'Nombre',
                            allowBlank: false,
                            enforceMaxLength: true,
                            maxLength: 128,
                            maxLengthText: 'The maximum length for this field is {128}'
                        },
                        {
                            xtype: 'checkboxfield',
                            anchor: '100%',
                            name: 'Activo',
                            fieldLabel: 'Activo',
                            hideLabel: false,
                            boxLabel: '',
                            checked: true
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    margin: 10,
                    ui: 'default-framed',
                    layout: {
                        align: 'stretch',
                        padding: '2 5 0 5',
                        type: 'hbox'
                    },
                    animCollapse: false,
                    collapsible: true,
                    title: 'Contactos',
                    items: [
                        {
                            xtype: 'gridpanel',
                            name: 'ContactoList',
                            flex: 1,
                            height: 250,
                            margin: 5,
                            ui: 'default-framed',
                            autoScroll: true,
                            title: '',
                            enableColumnHide: false,
                            enableColumnMove: false,
                            enableColumnResize: false,
                            sortableColumns: false,
                            store: 'Contacto',
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    draggable: false,
                                    resizable: false,
                                    sortable: false,
                                    dataIndex: 'Nombre',
                                    flex: 1,
                                    hideable: false,
                                    text: 'Nombre'
                                }
                            ]
                        },
                        {
                            xtype: 'form',
                            name: 'ContactoForm',
                            fieldDefaults: {
                                msgTarget: 'side',
                                labelWidth: 80,
                                labelAlign: 'right'
                            },
                            flex: 1,
                            border: 0,
                            height: 250,
                            margin: 5,
                            style: {
                                border: 0,
                                padding: 0
                            },
                            ui: 'default-framed',
                            width: 325,
                            bodyPadding: '2 10 0 10',
                            title: '',
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    name: 'ContactoToolbar',
                                    dock: 'top',
                                    border: '',
                                    items: [
                                        {
                                            xtype: 'button',
                                            action: 'new',
                                            iconCls: 'new_document',
                                            tooltip: 'nuevo contacto'
                                        },
                                        {
                                            xtype: 'button',
                                            action: 'save',
                                            iconCls: 'save_document',
                                            tooltip: 'guardar cambios'
                                        },
                                        {
                                            xtype: 'button',
                                            action: 'remove',
                                            iconCls: 'remove',
                                            tooltip: 'borrar contacto'
                                        }
                                    ]
                                }
                            ],
                            items: [
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name: 'Nombre',
                                    fieldLabel: 'Nombre',
                                    allowBlank: false,
                                    enforceMaxLength: true,
                                    maxLength: 128,
                                    maxLengthText: 'Maximo 128 caracteres'
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name: 'Cargo',
                                    fieldLabel: 'Cargo',
                                    allowBlank: false,
                                    enforceMaxLength: true,
                                    maxLength: 64,
                                    maxLengthText: 'Maximo 64 caracteres'
                                },
                                {
                                    xtype: 'fieldset',
                                    frame: false,
                                    style: {
                                        border: 0,
                                        padding: 0
                                    },
                                    layout: {
                                        align: 'stretch',
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            flex: 1,
                                            name: 'Telefono',
                                            fieldLabel: 'Telefono',
                                            enforceMaxLength: true,
                                            maxLength: 16,
                                            maxLengthText: 'Maximo 16 caracteres'
                                        },
                                        {
                                            xtype: 'textfield',
                                            flex: 1,
                                            name: 'Fax',
                                            fieldLabel: 'Fax',
                                            enforceMaxLength: true,
                                            maxLength: 16,
                                            maxLengthText: 'Maximo 16 caracteres'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name: 'Celular',
                                    fieldLabel: 'Celular',
                                    enforceMaxLength: true,
                                    maxLength: 16,
                                    maxLengthText: 'Maximo 16 Caracteres'
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name: 'Mail',
                                    fieldLabel: 'Mail',
                                    enforceMaxLength: true,
                                    maxLength: 128,
                                    maxLengthText: 'Maximo 128 Caracteres',
                                    vtype: 'email'
                                },
                                {
                                    xtype: 'combobox',
                                    tpl: '<tpl for="."><div class="x-boundlist-item">{Nombre}  {Codigo}</div></tpl>',
                                    displayTpl: '<tpl for=".">{Nombre} {Codigo}</tpl>',
                                    anchor: '100%',
                                    name: 'IdCiudad',
                                    fieldLabel: 'Ciudad',
                                    allowBlank: false,
                                    displayField: 'Nombre',
                                    forceSelection: true,
                                    multiSelect: false,
                                    pageSize: 12,
                                    queryParam: 'Nombre',
                                    store: 'Ciudad',
                                    valueField: 'Id'
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name: 'Direccion',
                                    fieldLabel: 'Direccion',
                                    allowBlank: false,
                                    enforceMaxLength: true,
                                    maxLength: 256,
                                    maxLengthText: 'Maximo 256 Caracteres'
                                },
                                {
                                    xtype: 'checkboxfield',
                                    anchor: '100%',
                                    name: 'Activo',
                                    fieldLabel: 'Activo',
                                    boxLabel: '',
                                    checked: true
                                },
                                {
                                    xtype: 'hiddenfield',
                                    anchor: '0%',
                                    name: 'Id',
                                    fieldLabel: 'Label'
                                },
                                {
                                    xtype: 'hiddenfield',
                                    anchor: '0%',
                                    name: 'IdCliente',
                                    fieldLabel: 'Label'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    margin: 10,
                    ui: 'default-framed',
                    layout: {
                        align: 'stretch',
                        padding: '2 5 0 5',
                        type: 'hbox'
                    },
                    collapsible: true,
                    title: 'Tareas',
                    items: [
                        {
                            xtype: 'gridpanel',
                            flex: 1,
                            name: 'TareaList',
                            height: 150,
                            autoScroll: true,
                            enableColumnHide: false,
                            enableColumnMove: false,
                            enableColumnResize: false,
                            sortableColumns: false,
                            store: 'Tarea',
                            columns: [
                                {
                                    xtype: 'booleancolumn',
                                    draggable: false,
                                    width: 70,
                                    resizable: false,
                                    sortable: false,
                                    align: 'center',
                                    dataIndex: 'Cumplida',
                                    hideable: false,
                                    text: 'Estado',
                                    falseText: 'Pendiente',
                                    trueText: 'Cumplida'
                                },
                                {
                                    xtype: 'datecolumn',
                                    draggable: false,
                                    width: 70,
                                    resizable: false,
                                    sortable: false,
                                    align: 'center',
                                    dataIndex: 'Fecha',
                                    hideable: false,
                                    text: 'Fecha',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    renderer: function(value, metaData, record, rowIndex, colIndex, store, view) {

                                        var bgColor;
                                        var color;

                                        if(record.get('Cumplida')){
                                            bgColor='#FFFFFF'; //white
                                            color='#006400';   // dark green
                                        }
                                        else{
                                            if(Aicl.Util.isToday(record.get('Fecha'))){
                                                bgColor='#FFA500';  //orange
                                                color='#000000'; //black
                                            }
                                            else{
                                                if(Aicl.Util.isDueDate(record.get('Fecha'))){
                                                    bgColor='#FF0000'; //red
                                                    color='#000000';
                                                }
                                                else{
                                                    bgColor='#FFFFFF'; // white 
                                                    color='#000000';   // black
                                                }
                                            }

                                        }
                                        //'<p style="white-space:normal;color:{0}; background-color:{1};">{2}</p>'

                                        return Ext.String.format('<div style="white-space:normal;color:{0}; background-color:{1};">{2}</div>',
                                        color,
                                        bgColor,
                                        value);
                                    },
                                    draggable: false,
                                    resizable: false,
                                    sortable: false,
                                    dataIndex: 'Tema',
                                    flex: 1,
                                    hideable: false,
                                    text: 'Tarea'
                                }
                            ],
                            viewConfig: {

                            }
                        },
                        {
                            xtype: 'form',
                            flex: 1,
                            name: 'TareaForm',
                            fieldDefaults: {
                                msgTarget: 'side',
                                labelWidth: 80,
                                labelAlign: 'right'
                            },
                            margin: '5 5 0 5',
                            style: {
                                border: 0,
                                padding: 0
                            },
                            ui: 'default-framed',
                            bodyPadding: '2 5 0 5',
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    name: 'TareaToolbar',
                                    border: 0,
                                    items: [
                                        {
                                            xtype: 'button',
                                            action: 'new',
                                            iconCls: 'new_document',
                                            tooltip: 'nueva tarea'
                                        },
                                        {
                                            xtype: 'button',
                                            action: 'save',
                                            iconCls: 'save_document',
                                            tooltip: 'guardar tarea'
                                        },
                                        {
                                            xtype: 'button',
                                            action: 'remove',
                                            iconCls: 'remove',
                                            tooltip: 'borrar tarea'
                                        }
                                    ]
                                }
                            ],
                            items: [
                                {
                                    xtype: 'hiddenfield',
                                    anchor: '100%',
                                    name: 'Id',
                                    fieldLabel: 'Label'
                                },
                                {
                                    xtype: 'hiddenfield',
                                    anchor: '100%',
                                    name: 'IdCliente',
                                    fieldLabel: 'Label'
                                },
                                {
                                    xtype: 'datefield',
                                    width: 400,
                                    name: 'Fecha',
                                    fieldLabel: 'Fecha',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'textareafield',
                                    height: 30,
                                    width: 400,
                                    name: 'Tema',
                                    fieldLabel: 'Tarea',
                                    enforceMaxLength: true,
                                    maxLength: 128,
                                    maxLengthText: 'The maximum length for this field is 128'
                                },
                                {
                                    xtype: 'checkboxfield',
                                    anchor: '100%',
                                    name: 'Cumplida',
                                    fieldLabel: 'Cumplida ?'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});