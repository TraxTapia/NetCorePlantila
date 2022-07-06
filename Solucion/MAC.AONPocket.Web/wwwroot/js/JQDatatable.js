/*
* Datatable jQuery
/*
* Datatable
* Load data in Datatable
* 
* Options:
* source - Url to make request
* orderBy - datatable fileld order
* scrollY - table size in percent.
* columns - column definition
* language - language settings.
* fnRowCallback - function to process selecions.
*/
(function ($) {
    $.fn.JQDatatable = function (objOpts) {
        // Easiest way to have default options
        var objSelf = this,
            objSettings = $.extend({
                // Defaults - In json format
                "source": "",
                "orderByColumn": 0,
                "orderByDirec": "asc",
                "scrollY": "75vh",
                "columns" : [],
                "language": ["Procesando...", "Mostrar _MENU_ registros", "No se encontro información",
                             "Ningún dato disponible en esta tabla", "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                             "Mostrando registros del 0 al 0 de un total de 0 registros", "(filtrado de un total de _MAX_ registros)",
                             "FILTRA TU BÚSQUEDA:", "Cargando", "Primero", "Último", "Siguiente","Anterior"],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) { },
                "objSelectKey": null,
                "jsonData": []
            }, objOpts);

        for (var i = 0; i < objSettings.columns.length; i++) {
            if (objSettings.columns[i].render === "dateColumn") {
                objSettings.columns[i].render = function (data, type, row) { return dateColumn(data, type, row); };
            }
        }
        
        $(this).DataTable({
            "fixedHeader": true,
            "scrollY": objSettings.scrollY,
            "scrollCollapse": true,
            "sScrollX": "100%",
            "sScrollXInner": "110%",
            "pageLength": 50,
            "destroy": true,
            "processing": true,
            "serverSide": false,
            "searching": true,
            "lengthChange": true,
            "sAjaxSource": objSettings.source,
            "data": objSettings.jsonData,
            "dom": 'lBfrtip',
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                /* Append the grade to the default row class name */
                if (objSettings.fnRowCallback !== null) {
                    var cbresult = dyn_functions[objSettings.fnRowCallback](nRow, aData, iDisplayIndex);
                    return cbresult;
                } else {
                    var objSelect = objSettings.objSelectKey;
                    if (objSelect !== null) {
                        /* Append the grade to the default row class name */
                        var vlink = $('td:eq(' + objSelect.column + ')', nRow).find('a');
                        if (vlink.length === 0) {
                            var k = $('td:eq(' + objSelect.column + ')', nRow).html();
                            $('td:eq(' + objSelect.column + ')', nRow).html('<a href="' + objSelect.refUrl + '=' + k + '">' +
                                k + '</a>');
                            return nRow;
                        }
                    }
                }
            },
            "buttons": [
                {
                    extend: 'collection',
                    text: 'Exportar',
                    buttons: [
                        'copy',
                        'excel',
                        'csv',
                        'pdf',
                        'print'
                    ]
                }
            ]
             , "order": [[objSettings.orderByColumn, objSettings.orderByDirec]]
             , "columns": objSettings.columns
             , language: {
                "sProcessing": objSettings.language[0],
                "sLengthMenu": objSettings.language[1],
                "sZeroRecords": objSettings.language[2],
                "sEmptyTable": objSettings.language[3],
                "sInfo": objSettings.language[4],
                "sInfoEmpty": objSettings.language[5],
                "sInfoFiltered": objSettings.language[6],
                "sInfoPostFix": "",
                "sSearch": objSettings.language[7],
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": objSettings.language[8],
                "oPaginate": {
                    "sFirst": objSettings.language[9],
                    "sLast": objSettings.language[10],
                    "sNext": objSettings.language[11],
                    "sPrevious": objSettings.language[12]
                }
             }
        });
    }//LoadInfo
}(jQuery));

function fnRowCallback(nRow, aData, iDisplayIndex,fun) {
    return fun(nRow, aData, iDisplayIndex);
}

function dateColumn(data, type, row) {
    if (data === null) {
        return "";
    }else if (type === "sort" || type === "type" || data === "" || data.toLowerCase==="null") {
        return data;
    }
    return moment(data).format("DD/MM/YYYY hh:mm:ss");
}

