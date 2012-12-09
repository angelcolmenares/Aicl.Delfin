Ext.define('App.controller.Login',{
    extend:'Ext.app.Controller',
    init:function () {
        this.control({
          
            'login button[action=login]':{
                click:this.login
            },
            'login textfield':{
                specialkey:this.keyenter
            }
        });
    },
    views:[
        'Login'
    ],
    refs:[
         {ref:'loginWindow', selector:'login'},
         {ref:'loginForm', selector:'form'}
    ],
    
    login:function () {
    	var form = this.getLoginForm();
    	if(!form.getForm().isValid()){
    		Aicl.Util.msg('Empty fields','please write username and password');
    		return;
    	}
        var me=this;
    	var record = form.getValues();
				Aicl.Util.login({
					success : function(result) {
						me.getLoginWindow().hide()
						me.createMenu();
						me.showTareas();
					},
					failure : function(response, options) {
						console.log(arguments);
					},
					params : record
				});
    	
    },
    
    keyenter:function (item, event) {
        if (event.getKey() == event.ENTER) {
            this.login();
        }

    },
    
    showTareas:function(){
    	Ext.getDom('iframe-win').src = Aicl.Util.getUrlModules()+"/tarea";
    },
    
    createMenu: function(){
		var me = this;
		var buttons=[];
		var i=0;
		var grupos = Aicl.Util.getRoles();
		for(var grupo in grupos ){
			if(grupos[grupo].Directory){
				buttons[i]= Ext.create('Ext.Button', {
    				text    : grupos[grupo].Title,
    				directory:grupos[grupo].Directory,
    				scale   : 'small',
    				handler	: function(){
    				Ext.getDom('iframe-win').src = Aicl.Util.getUrlModules()+"/"+this.directory; // 'modules/'+this.directory;
    				}
				});
				i++;
			}
		};
		
		buttons[i]= Ext.create('Ext.Button', {
	    	text    : 'Salir',
	    	scale   : 'small',
	    	handler	: function(){
	    		Aicl.Util.logout({
	    			callback:function(result, success){
	    				vp.destroy();
	    				me.getLoginWindow().show();
	    			}
	    		});
	    	}
		});
		
		var ta = Ext.create("Ext.form.TextArea");
		
		var pubnub = PUBNUB.init({
    		publish_key   : Aicl.Util.getPublishKey(),
    		subscribe_key : Aicl.Util.getSubscribeKey(),
    		ssl           : false,
    		origin        : 'pubsub.pubnub.com'
		});
		
		pubnub.subscribe({
			channel: Aicl.Util.getChannel(),
    	    callback: function(message){
    	    console.log(message);
            ta.setValue( JSON.stringify(message) +'\n'+ ta.getValue()) ;
          }
        });

		var vp=Ext.create('Ext.Viewport', {
        	layout: {
        		type: 'border',
            	padding: 2
        	},
        	defaults: {
            	split: true
        	},
        	items: [{
            	region: 'west',
            	layout:'fit',
            	items:[{
            		layout: {                        
    	    			type: 'vbox',
        				align:'stretch'
    				},
    				defaults:{margins:'2 2 2 2'},
        			items:buttons
            	}],
            	collapsible: true,
            	split: true,
            	width: '18%'
        	},{
            	region: 'center',
            	layout:'fit',
            	items:[{
        			xtype : 'component',
        			id    : 'iframe-win', 
        			autoEl : {
	            		tag : 'iframe',
            			src : 'intro.html'
        			}
            	}]
        	},{
            	region: 'south',
            	layout:'fit',
            	height:30,
            	items:[ta]
        	}]
    	});
	},
	
	onLaunch: function(application){
		if(Aicl.Util.isAuth()){
			this.createMenu();
		}
		else{
    		this.getLoginWindow().show();	
		}
    }    
});

