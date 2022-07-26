tinyMCEPopup.requireLangPack();

var IconIncludeDialog = {

	init : function() {
		//var f = document.forms[0];

		// Get the selected contents as text and place it in the input
		//f.someval.value = tinyMCEPopup.editor.selection.getContent({format : 'text'});
		//f.somearg.value = tinyMCEPopup.getWindowArg('some_custom_arg');
	},

	insert : function() {
		var assetsPath = "../view/assets/imagenes/iconos";
		var pdfName = "icon_pdf.png";
		var docName = "icon_doc.png";
		var xlsName = "icon_xls.png";
		var pptName = "icon_ppt.png";
		var zipName = "icon_zip.png";
		var extName = "icon_external.png";
		
		var currentIconType = document.forms[0].iconType.value;
		
		if(currentIconType != null && currentIconType.length > 0) {
			var currentFileName = "";
			switch(currentIconType) {
				case "pdf":
					currentFileName = pdfName;
					break;
				case "doc":
					currentFileName = docName;
					break;
				case "xls":
					currentFileName = xlsName;
					break;
				case "ppt":
					currentFileName = pptName;
					break;
				case "zip":
					currentFileName = zipName;
					break;
				case "ext":
					currentFileName = extName;
					break;
			}
			// Insert the contents from the input into the document
			tinyMCEPopup.editor.execCommand('mceInsertContent', false, "<a href=\"" + document.forms[0].mceIconIncludeFilePath.value + "\"><img src=\"" + assetsPath + currentFileName + "\" /></a>");
		}
		tinyMCEPopup.close();
	},
	
	iconClick : function(event) {
		// Selecciona el icono deseado
		//window.alert("Alert=" + event.target.id);
		var iconType  = document.forms[0].iconType;
		if(iconType.value != null && iconType.value.length > 0) {
			var iconTemp = document.getElementById(iconType.value);
			iconTemp.className = "icon";
		}
		event.target.className = "selected";
		iconType.value = event.target.id;
	},
	
	browseClick : function() {
		//window.mcFileManager.browse({
		mcFileManager.browse({
			fields : 'mceIconIncludeFilePath',
//			relative_urls : true,
			rootpath: tinyMCEPopup.getParam("filemanager_rootpath")

			});
//		window.mcFileManager.open('example1','url');
	}
};

tinyMCEPopup.onInit.add(IconIncludeDialog.init, IconIncludeDialog);
