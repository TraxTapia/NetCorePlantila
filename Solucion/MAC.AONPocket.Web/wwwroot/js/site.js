// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function NotificarError(perror) {
    swal({ title: "Error de la aplicación", text: perror, type: "error" });
}

$(document).ready(function () {
    //startTime();
    var submits = $('.btnSubmit');
    $.each(submits, function (key, value) {
        $(this).click(function () {
            blockPartial('#page-wrapper');
        });
    });
});

function startTime() {
    var today = new Date();
    var hr = today.getHours();
    var min = today.getMinutes();
    var sec = today.getSeconds();
    //Add a zero in front of numbers<10
    min = checkTime(min);
    sec = checkTime(sec);
    document.getElementById("clock").innerHTML = hr + " : " + min + " : " + sec;
    var time = setTimeout(function () { startTime(); }, 500);
}
function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

$.extend(
    {
        redirectPost: function (location, args) {
            var form = '';
            $.each(args, function (key, value) {
                var a = "" + value;
                nvalue = a.split('"').join('\"');
                form += '<input type="hidden" name="' + key + '" value="' + nvalue + '">';
            });
            $('<form action="' + location + '" method="POST">' + form + '</form>').appendTo($(document.body)).submit();
        }
    });

//*****************************************
//[NOTIFICACION][INICIO]
function notification(msg, title) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "100",
        "hideDuration": "500",
        "timeOut": "2500",
        "extendedTimeOut": "500",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "show",
        "hideMethod": "hide"
    };
    toastr.success(msg, title);
}
//[NOTIFICACION][FIN]
//*****************************************

function sendAjaxPost(filters, puri, returntype, sendtype, pfuncexito, pfuncerror, psender) {
    var respuesta;
    $.ajax({
        url: puri,
        type: 'POST',
        cache: false,
        async: true,
        contentType: sendtype,
        dataType: returntype,
        data: filters,
        beforeSend: function () {

        }
    })
        .done(function (result) {
            pfuncexito(result, psender);
        })
        .always(function () {

        })
        .fail(function (xhr) {
            pfuncerror(xhr);
        });
}

function sendAjaxPostCore(filters, purl, pdatatype, pfuncexito, pfuncerror, psender, JsonResult) {
    $.ajax({
        url: purl,
        type: 'POST',
        cache: false,
        async: true,
        dataType: pdatatype,
        data: filters,
        beforeSend: function () {

        }
    })
        .done(function (result) {
            if (JsonResult) {
                pfuncexito(JSON.parse(result), psender);
            } else {
                pfuncexito(result, psender);
            }
        })
        .always(function () {

        })
        .fail(function (xhr) {
            pfuncerror(xhr);
        });
}

function sendAjaxGetCore(filters, purl, pdatatype, pfuncexito, pfuncerror, psender, JsonResult) {
    $.ajax({
        url: purl,
        type: 'GET',
        cache: false,
        async: true,
        dataType: pdatatype,
        data: filters,
        beforeSend: function () {

        }
    })
        .done(function (result) {
            if (JsonResult) {
                pfuncexito(JSON.parse(result), psender);
            } else {
                pfuncexito(result, psender);
            }
        })
        .always(function () {

        })
        .fail(function (xhr) {
            pfuncerror(xhr);
        });
}

function sendAjaxPostCoreForm(formData, purl, pdatatype, pfuncexito, pfuncerror, psender, JsonResult) {
    $.ajax({
        url: purl,
        type: 'POST',
        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        success: function (result) {
            pfuncexito(result, psender);
        },
        error: function (jqXHR) {
            pfuncerror(jqXHR);
        },
        complete: function (jqXHR, status) {
        }
    });
}

function convertJson(stringData) {
    var params = stringData.split("|");
    stringData = params.join('"');
    stringData = "{" + stringData + "}";
    var obj = JSON.parse(stringData);
    return obj;
}

function MensajeriaAppBorrar(sender, pmensaje) {
    var url = urlhome + "/EliminarMensajes";
    var omensaje = convertJson(pmensaje);
    sender = { tipoMensaje: omensaje.id_Tipo_Mensaje };
    var filters = {
        data: omensaje
    };
    sendAjaxPostCore(filters, url, "html", eliminarMensajes, errorMensajes, sender, false);
}

function MensajeriaAppBorrarxId(psender, pmensaje, pcuantos) {
    var url = urlhome + "/EliminarMensaje";
    var omensaje = convertJson(pmensaje);
    var params = { sender: psender, cuantos: pcuantos, tipoMensaje: omensaje.id_Tipo_Mensaje };
    var filters = {
        data: omensaje
    };
    sendAjaxPostCore(filters, url, "html", eliminarMensaje, errorMensajes, params, true);
}

function MensajeriaAppLeerTodo(psender, pmensaje) {
    var url = urlhome + "/LeerMensajes";
    var omensaje = convertJson(pmensaje);
    var params = { sender: psender, tipoMensaje: omensaje.id_Tipo_Mensaje };
    var filters = {
        data: omensaje
    };
    sendAjaxPostCore(filters, url, "html", leerMensajes, errorMensajes, params, false);
}

function MensajeriaAppLeer(sender, pmensaje) {
    var url = urlhome + '/LeerMensaje';
    var omensaje = convertJson(pmensaje);
    var filters = {
        data: omensaje
    };
    sendAjaxPostCore(filters, url, "html", leerMensaje, errorMensajes, sender, true);
}

function errorMensajes(result) {

}

function eliminarMensajes(result, sender) {
    $("#divencabezado").html(result);
}

function eliminarMensaje(result, psender) {
    if (result.model.success) {
        var obj = psender.sender;
        $(obj).parent().parent().remove();
        var lbl = $("#numavisos_" + psender.tipoMensaje);
        var numavisos = psender.cuantos - 1;
        $(lbl).html(numavisos);
    }
}

function leerMensajes(result, sender) {
    $("#divencabezado").html(result);
}

function leerMensaje(result, sender) {
    if (result.model.success) {
        $(sender).css('color', 'silver');
    }
}

function TablaToHTML(ptabla) {
    var tabla = document.getElementById(ptabla);
    var table = "<table>";
    var row;
    var cell;
    var c = 0;
    for (var r = 0; r < tabla.rows.length; r++) {
        row = tabla.rows[r];
        table += "<tr>";
        c = 0;
        for (c = 0; c < row.cells.length; c++) {
            cell = row.cells[c];
            table += "<td>";
            table += $(cell).text();
            table += "</td>";
        }
        table += "</tr>";
    }
    table += "</table>";
    window.open('data:application/vnd.ms-excel,' + table, 'Datos');
}

