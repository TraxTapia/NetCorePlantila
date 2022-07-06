var columnasGrupo = [];
$(document).ready(function () {
    if (confirmarEnvio) {
        $("#ConfirmarEnvio").modal("show");
    }
    if (errorApp !== "") {
        swal({ title: "Error en la aplicación", text: errorApp, type: "error" });
    }
    if (envioArchivos !== "") {
        swal({ title: "Envío de Documentación", text: "Archivos enviados correctamente.", type: "success" });
    }
});

function habilitarCarga() {
    if ($("#OT").val() !== "" && $("#fileLayout").val()!=="") {
        $("#btnCargaLayout").removeAttr("disabled");
    } else {
        $("#btnCargaLayout").attr("disabled","disabled");
    }
}

function SeleccionarColumna(ob,columna) {
    var seleccionado = $(ob).prop('checked');
    var pos = $.inArray(columna, columnasGrupo);
    if (pos < 0 && seleccionado) {
        columnasGrupo.push(columna);
    }else if(pos >= 0 && !seleccionado) {
        columnasGrupo.splice(pos,1);
    }
}

function agregarAgrupaciones() {
    $("#divAgrupacion").html("");
    var sid = "";
    for (var i = 0; i < columnasGrupo.length; i++) {
        sid = "agrupadores[" + i + "]";
        var nagrupador = $("<input type='hidden' id='" + sid + "' name='" + sid + "' value='" + columnasGrupo[i] + "' />");
        $("#divAgrupacion").append($(nagrupador));
    }
}

function SeleccionarFilas(ob) {
    var marcar = $(ob).prop('checked');
    $(".selfila").prop("checked", marcar);
}


function Pendientes() {
    blockPartial('#boxBusquedaEnvios');
    sendAjaxGetCore(null, "../Home/PendientesEnvio", "html", mostrarPendientes, errorGeneral, null, false);
 }

function SelectPendiente(Id, OT) {
    blockPartial('#twDetalleEnvio');
    params = { "Id": Id, "IdOT": OT };
    sendAjaxPostCore(params, "../Home/SeleccionarEnvio", "html", mostrarEnvio, errorGeneral, null, false);
}

function DetalleEnvio(Id,OT) {
    params = { "Id": Id };
    blockPartial('#twDetalleEnvio');
    sendAjaxPostCore(params, "../Home/DetalleEnvio", "html", mostrarDetalle, errorGeneral, null, false);
}

function ModalPendientes() {
    $("#divModalPendientes").modal('show');
    $("#btnConsultarPendientes").click();
}

function mostrarEnvio(result, sender) {
    unblockPartial('#twDetalleEnvio');
    $("#DivDetalleOT").html(result);
    $("#divModalPendientes").modal('hide');
    $("#btnOTSPendientes").click();
    $("#DivSubirOT").hide();
    $('html,body').animate({ scrollTop: 9999 }, 'slow');
}

function errorGeneral(result) {
    unblockPartial('#boxBusquedaEnvios');
    swal({ title: "Error en la aplicación", text: result, type: "error" });
}

function mostrarPendientes(result, sender) {
    unblockPartial('#boxBusquedaEnvios');
    $("#divEnvios").html(result);
}

function mostrarDetalle(result, sender) {
    $("#DivDetalle").html(result);
    $('html,body').animate({ scrollTop: 9999 }, 'slow');
}

function setParametrosEnvio(opc) {
    $('#tipoEnvio').val(opc);
    $('#tipoEnvioConfirm').val(opc);
}

function ActivarCargaDoctos() {
    $("#btnCargaCondiciones").hide();
    var fileUpload = $('#filecondiciones').get(0).files;
    if (fileUpload.length > 0) {
        $("#btnCargaCondiciones").show();
    }
}

function SendServer() {
    let formData = new FormData();
    var fileUpload = $('#filecondiciones').get(0).files;
    for (var i = 0; i < fileUpload.length; i++) {
        formData.append('condiciones', fileUpload[i]);
    }
    var url = "../Home/UpLoadCondiciones";
    var planSelec = $("#PlanDoc").val();
    formData.append('plandoc', planSelec);
    var sender = $("#btnCargaCondiciones");
    blockPartial('#divFileCondiciones');
    sendAjaxPostCoreForm(formData, url, "html", fileSave, fileSave, sender, true);
}

function fileSave(result, sender) {
    unblockPartial('#divFileCondiciones');
    if (result.success === 1) {
        notification("Documentos cargados.", "Condiciones Generales");
    } else {
        swal({ title: "Error en la aplicación", text: result.mensaje, type: "error" });
    }
}