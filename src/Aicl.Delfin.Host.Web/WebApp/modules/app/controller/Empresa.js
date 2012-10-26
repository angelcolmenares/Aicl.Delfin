/*
 * File: app/controller/Empresa.js
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

Ext.define('App.controller.Empresa', {
    extend: 'Ext.app.Controller',

    views: [
        'empresa.Form'
    ],

    refs: [
        {
            ref: 'empresaForm',
            selector: 'form[name=EmpresaForm]'
        }
    ],

    onEmpresaSaveButtonClick: function(button, e, options) {
        var form = this.getEmpresaForm().getForm();
        var record= form.getFieldValues(false);

        Aicl.Util.executeRestRequest({
            url : Aicl.Util.getUrlApi()+'/Empresa/'+ (record.Id?'update/'+record.Id:'create'),
            method : record.Id?'put':'post',
            params: record,
            success : function(result){
                if(result.Data.length) {
                    form.setValues(result.Data[0]);
                }
                Aicl.Util.msg('OK', (record.Id?'Empresa Actualizada':'Empresa Creada'));
            },
            failure : function(response, options){
                console.log(arguments);
                Ext.Msg.alert('Error', 'Error al guardar datos de la empresa. Intente mas tarde');
            }

        });
    },

    onLaunch: function() {
        var form = this.getEmpresaForm().getForm();
        Aicl.Util.executeRestRequest({
            url : Aicl.Util.getUrlApi()+'/Empresa/read',
            method : 'get',
            success : function(result){
                if(result.Data.length) {
                    form.setValues(result.Data[0]);
                    Aicl.Util.msg('OK','Datos Cargados' );
                }
                else{
                    Aicl.Util.msg('OK','Se creara la Empresa');
                }
            },
            failure : function(response, options){
                console.log(arguments);
                Ext.Msg.alert('Error', 'Error al cargar datos de la empresa. Intente mas tarde');
            }

        });
    },

    init: function(application) {
        this.control({
            "toolbar[name=EmpresaToolbar] button[action=save]": {
                click: this.onEmpresaSaveButtonClick
            }
        });
    }

});