Ext.define('App.controller.Pedido',{
	extend: 'Ext.app.Controller',
    stores: ['Pedido', 'PedidoItem','FormaPago','ClienteContacto','Ciudad'],  
    views:  ['pedido.Panel' ],
    refs:[
    	{ref: 'mainPanel', selector: 'pedidopanel' },
    	{ref: 'pedidoNewButton',   selector: 'toolbar[name=mainToolbar] button[action=new]' },
    	{ref: 'pedidoSaveButton', selector: 'toolbar[name=mainToolbar] button[action=save]' },
    	{ref: 'pedidoDeleteButton', selector: 'toolbar[name=mainToolbar] button[action=delete]' },
    	{ref: 'buscarPedidoText', 	 selector: 'pedidopanel textfield[name=buscarPedidoText]'},
    	
    	{ref: 'pedidoList',    	 selector: 'pedidolist' },
    	{ref: 'pedidoForm',    	 selector: 'pedidoform' },
    	{ref: 'pedidoSelectButton', selector: 'pedidolist button[action=select]'},
    	{ref: 'clienteSelectButton', selector: 'clientecontactolist button[action=select]'},
    	{ref: 'nitClienteText', selector: 'pedidoform textfield[name=NitCliente]'},
    	{ref: 'nombreClienteText', selector: 'pedidoform textfield[name=NombreCliente]'},
    	
    	{ref: 'clienteList',    	 selector: 'clientecontactolist' },
    	{ref: 'formaPagoCombo',    	 selector: 'formapagocombo' }
    	    	
    ],

    init: function(application) {
    	    	
        this.control({
        	
        	'pedidolist': { 
                selectionchange: function( sm,  selections,  eOpts){
                	var disableSelectButton=true;
                	if (selections.length){
    					disableSelectButton=false;   		
        			}
        			this.getPedidoSelectButton().setDisabled(disableSelectButton);
                }
            },
            
        	'clientecontactolist':{
        		selectionchange: function( sm,  selections,  eOpts){
                	var disableSelectButton=true;
                	if (selections.length){
    					disableSelectButton=false;   		
        			}
        			this.getClienteSelectButton().setDisabled(disableSelectButton);
                }
        		
        	},
                 	
        	
            'pedidoitemlist': { 
                selectionchange: function( sm,  selections,  eOpts){
                	
                }
            },
            
        	'pedidopanel button[action=buscarPedido]': {
                click: function(button, event, options){
                	var searchText= this.getBuscarPedidoText().getValue();
                	var consecutivo, nombre;
                	
                	consecutivo=parseInt(searchText);
                	if(isNaN(consecutivo)){
                		consecutivo=0,
                		nombre=searchText
                		
                	}	
                	var request={
                		Consecutivo: consecutivo,
                		NombreCliente:nombre,
						format:'json'
                	};
                	
                	var store = this.getPedidoStore();       	  	
                	store.getProxy().extraParams=request;
                	store.loadPage(1);
                }
            },
            
            'pedidolist button[action=select]': {
                click: function(button, event, options){
                	this.getMainPanel().hidePedidoSearchWindow();
                	var record= this.getPedidoList().getSelectionModel().getSelection()[0];
                	this.pedidoLoadRecord(record);
                }
            },
            
            'clientecontactolist button[action=select]': {
                click: function(button, event, options){
                	this.getMainPanel().hideClienteSearchWindow();
                	var record= this.getClienteList().getSelectionModel().getSelection()[0];
                	this.clienteLoadRecord(record);
                }
            },
            
            'toolbar[name=mainToolbar] button[action=new]':{
            	click:function(button, event, options){
                	this.getPedidoForm().getForm().reset();
                	var fp = this.getFormaPagoStore().getAt(0);
                	if(fp) this.getFormaPagoCombo().setValue(fp.getId());
                				
                }
            },
            
            'toolbar[name=mainToolbar] button[action=save]':{
            	click: function(button, event, options){
            		var record = this.getPedidoForm().getForm().getFieldValues(true);
            		this.getPedidoStore().getProxy().extraParams={format:'json'};
            		this.getPedidoStore().save(record);
            	}
            },
            
            'toolbar[name=mainToolbar] button[action=enviar]':{
            	click: function(button, event, options){
            		var id =  this.getPedidoForm().getForm().findField('Id').getValue();
            		var record = this.getPedidoStore().getById(parseInt(id));
            		this.getPedidoStore().enviar(record);
            	}
            },
            
            'toolbar[name=mainToolbar] button[action=aceptar]':{
            	click: function(button, event, options){
            		var id =  this.getPedidoForm().getForm().findField('Id').getValue();
            		var record = this.getPedidoStore().getById(parseInt(id));
            		this.getPedidoStore().aceptar(record);
            	}
            },
            
            'toolbar[name=mainToolbar] button[action=anular]':{
            	click: function(button, event, options){
            		var id =  this.getPedidoForm().getForm().findField('Id').getValue();
            		var record = this.getPedidoStore().getById(parseInt(id));
            		this.getPedidoStore().anular(record);
            	}
            },
            
            'pedidoform button[action=buscarClientePorNit]':{
            	click: function(button, event, options){
            		var searchText= this.getNitClienteText().getValue();
            		var request={
                		Nit: searchText,
						format:'json'
                	};
                	
                	var store = this.getClienteContactoStore();       	  	
                	store.getProxy().extraParams=request;
                	store.loadPage(1);
            		
            		this.getMainPanel().showClienteSearchWindow();
            	}
            },
            'pedidoform button[action=buscarClientePorNombre]':{
            	click: function(button, event, options){
            		var searchText= this.getNombreClienteText().getValue();
            		var request={
                		Nombre: searchText,
						format:'json'
                	};
                	
                	var store = this.getClienteContactoStore();       	  	
                	store.getProxy().extraParams=request;
                	store.loadPage(1);
            		
            		this.getMainPanel().showClienteSearchWindow();
            	}
            }
        })
    },
    
    onLaunch: function(application){
    	    	    	
    	this.getFormaPagoStore().load();
    	
    	this.getFormaPagoStore().on('load', function(store , records, success, eOpts){
    		if(!success){
    			Ext.Msg.alert('Error', 'Error al cargar Formas de Pago. Intente mas tarde');
    			return;
    		}
    		
    		if(records.length>0){
    			var fp = store.getAt(0);
			    if (fp){
			     	this.getFormaPagoCombo().setValue(fp.getId());
			    }
    		}
    	}, this);
    	
    	this.getPedidoStore().on('load', function(store , records, success, eOpts){
    		if(!success){
    			Ext.Msg.alert('Error', 'Error al cargar Pedidos. Intente mas tarde');
    			return;
    		}
    		
    		if(records.length==0){
    			Aicl.Util.msg('Aviso', 'Sin informacion');
    			return;
    		}
    		
    		if(records.length==1){
    			var record = records[0];
    			this.pedidoLoadRecord(record);
    			return;
    		}
    		
    		this.getMainPanel().showPedidoSearchWindow();
            
    	}, this);
    	
    	this.getPedidoStore().on('write', function(store, operation, eOpts ){
    		var record =  operation.getRecords()[0];
            if (operation.action == 'create') {
            	this.pedidoLoadRecord(record);
            }            
    	}, this);
    	
    	this.getPedidoStore().on('enviado', function(store, record, success){
    		console.log('enviado', arguments);
    		if(success){
    			this.pedidoLoadRecord(record);
    		}
    		
    	}, this);    	
    	
    	this.getPedidoStore().on('aceptado', function(store, record, success){
    		console.log('aceptado', arguments);
    		if(success){
    			this.pedidoLoadRecord(record);
    		}
    		
    	}, this);
    	
    	this.getPedidoStore().on('anulado', function(store, record, success){
    		console.log('anulado', arguments);
    		if(success){
    			this.pedidoLoadRecord(record);
    		}
    		
    	}, this);
    	
    },
    
    pedidoLoadRecord:function(record){
    	this.ciudadAddLocal(record,"IdCiudadDestinatario");
        this.getPedidoForm().getForm().loadRecord(record);
    },
    
    clienteLoadRecord:function(record){    	    	                  
    	console.log('clienteLoadRecord', record, this.getPedidoForm().getForm());
    	var pf =  this.getPedidoForm().getForm();
    	this.ciudadAddLocal(record,"IdCiudad");
    	pf.setValues({
    		IdContacto:record.get('IdContacto'),
    		NitCliente:record.get('Nit'),
    		NombreCliente: record.get('Nombre'),
    		NombreContacto: record.get('NombreContacto'),
    		NombreDestinatario: record.get('NombreContacto'),
    		CargoDestinatario: record.get('CargoContacto'),
    		TelefonoDestinatario: record.get('TelefonoContacto'),
    		FaxDestinatario: record.get('FaxContacto'),
    		CelularDestinatario: record.get('CelularContacto'),
    		MailDestinatario: record.get('MailContacto'),
    		IdCiudadDestinatario: record.get('IdCiudad'),
    		DireccionDestinatario: record.get('DireccionContacto')
    	});
    	
    },
    
    ciudadAddLocal:function(record, id){
    	var rt= this.getCiudadStore();
		if(!rt.getById(record.get(id))){
			rt.addLocal({
				Id:record.get(id),
				Nombre:record.get('NombreCiudad'),
				Codigo: record.get('CodigoCiudad')
			})
		};
    }
    
 	
});