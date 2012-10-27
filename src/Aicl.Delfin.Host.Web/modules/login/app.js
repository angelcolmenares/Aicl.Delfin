Ext.Loader.setConfig({enabled: true});
Ext.Loader.setPath('App', 'modules/app');
    
Ext.application({
name: 'App',
appFolder: 'modules/app',

launch: function(){

	//Aicl.Util.setPhotoDir(location.protocol + '//' + location.host +  '' + location.pathname+ 'photos');
	Aicl.Util.setEmptyImgUrl('../../resources/icons/fam/user.png');
    var loginWin = Ext.create('App.view.Login');
    //loginWin.show();
},
    
controllers: ['Login']
    
});  
