// Objetos para componentes VUE
function columnaFiltro(tipo, parametros) {
    return {
        type: tipo,
        param: parametros
    };
}

function accion(nombre, clase, titulo) {
    return {
        accion: nombre,
        clase: clase,
        titulo: titulo
    };
}

function paramInputText(nombre, text,
    placeholder,
    enable,
    alt,
    clase) {
    return {
        'nombre': nombre,
        'text': text,
        'placeholder': placeholder,
        'enable': enable,
        'alt': alt,
        'clase': clase
    };
}

function paramSelect(nombre, model,
    placeholder,
    enable,
    alt,
    clase, value) {
    return {
        'nombre': nombre,
        'model': model,
        'placeholder': placeholder,
        'enable': enable,
        'alt': alt,
        'clase': clase,
        'value': value
    };
}

function modelForm(label, tipo, parametros) {
    return {
        'label': label,
        'type': tipo,
        'param': parametros
    };
}

function obtenerFecha(stringFecha,isIso)
{
    var fecha = "";
    try {
        //"/[(]+[0-9]+[)]/g"
        var exp = new RegExp("[(]+[0-9]+[)]");
        if (exp.test(stringFecha)) {
            var f = exp.exec(stringFecha)[0];
            if (isIso === true) {
                fecha = (new Date(parseInt(f.replace("(", "").replace(")", "")))).toISOString();
                fecha = fecha.substring(0, fecha.indexOf("T"));
            }
            else {
                fecha = (new Date(parseInt(f.replace("(", "").replace(")", "")))).toLocaleDateString();
            }
        }
        if (fecha === "") {
            stringFecha = stringFecha.substring(0, stringFecha.indexOf("T"));
            fecha = stringFecha.substring(8, 10) + "/" + stringFecha.substring(5, 7) + "/" + stringFecha.substring(0, 4);  
        }
    } catch(err){
        fecha = stringFecha;
    }
    finally {
        return fecha;
    }    
}

function obtenerFechaHora(stringFecha, isIso) {
    var fecha = "",hora="";
    try {
        //"/[(]+[0-9]+[)]/g"
        var exp = new RegExp("[(]+[0-9]+[)]");
        if (exp.test(stringFecha)) {
            var f = exp.exec(stringFecha)[0];
            if (isIso === true) {
                fecha = (new Date(parseInt(f.replace("(", "").replace(")", "")))).toISOString();
                hora = fecha.substring(fecha.indexOf("T")+1, fecha.indexOf("T")+ 6);
                fecha = fecha.substring(0, fecha.indexOf("T"));
            }
            else {
                hora = (new Date(parseInt(f.replace("(", "").replace(")", "")))).toLocaleTimeString().substring(0,5);
                fecha = (new Date(parseInt(f.replace("(", "").replace(")", "")))).toLocaleDateString();
            }
        }
    } catch(err){
        fecha = stringFecha;
    }
    finally {
        return fecha + " " + hora;
    }
}

function obtenerIdArray(objeto, key) {
    var resp = new Array();
    var i = 0;
    for (i = 0; i < objeto.length; i++ ) {
        resp.push(objeto[i][key]);
    }
    return resp;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

/*
$(function () {

    //Bus de envetos
    const EventBus = new Vue();

    //Configuración Notify
    $.notify.addStyle('msjmac', {
        html: "<div  ><i class='fa fa-fw fa-info-circle' style='font-size:large; margin-right:10px;' ></i><span data-notify-text /></div>",
        classes: {
            base: {
                "white-space": "nowrap",
                "background-color": "#407d86",
                "color": "#FFFFFF",
                "padding": "15px",
                "border-radius": "0.5em",
                "border": "solid 1px #FFFFFF",
                "font-size": "large"
            },
            info: {
                "color": "white",
                "background-color": "blue"
            },
            success: {
                "color": "white",
                "background-color": "#94d60f ",
                "border": "solid 1px #008d4c"

            }
        }
    });
});
*/
