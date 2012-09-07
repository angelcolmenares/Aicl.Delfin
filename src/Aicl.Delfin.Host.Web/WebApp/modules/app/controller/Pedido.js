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
    	{ref: 'nombreClienteText', selector: 'pedidoform textfield[name=NombreCliente]'}
    	    	
    ],

    init: function(application) {
    	    	
        this.control({
        	
        	'pedidolist': { 
                selectionchange: function( sm,  selections,  eOpts){
                	var disableSPB=true;
                	if (selections.length){
    					disableSPB=false;   		
        			}
        			this.getPedidoSelectButton().setDisabled(disableSPB);
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
                	this.getMainPanel().hideSearchWindow();
                	var record= this.getPedidoList().getSelectionModel().getSelection()[0];
                	this.pedidoLoadRecord(record);
                }
            },
            
            'toolbar[name=mainToolbar] button[action=new]':{
            	click:function(button, event, options){
                	this.getPedidoForm().getForm().reset();
                				
                }
            },
            
            'toolbar[name=mainToolbar] button[action=save]':{
            	click: function(button, event, options){
            		var record = this.getPedidoForm().getForm().getFieldValues(true);
            		this.getPedidoStore().getProxy().extraParams={format:'json'};
            		this.getPedidotore().save(record);
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
    	this.getPedidoStore().on('load', function(store , records, success, eOpts){
    		if(!success){
    			Ext.Msg.alert('Error', eOpts);
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
    		
    		this.getMainPanel().showSearchWindow();
            
    	}, this);
    	
    	this.getPedidoStore().on('write', function(store, operation, eOpts ){
    		var record =  operation.getRecords()[0];
            if (operation.action == 'create') {
            	this.pedidoLoadRecord(record);
            }            
    	}, this);
    	
    	
				
    },
    
    pedidoLoadRecord:function(record){
    
    	    	                    	
        this.getPedidoForm().getForm().loadRecord(record);
      
    },
    
    terceroAddLocal:function(record, id){
    	var rt= this.getRemoteTerceroStore();
		if(!rt.getById(record.get(id))){
			rt.addLocal({
				Id:record.get(id),
				Nombre:record.get('NombreTercero'),
				Documento:record.get('DocumentoTercero'),
				DigitoVerificacion:record.get('DVTercero'),
				Telefono:record.get('TelefonoTercero'),
				Celular:record.get('CelularTercero'),
				Mail:record.get('MailTercero')
			})
		};
    }
    
 	
});