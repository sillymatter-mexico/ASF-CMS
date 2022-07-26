//select texto from vDIRECTORIOJSON  order by cc, orden_nomina, orden_puesto;
//select distinct substring(cc,1,2) cc, mapa1, mapa2 , mapa1  +  isnull(' - ' + mapa2,'') ss from vDIRECTORIOJSON  order by 1

function limpia() {
//$("#lbArea").val("0");
//$("#txtbusc").val("");
document.getElementById("twitterp").innerHTML = 'a';
$("#twitterp").innerHTML("");


}


function myFunctions() {
}



function replace_content(content, link) 
   { 

   var element_content = content;

   var exp_match = /(\b(https?|):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig; 
   element_content=element_content.replace(exp_match, '<a target="_blank" href="$1">$1</a>'); 
  
   var exp_match1 =/(^|[^\/])(www\.[\S]+(\b|$))/gim; 
   element_content=element_content.replace(exp_match1, '$1<a target="_blank" href="http://$2">$2</a>'); 



//  var exp_match6 =   /@\S+(\r\n|\n|\r|,|.|;|\s)/g; 
  var exp_match6 =   /@\S+(\s|,|.|;|\r\n|\n|\r)/g;    


///   element_content=element_content.replace(exp_match6, '<a target="_blank" href="$&">$&</a>'); 



var links = element_content

//links = links.replace(/\s/g,'  '); 

links = links.replace(/,/g,'  ,'); 
links = links.replace(/;/g,'  ;'); 

links = links.split('.').join('  .');


   element_content=links.replace(exp_match6, function (all, match, p1, p2, p3, offset, string){
             var _id = all.replace(match,"").trim();
                return  '<a target="_blank" href="https://twitter.com/'+_id+'">'+_id+'</a>'+ match+' ' ;  
               }); 


		element_content = element_content.split('  ').join(' ');
		element_content = element_content.split('  ').join(' ');
		element_content = element_content.split(' ,').join(',');
		element_content = element_content.split(' .').join('.');
		element_content = element_content.split(' ;').join(';');


element_content = element_content.split(/\n/).join('<br>');
element_content = element_content.split('<br><br>').join('<br>');
element_content = element_content.split('<br><br>').join('<br>');

   return element_content; /*+ "<br><br><br>"+  content;*/
   } 



function FnTwitter() {
    //var registro = Date.now();

document.getElementById("FichasTwitts").innerHTML='';

    var x = "";
    var x1 = "";
    var text = "";
//    text += "<table class='ttwitter'> <tr class=headdir> <th>Nombre</th> <th>Puesto</th> <th>Área</th><th>Ext.<br>Correo</th>    </tr>"
//$("#Dvtwitter").innerHTML("");
       var myObj = $(myJSON)

        var xxx = [];
        for (var x = 0; x < myObj.length; x++) {
            xxx += x + " " + myObj[x].nombre + " " + myObj[x].puesto;
            if (myObj[x].nombre != 'fin') {
                var vvcorreo = '<a href="mailto:' + myObj[x].alias + '@asf.gob.mx">' + myObj[x].alias + '</a> ';
                if (myObj[x].alias == 'undefined' || myObj[x].alias == 'undefined' || myObj[x].alias == ' ' || myObj[x].alias == '') {
                    vvcorreo = '';
                }


                try {
                    var txtima ='';
                    var txtimag ='';
		    var fotos = myObj[x].extended_entities.media;
 				 for (var xx = 0; xx < fotos.length; xx++) {

/*

var request = new XMLHttpRequest();  
request.open('GET', fotos[xx].media_url, true);
request.onreadystatechange = function(){
    if (request.readyState === 4){
        if (request.status === 404) {  
            alert("Oh no, it does not exist!");
        }  
    }
};
*/

if (fotos[xx].type=='video'){





//console.log(fotos[xx].video_info.variants[0].url);

//txtima +=' <div  class="imgtwit F'+fotos.length+'_'+(xx+1) +'"  style="background-image: url( '+fotos[xx].media_url+');"> <a href="'+fotos[xx].video_info.variants[0].url+'">ver</a>'+
txtima +=' <div  class="imgtwitV V'+fotos.length+'_'+(xx+1) +'"> '+
'<video width="100%" height="100%" controls  autoplay muted> <source src="'+fotos[xx].video_info.variants[0].url+'" type="'+fotos[xx].video_info.variants[0].content_type+'">no soportado</video></div>';
}
else
{

                     			     txtima +=' <div  class="imgtwit F'+fotos.length+'_'+(xx+1) +'"  style="background-image: url( '+fotos[xx].media_url+');"> <img src="'+fotos[xx].media_url+'" > </img> </div>';
}
				    }
                       txtimag ='<div class="fotostwitCont fotos_'+fotos.length+'">' + txtima +'</div>';
                 }

                 catch(err) {
                          txtimag ='';
                       }


             var _texto = myObj[x].full_text.substring(0,myObj[x].full_text.lastIndexOf("https")-1);
              var _link = myObj[x].full_text.substring(_texto.length+1,500);


                try {
                    var txtUrl ='';
                    var txtUrls ='';
		    var Urls = myObj[x].entities.urls;
 				 for (var yy = 0; yy < Urls.length; yy++) {
                                               _texto = _texto.replace(Urls[yy].url,'<a target="_blank" class="TwitUrl" href="'+Urls[yy].url+'">'+Urls[yy].display_url+'</a>');}
                     }
                 catch(err) {   }




		try {
		    var Menciones = myObj[x].entities.user_mentions;
 				 for (var ww = 0; ww < Menciones.length; ww++) {
                     			      _texto = _texto.replace('@'+Menciones[ww].screen_name,'<a target="_blank" class="TwitMencion"  href="https://twitter.com/'+Menciones[ww].screen_name+'">@'+Menciones[ww].screen_name+'</a>');
                    		    }
                     }

                 catch(err) {
                       }




		try {
		    var Hashtags = myObj[x].entities.hashtags;
 				 for (var zz = 0; zz < Hashtags.length; zz++) {
                     			      _texto = _texto.replace('#'+Hashtags[zz].text,'<a target="_blank" class="TwitHash"  href="https://twitter.com/hashtag/'+Hashtags[zz].text+'">#'+Hashtags[zz].text+'</a>');
                    		    }
                     }

                 catch(err) {
                       }

var fe = new Date(myObj[x].created_at) ;
//fe.setHours(fe.getHours() - 2);
//if(fe.getTimezoneOffset() > 0){
//    fe.setTime( fe.getTime() + fe.getTimezoneOffset()*60*1000 );
//}

var createdd = fe.toString().split(' ');
var created=fe.toString().split(' ');

//var horas = new Date(2018,01,01,14,0,0) - new Date(2018,01,01,12,0,0);

//"Thu Sep 05 00:15:05 +0000 2019"
//  0   1   2  3     4       5         6 7 8 9 10
//"Wed Sep 04 2019 19:15:05 GMT-0500 (hora de verano central)"

created[0]=createdd[0].replace('Mon','Lun').replace('Tue','Mar').replace('Wed','Mie').replace('Thu','Jue').replace('Fri','Vie').replace('Sat','Sab').replace('Sun','Dom');
created[1]=createdd[2];
created[2]=createdd[1].replace('Jan','Ene').replace('Aug','Ago');
created[3]=createdd[3];
created[4]=createdd[4];
created[5]='hrs.';
created[6]='';
created[7]='';
created[8]='';
created[9]='';



/*
 var createdd = myObj[x].created_at.split(' ');
 var created= myObj[x].created_at.split(' ');

var nvnvn = Date.parse(myObj[x].created_at);


created[0]=createdd[0].replace('Mon','Lun').replace('Tue','Mar').replace('Wed','Mie').replace('Thu','Jue').replace('Fri','Vie').replace('Sat','Sab').replace('Sun','Dom');
created[1]=createdd[2];
created[2]=createdd[1].replace('Jan','Ene').replace('Aug','Ago');
created[3]=createdd[5];
created[4]=createdd[3];
created[5]='';
*/

// var created = myObj[x].created_at.split(' +0000 ').join(' ');
created = created.join(' ');


//                text += ' <div class="fichaTwit">  <p> <a target="_blank" href="' +  _link + '"> Ir </a> </p> <p>' + replace_content(_texto, _link) + '</p>'+txtimag+'</div>';
                text += ' <div class="fichaTwit"> <div class="textotwitCont"> <p class="headerTwit"> <a target="_blank" href="' +  _link + '"> <img class="RSt" src="/view/assets/imagenes/iconos/tw.png">  ' + created + ' </a> </p> <p class="textotwit">' + _texto + '</p></div>'+txtimag+'</div>';
            }
        }

//    text += "</table>";
    // registro = Math.abs (registro  - Date.now() );
    // text = registro + text;

//$("#FichasTwitts").append(text);

    document.getElementById("FichasTwitts").innerHTML = text;

    // alert(registro);
}
