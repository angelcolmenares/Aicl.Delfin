Ext.define('App.view.pedido.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.pedidopanel',

    style: 'border: 0; padding: 0',
    ui: 'default-framed',
    width: 990,
    
    showPedidoSearchWindow:function(){
    	this.pedidoSearchWindow.show();
    },
    hidePedidoSearchWindow:function(){
    	this.pedidoSearchWindow.hide();
    },
    showClienteSearchWindow:function(){
    	this.clienteSearchWindow.show();
    },
    hideClienteSearchWindow:function(){
    	this.clienteSearchWindow.hide();
    },

    initComponent: function() {
    	this.pedidoSearchWindow=Ext.create('App.view.pedidosearch.Window');
	  	this.clienteSearchWindow=Ext.create('App.view.clientesearch.Window');
    	
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    name: 'mainToolbar',
                    dock: 'top',
                    style: 'border: 0; padding: 0',
                    items: [
                        {
                            xtype: 'button',
                            action: 'new',
                            iconCls: 'new_document'
                        },
                        {
                            xtype: 'textfield',
                            width: 300,
                            name: 'buscarPedidoText',
                            hideLabel: true,
                            emptyText: 'Consecutivo/Cliente'
                        },
                        {
                            xtype: 'button',
                            name: 'buscarPedido',
                            action: 'buscarPedido',
                            iconCls: 'open_document'
                        },
                        {
                            xtype: 'button',
                            action: 'save',
                            iconCls: 'save_document'
                        },
                        {
                            xtype: 'button',
                            action: 'enviar',
                            tooltip:'enviar pedido',
                            iconCls: 'send'
                        },
                        {
                            xtype: 'button',
                            action: 'aceptar',
                            tooltip:'Registrar aceptacion del pedido',
                            iconCls: 'accept'
                        },
                        {
                            xtype: 'button',
                            action: 'anular',
                            tooltip:'Anular pedido',
                            iconCls: 'remove'
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'pedidoform'
                },
                {xtype:'pedidoitempanel'}
            ]
        });

        me.callParent(arguments);
    }

});


Ext.define('App.view.pedido.Form', {
    extend: 'Ext.form.Panel',
    alias: 'widget.pedidoform',

    ui: 'default-framed',
    layout: {
        align: 'stretch',
        type: 'hbox'
    },
    bodyPadding: 10,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            fieldDefaults: {
                msgTarget: 'side',
                labelWidth: 80,
                labelAlign: 'right'
            },
            defaults: {
                width: 350
            },
            items: [
                {
                    xtype: 'fieldset',
                    flex: 0,
                    style: 'border: 0; padding: 0',
                    items: [
                        {
                            xtype: 'fieldset',
                            style: 'border: 0; padding: 0',
                            layout: {
                                align: 'stretch',
                                type: 'hbox'
                            },
                            items: [{
									xtype: 'hidden',
									name: 'Id'
								},{
									xtype: 'hidden',
									name: 'IdContacto'
								},
                                {
                                    xtype: 'textfield',
                                    flex: 1,
                                    name: 'NitCliente',
                                    fieldLabel: 'Nit'
                                },
                                {
                                    xtype: 'button',
                                    flex: 0,
                                    iconCls: 'find',
                                    style: 'marginLeft:4px',
                                    tooltip: 'Buscar por Nit',
                                    action:'buscarClientePorNit'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            style: 'border:0; padding:0',
                            layout: {
                                align: 'stretch',
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    anchor: '100%',
                                    name:'NombreCliente',
                                    flex: 1,
                                    fieldLabel: 'Cliente'
                                },
                                {
                                    xtype: 'button',
                                    flex: 0,
                                    iconCls: 'find',
                                    style: 'marginLeft:4px',
                                    tooltip: 'Buscar por Nombre',
                                    action:'buscarClientePorNombre'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            anchor: '100%',
                            name: 'NombreContacto',
                            readOnly: true,
                            fieldLabel: 'Contacto'
                        },
                        {
                            xtype: 'formapagocombo',
							fieldLabel: 'FormaPago',
							allowBlank: false,
							anchor:'100%'
                        },
                        {
                            xtype: 'numberfield',
                            anchor: '50%',
                            name: 'DiasDeVigencia',
                            fieldLabel: 'Vigencia',
                            allowDecimals: false,
                            value:15
                        },
                        {
                            xtype: 'datefield',
                            anchor: '50%',
                            name: 'VigenteHasta',
                            readOnly: true,
                            fieldLabel: 'Vigente Hasta'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    flex: 0,
                    margins: '0 0 0 30',
                    style: 'border:0; padding:0',
                    defaults: {
                        width: 350
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'NombreDestinatario',
                            fieldLabel: 'Destinatario'
                        },
                        {
                            xtype: 'textfield',
                            name: 'CargoDestinatario',
                            fieldLabel: 'Cargo'
                        },
                        {
                            xtype: 'fieldset',
                            style: 'border:0;padding:0',
                            layout: {
                                align: 'stretch',
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'TelefonoDestinatario',
                                    fieldLabel: 'Tel-Fax'
                                },
                                {
                                    xtype: 'textfield',
                                    flex: 1,
                                    style: 'marginLeft:4px',
                                    name: 'FaxDestinatario',
                                    hideLabel: true
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'CelularDestinatario',
                            fieldLabel: 'Celular'
                        },
                        {
                            xtype: 'textfield',
                            name: 'MailDestinatario',
                            fieldLabel: 'Mail',
                            vtype: 'email'
                        },
                        {
                            name:'IdCiudadDestinatario',
							xtype: 'ciudadcombo',
							fieldLabel: 'Ciudad',
							allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'DireccionDestinatario',
                            fieldLabel: 'Direccion'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    flex: 1,
                    margins: '0 0 0 40',
                    style: 'border:0;padding:0',
                    items: [
                        {
                            xtype: 'numberfield',
                            anchor: '100%',
                            name: 'Consecutivo',
                            readOnly: true,
                            fieldLabel: 'Consecutivo'
                        },
                        {
                            xtype: 'datefield',
                            anchor: '100%',
                            name: 'FechaCreacion',
                            readOnly: true,
                            fieldLabel: 'Creado',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            anchor: '100%',
                            name: 'FechaActualizacion',
                            readOnly: true,
                            fieldLabel: 'Actualizado',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            anchor: '100%',
                            name: 'FechaEnvio',
                            readOnly: true,
                            fieldLabel: 'Enviado',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            anchor: '100%',
                            name: 'FechaAceptacion',
                            readOnly: true,
                            fieldLabel: 'Aceptado',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            anchor: '100%',
                            name: 'FechaAnulado',
                            readOnly: true,
                            fieldLabel: 'Anulado',
                            format: 'd.m.Y'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});


Ext.define('App.view.pedido.List',{ 
    extend: 'Ext.grid.Panel',
    alias : 'widget.pedidolist', 
    frame: false,
    selType:'rowmodel',
    height: '100%',
    viewConfig : {	stripeRows: true  },
    
    initComponent: function() {
    this.store= 'Pedido';	
    this.bbar= Ext.create('Ext.PagingToolbar', {
            store: this.store,
            displayInfo: true,
            displayMsg: 'Pedidos del {0} al {1} de {2}',
            emptyMsg: "No hay Pedidos para Mostrar"
    });
        
            this.columns=[
	{
		text: 'Cnstv',
		width:50,
		dataIndex: 'Consecutivo',
		align: 'center',
        renderer: function(value, metadata, record, store){
			return Ext.String.format(
			'<div class="x-cell-positive" style="text-align:center">{0}</div>',
			value); 
		}
	},
	{
		text: 'Nit',
		dataIndex: 'NitCliente'
	},
	{
		text: 'Cliente',
		width:200,
		dataIndex: 'NombreCliente'
	},
	{
		text: 'Contacto',
		dataIndex: 'NombreContacto'
	},
	{
		text: 'Destinatario',
		dataIndex: 'NombreDestinatario'
	},
	{
		text: 'Envio',
		dataIndex: 'FechaEnvio',
		renderer: Ext.util.Format.dateRenderer('d.m.Y')
	},
	{
		text: 'Aceptacion',
		dataIndex: 'FechaAceptacion',
		renderer: Ext.util.Format.dateRenderer('d.m.Y')
	},
	{
		text: 'Anulado',
		dataIndex: 'FechaAnulado',
		renderer: Ext.util.Format.dateRenderer('d.m.Y')
	},
	{
		text: 'CreadoPor',
		dataIndex: 'NombreCreadoPor'
	},
	{
		text: 'FormaPago',
		dataIndex: 'DescripcionFormaPago'
	}
];

	
    this.dockedItems=[{
        xtype: 'toolbar',
        items: [{
            text:'Seleccionar',
     		tooltip:'Seleccionar Pedido',
      		iconCls:'select',
    		disabled:true,
    		action:'select'
        }]		
    }]
                
    this.callParent(arguments);
    
    }
});


Ext.define('App.view.pedidosearch.Window',{
	extend: 'Ext.Window',
    alias : 'widget.pedidosearchwindow',
	closable: true,
    closeAction: 'hide',
    y:22,
    x:22,
    autoHeight:true,
    width: 950,
    modal: false,
    items:[{
    	xtype:'pedidolist'
    }]
});

Ext.define('App.view.clientesearch.Window',{
	extend: 'Ext.Window',
    alias : 'widget.clientesearchwindow',
	closable: true,
    closeAction: 'hide',
    y:100,
    x:105,
    autoHeight:true,
    width: 400,
    modal: false,
    items:[{
    	xtype:'clientecontactolist'
    }]
});



Ext.define('App.view.clientecontacto.List',{ 
    extend: 'Ext.grid.Panel',
    alias : 'widget.clientecontactolist', 
    frame: false,
    selType:'rowmodel',
    height: '100%',
    viewConfig : {	stripeRows: true  },
    
    initComponent: function() {
    this.store= 'ClienteContacto';	
    this.bbar= Ext.create('Ext.PagingToolbar', {
            store: this.store,
            displayInfo: true,
            displayMsg: 'Cliente del {0} al {1} de {2}',
            emptyMsg: "No hay Clientes para Mostrar"
    });
        
        this.columns=[{
		text: 'Nit',
		dataIndex: 'Nit',
		sortable: true
	},
	{
		text: 'Nombre',
		dataIndex: 'Nombre',
		sortable: true
	},
	{
		text: 'NombreContacto',
		dataIndex: 'NombreContacto',
		sortable: true
	},
	{
		text: 'CargoContacto',
		dataIndex: 'CargoContacto',
		sortable: true
	},
	{
		text: 'TelefonoContacto',
		dataIndex: 'TelefonoContacto',
		sortable: true
	},
	{
		text: 'FaxContacto',
		dataIndex: 'FaxContacto',
		sortable: true
	},
	{
		text: 'CelularContacto',
		dataIndex: 'CelularContacto',
		sortable: true
	},
	{
		text: 'MailContacto',
		dataIndex: 'MailContacto',
		sortable: true
	},
	{
		text: 'DireccionContacto',
		dataIndex: 'DireccionContacto',
		sortable: true
	},
	{
		text: 'CodigoPostalContacto',
		dataIndex: 'CodigoPostalContacto',
		sortable: true
	},
	{
		text: 'NombreCiudad',
		dataIndex: 'NombreCiudad',
		sortable: true
	}
	
];

    this.dockedItems=[{
        xtype: 'toolbar',
        items: [{
            text:'Seleccionar',
     		tooltip:'Seleccionar Cliente',
      		iconCls:'select',
    		disabled:true,
    		action:'select'
        }]		
    }]
                
    this.callParent(arguments);
  
    }
});


Ext.define('App.view.formapago.ComboBox', {
	extend:'Ext.ux.form.field.BoxSelect',
	alias : 'widget.formapagocombo',
    displayField: 'Descripcion',
	valueField: 'Id',
	name:'IdFormaPago',
    store: 'FormaPago',
    forceSelection:true,
    //pageSize: 12,
    multiSelect:false,
    queryMode: 'local',
    queryParam :'Descripcion',
    triggerOnClick: false,
    labelTpl: '{Descripcion}',
    listConfig: {
        tpl: Ext.create('Ext.XTemplate',
            '<ul><tpl for=".">',
                '<li role="option" class="' + Ext.baseCSSPrefix + 'boundlist-item' + '">{Descripcion}</li>',
            '</tpl></ul>'
    )}
});


Ext.define('App.view.ciudad.ComboBox', {
	extend:'Ext.ux.form.field.BoxSelect',
	alias : 'widget.ciudadcombo',
    displayField: 'Nombre',
	valueField: 'Id',
    store: 'Ciudad',
    forceSelection:true,
    pageSize: 12,
    multiSelect:false,
    queryMode: 'remote',
    queryParam :'Nombre',
    triggerOnClick: false,
    labelTpl: '{Nombre} - {Codigo} ',
    listConfig: {
        tpl: Ext.create('Ext.XTemplate',
            '<ul><tpl for=".">',
                '<li role="option" class="' + Ext.baseCSSPrefix + 'boundlist-item' + '">{Nombre} {Codigo}</li>',
            '</tpl></ul>'
    )}
});


Ext.define('App.view.pedidoitem.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.pedidoitempanel',
    
    height: 347,
    style: 'border: 0; padding: 0',
    ui: 'default-framed',
    width: 990,
    layout: {
        align: 'stretch',
        type: 'hbox'
    },
    frameHeader: false,
    title: '',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    flex: 1,
                    height: 320,
                    style: 'border: 0; padding: 0',
                    ui: 'default-framed',
                    width: 650,
                    bodyPadding: '10 2,2,2',
                    frameHeader: false,
                    title: '',
                    items: [
                        {
                            xtype: 'pedidoitemlist'
                        },
                        {
                            xtype: 'panel',
                            height: 210,
                            margin: '5 0 0 0',
                            ui: 'default-framed',
                            layout: {
                                align: 'stretch',
                                type: 'hbox'
                            },
                            bodyPadding: 2,
                            title: '',
                            items: [
                                {
                                    xtype: 'itemform',
                                    width: 395
                                },
                                {
                                    xtype: 'itemresumenform',
                                    flex: 1
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    frame: false,
                    margin: '10 0 0 0',
                    style: '',
                    ui: 'default-framed',
                    width: 300,
                    bodyPadding: 2,
                    frameHeader: false,
                    title: '',
                    items: [
                        {
                            xtype: 'procedimientoform'
                        },
                        {
                            xtype: 'pedidoresumenform',
                            margin: '30 0 0 0'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('App.view.item.Form', {
    extend: 'Ext.form.Panel',
    alias: 'widget.itemform',

    style: 'border: 0; padding: 0',
    ui: 'default-framed',
    width: 350,
    bodyPadding: 2,
    frameHeader: false,
    title: '',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            fieldDefaults: {
                msgTarget: 'side',
                labelWidth: 80,
                labelAlign: 'right'
            },
            defaults: {
                anchor: '100%',
                labelStyle: 'padding-left:4px;'
            },
            items: [
                {
                    xtype: 'fieldset',
                    style: 'border: 0; padding: 0',
                    layout: {
                        align: 'stretch',
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            flex: 1,
                            name: 'NombreServicio',
                            fieldLabel: 'Servicio',
                            emptyText: 'nombre del servicio'
                        },
                        {
                            xtype: 'button',
                            flex: 0,
                            style: 'marginLeft:4px',
                            iconCls: 'find',
                            tooltip: 'buscar servicio'
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'Descripcion',
                    fieldLabel: 'Detalle'
                },
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'Nota',
                    fieldLabel: 'Nota'
                },
                {
                    xtype: 'numberfield',
                    anchor: '50%',
                    name: 'Cantidad',
                    fieldLabel: 'Cantidad',
                    allowDecimals: false,
                    decimalPrecision: 0
                },
                {
                    xtype: 'numberfield',
                    anchor: '50%',
                    name: 'DiasEntrega',
                    fieldLabel: 'Dias Entrega',
                    allowDecimals: false,
                    decimalPrecision: 0
                },{
                    xtype: 'numberfield',
                    anchor: '50%',
                    name: 'Descuento',
                    fieldLabel: 'Descuento %',
                    decimalPrecision: 6
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    style: 'border: 0; padding: 0',
                    ui: 'default-framed',
                    items: [
                        {
                            xtype: 'button',
                            iconCls: 'new_document',
                            text: ''
                        },
                        {
                            xtype: 'button',
                            iconCls: 'save_document',
                            text: '',
                            tooltip: 'Guardar'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'remove',
                            text: '',
                            tooltip: 'borrar'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('App.view.pedidoitem.List', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.pedidoitemlist',

    height: 121,
    style: 'border: 0; padding: 0',
    ui: 'default-framed',
 
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columns: [
    {
		text: 'Servicio',
		dataIndex: 'NombreServicio',
		width:300,
		renderer: function(value, metadata, record, store){   	
            return Ext.String.format( '<p style="white-space:normal;color:black;">{0}<br />{1}<br />{2}</p>',
            value,
            record.get('Descripcion'),
            record.get('Nota')?'Nota:' +record.get('Nota'):''
            );
        }
	},           	
	{
		text: 'Ctd',
		width:40,
		dataIndex: 'Cantidad',
		renderer: function(value, metadata, record, store){
			return Ext.String.format(
			'<div class="x-cell-positive" style="text-align:center">{0}</div>',
			value); 
		}
	},
	{
		text: 'Dto %',
		width:40,
		dataIndex: 'Descuento',
		renderer: function(value, metadata, record, store){
           	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        }
	},
	{
		text: 'Iva %',
		width:40,
		dataIndex: 'PorcentajeIva',
		renderer: function(value, metadata, record, store){
           	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        }
	},
	{
		text: 'Dias',
		width:40,
		dataIndex: 'DiasEntrega',
		renderer: function(value, metadata, record, store){
           	return Ext.String.format(
			'<div class="x-cell-positive" style="text-align:center">{0}</div>',
			value);
        }
	},
	{
		text: 'Unitario',
		dataIndex: 'CostoUnitario',
		renderer: function(value, metadata, record, store){
           	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        }
	},
	{
		text: 'Inversion',
		dataIndex: 'CostoInversion',
		renderer: function(value, metadata, record, store){
           	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        }
	},
	{
		text: 'Iva',
		dataIndex: 'ValorIva',
		renderer: function(value, metadata, record, store){
           	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        }
	},
	{
		text: 'Total',
		dataIndex: 'TotalItem',
		renderer: function(value, metadata, record, store){
           	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        }
	}

            ],
            viewConfig: {

            }
        });

        me.callParent(arguments);
    }

});
Ext.define('App.view.itemresumen.Form', {
    extend: 'Ext.form.Panel',
    alias: 'widget.itemresumenform',

    style: 'border: 0; padding: 0',
    ui: 'default-framed',
    bodyPadding: '20 10 10 10',
    frameHeader: false,
    title: '',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            fieldDefaults: {
                msgTarget: 'side',
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'CostoUnitario',
                    readOnly: true,
                    fieldLabel: 'Costo Unitario'
                },
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'CostoInversion',
                    readOnly: true,
                    fieldLabel: 'Costo Inversion'
                },
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'ValorIva',
                    readOnly: true,
                    fieldLabel: 'IVA'
                },
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'TotalItem',
                    readOnly: true,
                    fieldLabel: 'Total'
                }
            ]
        });

        me.callParent(arguments);
    }

});

Ext.define('App.view.procedimiento.Form', {
    extend: 'Ext.form.Panel',
    alias: 'widget.procedimientoform',

    height: 150,
    style: 'border: 0; padding: 0',
    ui: 'default-framed',
    width: 285,
    bodyPadding: 0,
    frameHeader: false,
    title: 'Procedimiento',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textareafield',
                    anchor: '100%',
                    height: 97,
                    width: 285,
                    name: 'DescripcionProcedimiento',
                    readOnly: true,
                    hideLabel: true
                }
            ]
        });

        me.callParent(arguments);
    }

});
Ext.define('App.view.pedidoresumen.Form', {
    extend: 'Ext.form.Panel',
    alias: 'widget.pedidoresumenform',

    style: '\'border: 0; padding: 0\'',
    ui: 'default-framed',
    width: 285,
    bodyPadding: '20 20 10 10',
    frameHeader: false,
    title: 'Resumen Oferta',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            fieldDefaults: {
                msgTarget: 'side',
                labelWidth: 80,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'SubtotalOferta',
                    value: 0,
                    fieldLabel: 'Subtotal',
                    readOnly:true
                },
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'IvaOferta',
                    value: 0.00,
                    fieldLabel: 'Iva',
                    readOnly:true
                },
                {
                    xtype: 'textfield',
                    anchor: '100%',
                    name: 'TotalOferta',
                    submitValue: false,
                    value: '0.00',
                    fieldLabel: 'Total',
                    readOnly:true
                }
            ]
        });

        me.callParent(arguments);
    }

});