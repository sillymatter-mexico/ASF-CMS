$(document).ready(function() {
    // $(document).ajaxStart(function() { $.blockUI(); });
    //    $(document).ajaxComplete(function() { $("#msgAjaxLoading").hide(); });
$(document).ajaxStart(function() {
document.getElementById("msgAjaxLoading").style.display = "block";
//document.getElementById("msgAjaxLoading").style.display = "block";
//alert(1); 
});
$(document).ajaxStop(function() {
//alert(2);
//document.getElementById("msgAjaxLoading").style.display = "block";

$("#msgAjaxLoading").hide();  
});

});

 function esconde(id){
     var itm = document.getElementById(id);
     id = id+'_2';
     var nxt = document.getElementById(id);
     if (itm.style) {
         if (itm.style.display == "none") {
             itm.style.display = "";
             nxt.style.display = "none";
         }
     }
     else {
         itm.style.display = "none";
         nxt.style.display = "";
     }
 }

 function escondeSwap(id) {
    var itm = document.getElementById(id);
    if(itm != null && itm != undefined) {
        itm.style.display = itm.style.display == "none" ? "" : "none";    
    }
}

function showhide(id)
	{
		var itm = null;
		if (document.getElementById) {
			itm = document.getElementById(id);
		} else if (document.all){
			itm = document.all[id];
		} else if (document.layers){
			itm = document.layers[id];
		}
		if (itm.style) {
			if (itm.style.display == "none") { itm.style.display = ""; }
			else { itm.style.display = "none"; }
		}
}


function validateCharset(text) {
    var res = {
        hasDoubles: false,
        details: []
    };

    for (var i = 0, r = 0, c = 0; i < text.length; i++) {
        var code = text.charCodeAt(i);
        if (code > 255) {
            if (!res.hasDoubles)
                res.hasDoubles = true;
            res.details.push({
                char: text[i],
                code: code,
                row: r,
                col: c
            });
        }

        if (text[i] == '\n') {
            r++;
            c = 0;
        }
        c++;
    }

    return res;
}