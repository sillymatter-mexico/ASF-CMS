//select texto from vDIRECTORIOJSON  order by cc, orden_nomina, orden_puesto;
//select distinct substring(cc,1,2) cc, mapa1, mapa2 , mapa1  +  isnull(' - ' + mapa2,'') ss from vDIRECTORIOJSON  order by 1

function limpia() {
$("#lbArea").val("0");
$("#txtbusc").val("");
document.getElementById("directorio").innerHTML = ' ';
}




function hiliter(word, element) {
    var rgxp = new RegExp(word, 'g');
    var repl = '<span class="myClass">' + word + '</span>';
    element.innerHTML = element.innerHTML.replace(rgxp, repl);
}


function highlight_words(word, element) {
    if(word) {
        var textNodes;
        word = word.replace(/\W/g, '');
        var str = word.split(" ");
        $(str).each(function() {
            var term = this;
            var textNodes = $(element).contents().filter(function() { return this.nodeType === 3 });
            textNodes.each(function() {
              var content = $(this).text();
              var regex = new RegExp(term, "gi");
              content = content.replace(regex, '<span class="highlight">' + term + '</span>');
              $(this).replaceWith(content);
            });
        });
    }
}






function myFunction() {
    //var registro = Date.now();
    var x = "";
    var text = "";
    text += "<table class='tdirectorio'> <tr class=headdir> <th>Nombre</th> <th>Puesto</th> <th>Área</th><th>Ext.<br>Correo</th>    </tr>"
    document.getElementById("directorio").innerHTML = ' ';
    var busca = document.getElementById("txtbusc").value.trim();
    var area = document.getElementById("lbArea").value.trim();


    if (busca == 'undefined' || busca == 'undefined' || busca == ' ' || busca == '' || busca.length <3) {
        busca = 'smsdku98kns';
    }
    else {
        busca = busca.toUpperCase().replace('Á', 'A').replace('É', 'E').replace('Í', 'I').replace('Ó', 'O').replace('Ú', 'U');    //'balleNá';

var asas= '8222';
asas = asas.substring(0,area.length);

var sss

        if (busca.indexOf(' ') >= 0 && area == "0"  ) {
            var myObj = $(myJSON).filter(function (i, n) { return RegExp(busca.replace(/\s+/g, '.*'), 'i').test(n.busc); });
            sss= RegExp(busca.replace(/\s+/g, '.*'), 'i');
        }
        else if (busca.indexOf(' ') < 0 && area == "0"  ) {
            var myObj = $(myJSON).filter(function (i, n) { return n.busc.indexOf(busca) >= 0; });
            sss= RegExp(busca, 'i');
        }

        if (busca.indexOf(' ') >= 0 && area != "0"  ) {
            var myObj = $(myJSON).filter(function (i, n) { return RegExp(busca.replace(/\s+/g, '.*'), 'i').test(n.busc) && n.cc.substring(0,area.length) == area ; });
        }
        else if (busca.indexOf(' ') < 0 && area != "0"  ) {
            var myObj = $(myJSON).filter(function (i, n) { return n.busc.indexOf(busca) >= 0 && n.cc.substring(0,area.length) == area ; });
        }

        var xxx = [];
        for (var x = 0; x < myObj.length; x++) {
            xxx += x + " " + myObj[x].nombre + " " + myObj[x].puesto;
            if (myObj[x].nombre != 'fin') {
                var vvcorreo = '<a href="mailto:' + myObj[x].alias + '@asf.gob.mx">' + myObj[x].alias + '</a> ';
                if (myObj[x].alias == 'undefined' || myObj[x].alias == 'undefined' || myObj[x].alias == ' ' || myObj[x].alias == '') {
                    vvcorreo = '';
                }
                text += "<tr><td>" + myObj[x].nombre + "</td><td>" + myObj[x].puesto.replace('&nbsp;', '') + "</td> <td>" + myObj[x].area + "</td> <td>" + myObj[x].extension + "<br>" + vvcorreo + "</td>     </tr>";
            }
        }
   

 }
    text += "</table>";
    // registro = Math.abs (registro  - Date.now() );
    // text = registro + text;
    var re = new RegExp(busca,"g");
    document.getElementById("directorio").innerHTML = text.replace(re,'<span style="color:red;">' + $("#txtbusc").val()+ '</span>' );

//    document.getElementById("directorio").innerHTML = text;


    // alert(registro);

//highlight_words( $("#txtbusc").val(), '.tdirectorio')

}



$( "#lbArea" ).change(function() {
  myFunction();
});

