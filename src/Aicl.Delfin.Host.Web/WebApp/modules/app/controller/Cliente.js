/*
 * File: app/controller/Cliente.js
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

Ext.define('App.controller.Cliente', {
    extend: 'Ext.app.Controller',
    views:  ['cliente.Panel','cliente.Window' ],

    stores: [
        'Cliente',
        'Contacto',
        'Ciudad'
    ],

    refs: [
        {
            ref: 'clienteForm',
            selector: 'form[name=ClienteForm]'
        },
        {
            ref: 'findClienteText',
            selector: 'toolbar[name=MainToolbar] textfield[name=FindCliente]'
        },
        {
            ref: 'contactoForm',
            selector: 'form[name=ContactoForm]'
        },
        {
            ref: 'clienteList',
            selector: 'gridpanel[name=ClienteList]'
        },
        {
            ref: 'contactoList',
            selector: 'gridpanel[name=ContactoList]'
        },
        {
            ref: 'clienteSelectButton',
            selector: 'toolbar[name=FindToolbar] button[action=select]'
        }
    ],

    onContactoListSelectionChange: function(tablepanel, selections, options) {
        if (selections.length){
            this.contactoLoadRecord(selections[0]);
        }
        else{
            this.getContactoForm().getForm().reset();
        }
    },

    onNewClienteClick: function(button, e, options) {
        this.getClienteForm().getForm().reset();
        this.getContactoStore().removeAll();
    },

    onFindClienteClick: function(button, e, options) {
        var searchText= this.getFindClienteText().getValue();
        var nit, nombre;
        nit=parseInt(searchText);
        if(isNaN(nit)){
            nit='';
            nombre=searchText;
        }
		else nit= searchText;



        var request={
            Nit: nit,
            Nombre:nombre,
            format:'json'
        };

        var store=this.getClienteStore();
        store.getProxy().extraParams=request;
        store.loadPage(1);
    },

    onSaveClienteClick: function(button, e, options) {
        var record = this.getClienteForm().getForm().getFieldValues(false);
        this.getClienteStore().getProxy().extraParams={format:'json'};
        this.getClienteStore().save(record);
    },

    onRemoveClienteClick: function(button, e, options) {
        var grid = this.getClienteList();
        var record = grid.getSelectionModel().getSelection()[0];
        this.getClienteStore().remove(record);
    },

    onNewContactoClick: function(button, e, options) {
        this.getContactoList().getSelectionModel().deselectAll();
    },

    onSaveContactoClick: function(button, e, options) {
        var record=this.getContactoForm().getForm().getFieldValues(false);
		record.IdCliente= this.getClienteForm().getForm().findField("Id").getValue();
        this.getContactoStore().getProxy().extraParams={format:'json'};
        this.getContactoStore().save(record);
    },

    onRemoveContactoClick: function(button, e, options) {
        var grid = this.getContactoList();
        var record = grid.getSelectionModel().getSelection()[0];
        this.getContactoStore().remove(record);
    },

    onSelectClienteClick: function(button, e, options) {
        this.selectClienteWindow.hide();
        var record= this.getClienteList().getSelectionModel().getSelection()[0];
        this.clienteLoadContactos(record);
        this.clienteLoadRecord(record);
    },

    onClienteListSelectionChange: function(tablepanel, selections, options) {
        this.getClienteSelectButton().setDisabled(selections.length?false:true);
    },

    init: function(application) {
        this.selectClienteWindow= new App.view.cliente.Window();


        this.control({
            "gridpanel[name=ContactoList]": {
                selectionchange: this.onContactoListSelectionChange
            },
            "toolbar[name=MainToolbar] button[action=new]": {
                click: this.onNewClienteClick
            },
            "toolbar[name=MainToolbar] button[action=find]": {
                click: this.onFindClienteClick
            },
            "toolbar[name=MainToolbar] button[action=save]": {
                click: this.onSaveClienteClick
            },
            "toolbar[name=MainToolbar] button[action=remove]": {
                click: this.onRemoveClienteClick
            },
            "toolbar[name=ContactoToolbar] button[action=new]": {
                click: this.onNewContactoClick
            },
            "toolbar[name=ContactoToolbar] button[action=save]": {
                click: this.onSaveContactoClick
            },
            "toolbar[name=ContactoToolbar] button[action=remove]": {
                click: this.onRemoveContactoClick
            },
            "toolbar[name=FindToolbar] button[action=select]": {
                click: this.onSelectClienteClick
            },
            "gridpanel[name=ClienteList]": {
                selectionchange: this.onClienteListSelectionChange
            }
        });
    },

    onLaunch: function() {
        this.getClienteStore().on('load', function(store , records, success, eOpts){
            if(!success){
                Ext.Msg.alert('Error', 'Error al cargar Clientes. Intente mas tarde');
                return;
            }
            if(records.length===0){
                Aicl.Util.msg('Aviso', 'Sin informacion');
                return;
            }
            if(records.length==1){
                var record = records[0];
                this.clienteLoadContactos(record);
                this.clienteLoadRecord(record);
                return;
            }
            this.selectClienteWindow.show();
        }, this);

        this.getClienteStore().on('write', function(store, operation, eOpts ){
            var record =  operation.getRecords()[0];
            if (operation.action != 'destroy'){
                this.getClienteList().getSelectionModel().select(record,true,true);
                this.clienteLoadRecord(record);
            }       
        }, this);


        this.getContactoStore().on('write', function(store, operation, eOpts ){
            var record =  operation.getRecords()[0];
            if (operation.action != 'destroy'){
                this.getContactoList().getSelectionModel().select(record,true,true);
                this.contactoLoadRecord(record);
            }       
        }, this);
    },

    contactoLoadRecord: function(record) {
    	this.ciudadAddLocal(record,"IdCiudad");
        this.getContactoForm().getForm().loadRecord(record);
    },

    clienteLoadRecord: function(record) {
        this.getClienteForm().getForm().loadRecord(record);
    },

    clienteLoadContactos: function(record) {
        this.getContactoStore().load({params:{IdCliente: record.getId()}});
        this.getContactoList().determineScrollbars();
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