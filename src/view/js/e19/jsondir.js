
//select texto from vDIRECTORIOJSON  order by cc, orden_nomina, orden_puesto;




function myFunction() {

var myObj =   myJSON; 
    document.getElementById("directorio").innerHTML =  ' ';

var busca= document.getElementById("txtbusc").value;
if (busca=='undefined' || busca=='undefined' || busca==' ' || busca=='' )
{
busca='smsdku98kns';
}



busca =  busca.toUpperCase().replace('Á','A').replace('É','E').replace('Í','I').replace('Ó','O').replace('Ú','U');    //'balleNá';

//textb=textb.toUpperCase().replace('Á','A').replace('É','E').replace('Í','I').replace('Ó','O').replace('Ú','U');

var x="";
var text="";
  text += "<table class='tdirectorio'> <tr class=headdir> <th>Nombre</th> <th>Puesto</th> <th>Ext.</th> <th>Correo</th>    </tr>"
    for (x in myObj) {
     document.getElementById("directorio").innerHTML =  x;
    if (myObj[x].nombre!='fin')
      {

var vnombre = myObj[x].nombre.toUpperCase().replace('Á','A').replace('É','E').replace('Í','I').replace('Ó','O').replace('Ú','U');
var vpuesto = myObj[x].puesto.toUpperCase().replace('Á','A').replace('É','E').replace('Í','I').replace('Ó','O').replace('Ú','U');
var vext = myObj[x].extension.toUpperCase().replace('Á','A').replace('É','E').replace('Í','I').replace('Ó','O').replace('Ú','U');

         if( vnombre.search(busca) >= 0 || vpuesto.search(busca) >= 0 || vext.search(busca) >= 0) {
var vvcorreo = '<a href="mailto:' +myObj[x].alias +'@asf.gob.mx">'  + myObj[x].alias + '</a> ';
//alert(vvcorreo);

if (myObj[x].alias =='undefined' || myObj[x].alias =='undefined' || myObj[x].alias==' ' || myObj[x].alias=='' )
{
vvcorreo ='';
}

             text += "<tr><td>" + myObj[x].nombre + "</td><td>" + myObj[x].puesto.replace('&nbsp;','') + "</td> <td>" + myObj[x].extension + "</td> <td>" + vvcorreo + "</td>     </tr>";
    }
      }
    }
    
    
    text += "</table>"    

    document.getElementById("directorio").innerHTML =   text ;


}

