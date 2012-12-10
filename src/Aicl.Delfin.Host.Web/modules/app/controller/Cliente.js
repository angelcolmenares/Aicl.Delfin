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

    stores: [
        'Cliente',
        'Contacto',
        'Ciudad',
        'Tarea'
    ],
    views: [
        'cliente.Panel',
        'cliente.Window',
        'reportCP.Window'
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
        },
        {
            ref: 'tareaForm',
            selector: 'form[name=TareaForm]'
        },
        {
            ref: 'tareaList',
            selector: 'gridpanel[name=TareaList]'
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
        this.getTareaStore().removeAll();
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
        if(this.getContactoStore().count()>0){
            Ext.Msg.alert('Error', 'Debe borrar los contactos primero');
            return;
        }

        if(this.getTareaStore().count()>0){
            Ext.Msg.alert('Error', 'Debe borrar las tareas primero');
            return;
        }

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
        this.clienteLoadTareas(record);
        this.clienteLoadRecord(record);
    },

    onClienteListSelectionChange: function(tablepanel, selections, options) {
        this.getClienteSelectButton().setDisabled(selections.length?false:true);
    },

    onNewTareaButtonClick: function(button, e, options) {
        this.getTareaList().getSelectionModel().deselectAll();
    },

    onSaveTareaButtonClick: function(button, e, options) {
        var record=this.getTareaForm().getForm().getFieldValues(false);
        record.IdCliente= this.getClienteForm().getForm().findField("Id").getValue();
        this.getTareaStore().getProxy().extraParams={format:'json'};
        this.getTareaStore().save(record);
    },

    onRemoveTareaButtonClick: function(button, e, options) {
        var grid = this.getTareaList();
        var record = grid.getSelectionModel().getSelection()[0];
        this.getTareaStore().remove(record);
    },

    onTareaListSelectionChange: function(tablepanel, selections, options) {
        if (selections.length){
            this.tareaLoadRecord(selections[0]);
        }
        else{
            this.getTareaForm().getForm().reset();
        }
    },

    onReportButtonClick: function(button, e, options) {
        var id= this.getClienteForm().getForm().findField("Id").getValue();
        var me=this;

        Aicl.Util.executeRestRequest({
            url : Aicl.Util.getUrlApi()+'/ClienteProcedimiento/'+id,
            method : 'get',
            success : function(result) {

                me.reportWindow.show();
                var report = Ext.getDom('report-cp');
                report.innerHTML=result.Html;
            }

        });





    },

    init: function(application) {
        this.selectClienteWindow= new App.view.cliente.Window();
        this.reportWindow = new App.view.reportCP.Window();


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
            },
            "toolbar[name=TareaToolbar] button[action=new]": {
                click: this.onNewTareaButtonClick
            },
            "toolbar[name=TareaToolbar] button[action=save]": {
                click: this.onSaveTareaButtonClick
            },
            "toolbar[name=TareaToolbar] button[action=remove]": {
                click: this.onRemoveTareaButtonClick
            },
            "gridpanel[name=TareaList]": {
                selectionchange: this.onTareaListSelectionChange
            },
            "toolbar[name=MainToolbar] button[action=report]": {
                click: this.onReportButtonClick
            }
        });
    },

    onLaunch: function() {
        var me = this;

        Ext.create('Ext.LoadMask', me.getClienteForm(), {
            msg: "Cargando Clientes...",
            store: me.getClienteStore()
        });

        Ext.create('Ext.LoadMask', me.getContactoList(), {
            msg: "Cargando Contactos...",
            store: me.getContactoStore()
        });

        Ext.create('Ext.LoadMask', me.getTareaList(), {
            msg: "Cargando Tares...",
            store: me.getTareaStore()
        });


        this.getClienteStore().on('load', function(store , records, success, eOpts){
            if(!success){
                Ext.Msg.alert('Error', 'Error al cargar Clientes. Intente mas tarde');
                return;
            }
            if(records.length===0){
                Aicl.Util.msg('Aviso', 'Sin informacion de clientes');
                return;
            }
            if(records.length==1){
                var record = records[0];
                this.clienteLoadContactos(record);
                this.clienteLoadTareas(record);
                this.clienteLoadRecord(record);
                return;
            }
            this.selectClienteWindow.show();
        }, this);


        this.getContactoStore().on('load', function(store , records, success, eOpts){
            if(!success){
                Ext.Msg.alert('Error', 'Error al cargar Contactos. Intente mas tarde');
                return;
            }
            if(records.length===0){
                Aicl.Util.msg('Aviso', 'Sin Contactos');
                return;
            }

            var record = records[0];
            this.getContactoList().getSelectionModel().select(record,true,true);
            this.contactoLoadRecord(record);


        }, this);


        this.getClienteStore().on('write', function(store, operation, eOpts ){
            var record =  operation.getRecords()[0];
            if (operation.action != 'destroy'){
                this.getClienteList().getSelectionModel().select(record,true,true);
                this.clienteLoadRecord(record);
            }
            else{
                this.getClienteForm().getForm().reset();
            }
        }, this);


        this.getContactoStore().on('write', function(store, operation, eOpts ){
            var record =  operation.getRecords()[0];
            if (operation.action != 'destroy'){
                this.getContactoList().getSelectionModel().select(record,true,true);
                this.contactoLoadRecord(record);
            }       
        }, this);

        this.getTareaStore().on('load', function(store , records, success, eOpts){
            if(!success){
                Ext.Msg.alert('Error', 'Error al cargar Tareas. Intente mas tarde');
                return;
            }
            if(records.length===0){
                Aicl.Util.msg('Aviso', 'Sin Tareas');
                return;
            }

            var record = records[0];
            this.getTareaList().getSelectionModel().select(record,true,true);
            this.tareaLoadRecord(record);    

        }, this);

        this.getTareaStore().on('write', function(store, operation, eOpts ){
            var record =  operation.getRecords()[0];
            if (operation.action != 'destroy'){
                this.getTareaList().getSelectionModel().select(record,true,true);
                this.tareaLoadRecord(record);
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
    },

    ciudadAddLocal: function(record,id) {
        var rt= this.getCiudadStore();
        if(!rt.getById(record.get(id))){
            rt.addLocal({
                Id:record.get(id),
                Nombre:record.get('NombreCiudad'),
                Codigo: record.get('CodigoCiudad')
            });
        }

    },

    clienteLoadTareas: function(record) {
        this.getTareaStore().load({params:{IdCliente: record.getId()}});
    },

    tareaLoadRecord: function(record) {
        this.getTareaForm().getForm().loadRecord(record);
    }

});
