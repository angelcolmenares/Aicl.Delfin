Ext.define('App.view.pedido.Panel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.pedidopanel',


    style: 'border: 0; padding: 0',
    ui: 'default-framed',
    width: 990,
    frameHeader: false,
    title: '',
    
    showSearchWindow:function(){
    	this.searchWindow.show();
    },
    hideSearchWindow:function(){
    	this.searchWindow.hide();
    },
    showClienteSearchWindow:function(){
    	this.clienteSearchWindow.show();
    },
    hideClienteSearchWindow:function(){
    	this.clienteSearchWindow.hide();
    },

    initComponent: function() {
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
                            iconCls: 'new_document',
                            text: ''
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
                            iconCls: 'open_document',
                            text: ''
                        },
                        {
                            xtype: 'button',
                            action: 'save',
                            iconCls: 'save_document',
                            text: ''
                        },
                        {
                            xtype: 'button',
                            action: 'delete',
                            iconCls: 'remove',
                            text: ''
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
                width: 350
            },
            items: [
                {
                    xtype: 'fieldset',
                    flex: 0,
                    style: 'border: 0; padding: 0',
                    title: '',
                    items: [
                        {
                            xtype: 'fieldset',
                            style: 'border: 0; padding: 0',
                            layout: {
                                align: 'stretch',
                                type: 'hbox'
                            },
                            title: '',
                            items: [
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
                            decimalPrecision: 0
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
                        anchor: '100%',
                        width: 350
                    },
                    title: '',
                    items: [
                        {
                            xtype: 'textfield',
                            anchor: '100%',
                            name: 'NombreDestinatario',
                            fieldLabel: 'Destinatario'
                        },
                        {
                            xtype: 'textfield',
                            anchor: '100%',
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
                            title: '',
                            items: [
                                {
                                    xtype: 'textfield',
                                    anchor: '0',
                                    name: 'TelefonoDestinatario',
                                    fieldLabel: 'Tel-Fax'
                                },
                                {
                                    xtype: 'textfield',
                                    anchor: '',
                                    flex: 1,
                                    style: 'marginLeft:4px',
                                    name: 'FaxDestinatario',
                                    hideLabel: true
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            anchor: '100%',
                            name: 'CelularDestinatario',
                            fieldLabel: 'Celular'
                        },
                        {
                            xtype: 'textfield',
                            anchor: '100%',
                            name: 'MailDestinatario',
                            fieldLabel: 'Mail',
                            vtype: 'email'
                        },
                        {
                            name:'IdCiudadDestinatario',
							xtype: 'ciudadcombo',
							fieldLabel: 'Ciudad',
							allowBlank: false,
                            anchor: '100%'

                        },
                        {
                            xtype: 'textfield',
                            anchor: '100%',
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
                    title: '',
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
		text: 'Consecutivo',
		dataIndex: 'Consecutivo',
		flex: 1,
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'IdContacto',
		dataIndex: 'IdContacto',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'FechaCreacion',
		dataIndex: 'FechaCreacion',
		sortable: true,
		renderer: Ext.util.Format.dateRenderer('d.m.Y')
	},
	{
		text: 'FechaActualizacion',
		dataIndex: 'FechaActualizacion',
		sortable: true,
		renderer: Ext.util.Format.dateRenderer('d.m.Y')
	},
	{
		text: 'FechaEnvio',
		dataIndex: 'FechaEnvio',
		sortable: true,
		renderer: Ext.util.Format.dateRenderer('d.m.Y')
	},
	{
		text: 'FechaAceptacion',
		dataIndex: 'FechaAceptacion',
		sortable: true,
		renderer: Ext.util.Format.dateRenderer('d.m.Y')
	},
	{
		text: 'FechaAnulado',
		dataIndex: 'FechaAnulado',
		sortable: true,
		renderer: Ext.util.Format.dateRenderer('d.m.Y')
	},
	{
		text: 'DiasDeVigencia',
		dataIndex: 'DiasDeVigencia',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'VigenteHasta',
		dataIndex: 'VigenteHasta',
		sortable: true,
		renderer: Ext.util.Format.dateRenderer('d.m.Y')
	},
	{
		text: 'IdCreadoPor',
		dataIndex: 'IdCreadoPor',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'NombreCreadoPor',
		dataIndex: 'NombreCreadoPor',
		sortable: true
	},
	{
		text: 'IdEnviadoPor',
		dataIndex: 'IdEnviadoPor',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'NombreEnviadoPor',
		dataIndex: 'NombreEnviadoPor',
		sortable: true
	},
	{
		text: 'IdAceptadoPor',
		dataIndex: 'IdAceptadoPor',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'NombreAceptadoPor',
		dataIndex: 'NombreAceptadoPor',
		sortable: true
	},
	{
		text: 'IdAnuladoPor',
		dataIndex: 'IdAnuladoPor',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'NombreAnuladoPor',
		dataIndex: 'NombreAnuladoPor',
		sortable: true
	},
	{
		text: 'IdFormaPago',
		dataIndex: 'IdFormaPago',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'DescripcionFormaPago',
		dataIndex: 'DescripcionFormaPago',
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
		text: 'NitCliente',
		dataIndex: 'NitCliente',
		sortable: true
	},
	{
		text: 'NombreCliente',
		dataIndex: 'NombreCliente',
		sortable: true
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


Ext.define('App.view.pedido.SearchWindow',{
	extend: 'Ext.Window',
    alias : 'widget.pedidosearchwindow',
	closable: true,
    closeAction: 'hide',
    y:25,
    x:25,
    autoHeight:true,
    width: 400,
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


Ext.define('App.view.pedidoitem.Form', {
    extend: 'Ext.form.Panel',
    alias : 'widget.pedidoitemform',
    ui:'default-framed',
    frame:false,
    margin: '2 0 0 15px',
    bodyStyle :'padding:0px 0px 0px 0px',
    style: {border: 0, padding: 0},
    autoWidth:true,
    autoHeight:true,
    autoScroll:true,
    fieldDefaults : { msgTarget: 'side', labelWidth: 100,labelAlign: 'right'},
    defaultType:'textfield',
    defaults : { anchor: '100%',labelStyle: 'padding-left:4px;'},
         
    initComponent: function() {
    this.items = [{	
    	xtype: 'toolbar',
    	name: 'padreToolbar',
        colspan:2,
        items: [{
            tooltip:'Agregar Servicio',
            iconCls:'add',
            disabled:true,
            action: 'new'
        },{
         	tooltip:'Guardar',
      		iconCls:'save_document',
        	disabled:true,
        	action:'save'
        },'-',{
            tooltip:'Borrar servicio seleccionado',
            iconCls:'remove',
            disabled:true,
            action: 'delete'
        }]
    },{
		xtype: 'hidden',
		name: 'Id'
	},
	{
		xtype: 'hidden',
		name: 'IdServicio'
	},
	{
		xtype: 'hidden',
		name: 'IdProcedimiento'
	},{
		name: 'NombreServicio',
		fieldLabel: 'NombreServicio'
	},
	{
		name: 'DescripcionProcedimiento',
		fieldLabel: 'Procedimiento'
	},{
		name: 'Descripcion',
		fieldLabel: 'Detalle',
		maxLength: 256,
		enforceMaxLength: true
	},
	{
		name: 'Nota',
		fieldLabel: 'Nota',
		maxLength: 256,
		enforceMaxLength: true
	},{
		xtype: 'numberfield',
		allowDecimals: false,
		name: 'Cantidad',
		fieldLabel: 'Cantidad',
		allowBlank: false
	},
	{
		xtype: 'numberfield',
		name: 'Descuento',
		fieldLabel: 'Descuento',
		allowBlank: false
	},
	{
		xtype: 'numberfield',
		name: 'ValorUnitario',
		fieldLabel: 'ValorUnitario',
		allowBlank: false
	},
	{
		xtype: 'numberfield',
		name: 'PorcentajeIva',
		fieldLabel: 'PorcentajeIva',
		allowBlank: false
	},
	{
		xtype: 'numberfield',
		name: 'ValorBase',
		fieldLabel: 'ValorBase',
		allowBlank: false
	},
	{
		xtype: 'numberfield',
		name: 'ValorIva',
		fieldLabel: 'ValorIva',
		allowBlank: false
	}];
  
    this.callParent(arguments);
    }
});


Ext.define('App.view.pedidoitem.List',{ 
    extend: 'Ext.grid.Panel',
    alias : 'widget.pedidoitemlist',
    title: 'Items',
    frame:false,
    selType : 'rowmodel',
    autoHeight:true,
    width:530,
    autoScroll:true,
    viewConfig : { 	stripeRows: true   },
    margin: '2 0 0 0',  
    initComponent: function() {	
    this.store ='PedidoItem';     
            this.columns=[
	{
		text: 'IdServicio',
		dataIndex: 'IdServicio',
		flex: 1,
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'IdProcedimiento',
		dataIndex: 'IdProcedimiento',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'Cantidad',
		dataIndex: 'Cantidad',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatInt(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatInt(value)+'</div>';
        	}
        }
	},
	{
		text: 'Descripcion',
		dataIndex: 'Descripcion',
		sortable: true
	},
	{
		text: 'Nota',
		dataIndex: 'Nota',
		sortable: true
	},
	{
		text: 'NombreServicio',
		dataIndex: 'NombreServicio',
		sortable: true
	},
	{
		text: 'DescripcionProcedimiento',
		dataIndex: 'DescripcionProcedimiento',
		sortable: true
	},
	{
		text: 'Descuento',
		dataIndex: 'Descuento',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatNumber(value)+'</div>';
        	}
        }
	},
	{
		text: 'ValorUnitario',
		dataIndex: 'ValorUnitario',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatNumber(value)+'</div>';
        	}
        }
	},
	{
		text: 'PorcentajeIva',
		dataIndex: 'PorcentajeIva',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatNumber(value)+'</div>';
        	}
        }
	},
	{
		text: 'ValorBase',
		dataIndex: 'ValorBase',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatNumber(value)+'</div>';
        	}
        }
	},
	{
		text: 'ValorIva',
		dataIndex: 'ValorIva',
		sortable: true,
		renderer: function(value, metadata, record, store){
           	if(value>=0){
            	return '<div class="x-cell-positive">'+Aicl.Util.formatNumber(value)+'</div>';
        	}else{
            	return '<div class="x-cell-negative">'+Aicl.Util.formatNumber(value)+'</div>';
        	}
        }
	}
];

             
    this.callParent(arguments);
    }
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
     		tooltip:'Seleccionar infante',
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
    queryParam :'Id',
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
                            xtype: 'itemlist'
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
                    title: '',
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
                    name: 'Descuento',
                    fieldLabel: 'Descuento',
                    decimalPrecision: 6
                },
                {
                    xtype: 'numberfield',
                    anchor: '50%',
                    name: 'DiasEntrega',
                    fieldLabel: 'Dias Entrega',
                    allowDecimals: false,
                    decimalPrecision: 0
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

Ext.define('App.view.item.List', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.itemlist',

    height: 121,
    style: 'border: 0; padding: 0',
    ui: 'default-framed',
    title: '',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'string',
                    text: 'String'
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'number',
                    text: 'Number'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'date',
                    text: 'Date'
                },
                {
                    xtype: 'booleancolumn',
                    width: 114,
                    dataIndex: 'bool',
                    text: 'Boolean'
                }
            ],
            viewConfig: {

            }
        });

        me.callParent(arguments);
    }

});
Ext.define('App.view.itemesumen.Form', {
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
                    name: 'ValorBase',
                    readOnly: true,
                    fieldLabel: 'Costo Unitario',
                    labelAlign: 'right'
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
                    name: 'Iva',
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