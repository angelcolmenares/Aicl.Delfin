Ext.define('App.view.pedido.Panel',{ 
    extend: 'Ext.panel.Panel',
    alias : 'widget.pedidopanel',
    frame: false,
    ui:'default-framed',
    style: {border: 0, padding: 0},
    margin: '0 0 0 0',
    width:956,
    layout: 'hbox',
    dockedItems: [{
    	xtype: 'toolbar',
    	style: {border: 0, padding: 1},
    	name:'mainToolbar',
    	dock: 'top',
    	items: [{
            tooltip:'Nuevo', iconCls:'new_document',  disabled:true,
            action: 'new'
        },{
            xtype:'textfield', emptyText:'Consecutivo/NombreCliente',
            width: 300,
            name: 'buscarPedidoText'
        },{
           tooltip:'Buscar por los criterios indicados..', iconCls:'find',
           action: 'buscarPedido'
        }]
	}],
    items:[{
    	xtype:'panel',
    	layout:'vbox',
    	items:[{
        	xtype: 'panel', ui:'default-framed',
        	style: {border: 0, padding: 0},
        	width: 946, height:300, 
        	items:[{xtype:'pedidoform'}]
    	},{
        	xtype: 'panel', ui:'default-framed',
        	style: {border: 0, padding: 0},
        	width: 946, height:300,
        	items:[
        		{xtype:'pedidoitemlist'}
        	]
    	}]
   	}],
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
   	initComponent: function(){
   		this.searchWindow=Ext.create('App.view.pedido.SearchWindow');
   		this.clienteSearchWindow=Ext.create('App.view.clientesearch.Window');
   		this.callParent(arguments);
   	}
});


Ext.define('App.view.pedido.Form', {
    extend: 'Ext.form.Panel',
    alias : 'widget.pedidoform',
    ui:'default-framed',
    style: {border: 0, padding: 0},
    frame:false,
    margin: '2 0 0 5px',
    bodyStyle :'padding:0px 0px 0px 0px',
    autoWidth:true,
    autoHeight: true,
    autoScroll: true,
	fieldDefaults :{ msgTarget: 'side',  labelWidth: 100, labelAlign: 'right'},
	defaultType:'textfield',
	defaults : { anchor: '100%', labelStyle: 'padding-left:4px;' },
    layout: {
            type: 'table',
            columns: 3
    },
    
    initComponent: function() {
        this.items = [{
    		xtype: 'toolbar',
    		name:'pedidoToolbar',
    		colspan:3,
    		dock: 'top',
    		items: [{
           		tooltip:'Guardar',
      			iconCls:'save_document',
        		disabled:true,
        		action:'save'
           	},'-',{
               	tooltip:'Borrar',
               	iconCls:'remove',
               	disabled:true,
               	action: 'delete'
           	}]
	},{
		xtype:'fieldset',
		collapsible: false,
        defaultType: 'textfield',
        defaults: {anchor: '100%', width:340},
        style: {border: 0, padding: 0},
		height: 290,
		margin: '0 0 0 0',
        layout: 'anchor',
		items:[{
			xtype: 'hidden',
			name: 'Id'
		},{
			xtype: 'hidden',
			name: 'IdContacto'
		},
		{
			xtype:'fieldset',
			style: {border: 0, padding: 0},
			items:[{
				layout:'hbox',
				ui:'default-framed',
				style: {border: 0, padding: 0},
				items:[{
					xtype:'textfield',
					width: 310,
					name: 'NitCliente',
					fieldLabel: 'Nit'
				},{
					xtype:'button',
					tooltip:'Buscar por Nit', 
					iconCls:'find',
           			action: 'buscarClientePorNit',
					hideLabel:true,
					style:{marginLeft:"8px"}
				}]
			}]
		},{
			xtype:'fieldset',
			style: {border: 0, padding: 0},
			items:[{
				layout:'hbox',
				ui:'default-framed',
				style: {border: 0, padding: 0},
				items:[{
					xtype:'textfield',
					width: 310,
					name: 'NombreCliente',
					fieldLabel: 'Cliente'
				},{
					xtype:'button',
					tooltip:'Buscar por Nit', 
					iconCls:'find',
           			action: 'buscarClientePorNombre',
					hideLabel:true,
					style:{marginLeft:"8px"}
				}]
			}]
		},{
			xtype: 'formapagocombo',
			fieldLabel: 'FormaPago',
			allowBlank: false
		},{
			xtype: 'numberfield',
			allowDecimals: false,
			name: 'DiasDeVigencia',
			fieldLabel: 'DiasDeVigencia',
			allowBlank: false
		},{
			xtype: 'datefield',
			name: 'VigenteHasta',
			fieldLabel: 'VigenteHasta',
			format: 'd.m.Y',
			readOnly:true
		}]
	},{
		xtype:'fieldset',
		collapsible: false,
        defaultType: 'textfield',
        defaults: {anchor: '100%', width:300},
        style: {border: 0, padding: 0},
        margin: '12 0 0 0',
		height: 290,
        layout: 'anchor',
		items:[{
			name: 'NombreContacto',
			fieldLabel: 'Nombre'
		},{
			name: 'CargoContacto',
			fieldLabel: 'Cargo'
		},{
			name: 'TelefonoContacto',
			fieldLabel: 'Telefono'
		},{
			name: 'FaxContacto',
			fieldLabel: 'Fax'
		},{
		name: 'CelularContacto',
		fieldLabel: 'Celular'
		},{
			name: 'MailContacto',
			fieldLabel: 'Mail',
			vtype: 'email'
		},{
			name: 'DireccionContacto',
			fieldLabel: 'Direccion'
		}]		
	},{
		xtype:'fieldset',
		collapsible: false,
        defaultType: 'textfield',
        defaults: {anchor: '100%', width:220},
		height: 290,
		style: {border: 0, padding: 0},
        layout: 'anchor',
        margin: '12 0 0 0',
		items:[{
			xtype: 'numberfield',
			allowDecimals: false,
			name: 'Consecutivo',
			fieldLabel: 'Consecutivo',
			allowBlank: false
		},{
			xtype: 'datefield',
			name: 'FechaCreacion',
			fieldLabel: 'Creado',
			allowBlank: false,
			format: 'd.m.Y'
		},{
			xtype: 'datefield',
			name: 'FechaActualizacion',
			fieldLabel: 'Actualizado',
			allowBlank: false,
			format: 'd.m.Y'
		},{
			xtype: 'datefield',
			name: 'FechaEnvio',
			fieldLabel: 'Enviado',
			format: 'd.m.Y'
		},{
			xtype: 'datefield',
			name: 'FechaAceptacion',
			fieldLabel: 'Aceptado',
			format: 'd.m.Y'
		},{
			xtype: 'datefield',
			name: 'FechaAnulado',
			fieldLabel: 'Anulado',
			format: 'd.m.Y'
		}]		
	}];
	
    this.callParent(arguments);
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




Ext.define('App.view.pedidoitem.List',{ 
    extend: 'Ext.grid.Panel',
    alias : 'widget.pedidoitemlist',
    title: 'Items',
    frame:false,
    selType : 'rowmodel',
    autoHeight:true,
    width:430,
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