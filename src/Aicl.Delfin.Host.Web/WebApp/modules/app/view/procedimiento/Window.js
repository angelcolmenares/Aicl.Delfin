/*
 * File: app/view/procedimiento/Window.js
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

Ext.define('App.view.procedimiento.Window', {
    extend: 'Ext.window.Window',
    alias: 'widget.procedimientowindow',

    width: 800,
    closeAction: 'hide',
    title: 'Procedimientos',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    name: 'ProcedimientoToolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            action: 'new',
                            iconCls: 'new_document'
                        },
                        {
                            xtype: 'textfield',
                            name: 'FindProcedimiento',
                            hideLabel: true
                        },
                        {
                            xtype: 'button',
                            action: 'find',
                            iconCls: 'open_document'
                        },
                        {
                            xtype: 'button',
                            action: 'save',
                            iconCls: 'save_document'
                        },
                        {
                            xtype: 'button',
                            action: 'remove',
                            iconCls: 'remove'
                        },
                        {
                            xtype: 'button',
                            action: 'select',
                            iconCls: 'select'
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'panel',
                    name: 'ProcedimientoPanel',
                    layout: {
                        align: 'stretch',
                        type: 'hbox'
                    },
                    bodyPadding: 10,
                    items: [
                        {
                            xtype: 'gridpanel',
                            name: 'ProcedimientoList',
                            flex: 1,
                            padding: 5,
							store: 'Procedimiento',
							bbar: Ext.create('Ext.PagingToolbar', {
            					store: 'Procedimiento',
					            displayInfo: true,
        		    			displayMsg: 'Procedimientos del {0} al {1} de {2}',
        		    			emptyMsg: "No hay Procedimientos para Mostrar"
						    }),
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Nombre',
                                    flex: 1,
                                    text: 'Nombre'
                                }
                            ],
                            viewConfig: {

                            }
                        },
                        {
                            xtype: 'form',
                            name: 'ProcedimientoForm',
                            fieldDefaults: {
                                msgTarget: 'side',
                                labelWidth: 80,
                                labelAlign: 'right'
                            },
                            flex: 1.25,
                            padding: 5,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'hiddenfield',
                                    anchor: '100%',
                                    name: 'Id',
                                    fieldLabel: 'Label'
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name: 'Nombre',
                                    fieldLabel: 'Nombre',
                                    allowBlank: false,
                                    enforceMaxLength: true,
                                    maxLength: 96,
                                    maxLengthText: 'The maximum length for this field is {96}'
                                },
                                {
                                    xtype: 'textareafield',
                                    anchor: '100%',
                                    height: 204,
                                    width: 353,
                                    name: 'Descripcion',
                                    fieldLabel: 'Descripcion',
                                    allowBlank: false,
                                    enforceMaxLength: true,
                                    maxLength: 4096
                                },
                                {
                                    xtype: 'numberfield',
                                    anchor: '50%',
                                    name: 'ValorUnitario',
                                    value: 0,
                                    fieldLabel: 'Valor Total',
                                    allowBlank: false,
                                    allowDecimals: false
                                },
                                {
                                    xtype: 'numberfield',
                                    anchor: '50%',
                                    name: 'PorcentajeIva',
                                    value: 0,
                                    fieldLabel: 'Iva %',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'checkboxfield',
                                    anchor: '100%',
                                    name: 'Activo',
                                    fieldLabel: 'Activo'
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '50%',
                                    name: 'ValorIva',
                                    readOnly: true,
                                    submitValue: false,
                                    fieldLabel: 'Iva $'
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '50%',
                                    name: 'ValorBase',
                                    readOnly: true,
                                    submitValue: false,
                                    fieldLabel: 'Valor Base'
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