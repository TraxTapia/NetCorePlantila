LoadInfo = function (ModelDataTable) {
    $('#JQTable').JQDatatable(ModelDataTable);
}//LoadInfo

var dyn_functions = [];
dyn_functions['registroCondiciones'] = function (nRow, aData, iDisplayIndex) {
    $('td:eq(9)', nRow).html('<a href="#">Detalle</a>');
    $('td:eq(10)', nRow).html('<a href="#">EANs relacionados</a>');
    $('td:eq(10)', nRow).css('white-space','nowrap');
    $('td:eq(11)', nRow).html('<a href="#">Especialidades</a>');
};

function gurdarSal() {
    if ($("#frmEditarSal").valid()) {   // test for validity
        var a = 1;
    }
    return false;
}