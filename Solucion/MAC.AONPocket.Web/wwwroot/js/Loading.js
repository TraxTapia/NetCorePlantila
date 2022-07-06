
function blockUI() {
    $.blockUI({
        message: '<div class="sk-spinner sk-spinner-three-bounce"><div class="sk-bounce1"></div><div class="sk-bounce2"></div><div class="sk-bounce3"></div></div>',
        css: {
            border: 'none',
            backgroundColor: 'transparent',
            'z-index': 1051
        },
        overlayCSS: {
            backgroundColor: '#F2F2F2',
            opacity: '0.6',
            'z-index': 1051,
            cursor: 'not-allowed'
        }
    });
}

function unblockUI() {
    $.unblockUI();
}

function blockPartial(idElement) {

    $(idElement).block({
        message: '<div class="sk-spinner sk-spinner-three-bounce"><div class="sk-bounce1"></div><div class="sk-bounce2"></div><div class="sk-bounce3"></div></div>',
        css: {
            border: 'none',
            backgroundColor: 'transparent',
            'z-index': 1051
        },
        overlayCSS: {
            backgroundColor: '#F2F2F2',
            opacity: '0.6',
            'z-index': 1051,
            cursor: 'not-allowed'
        }
    });
}

function unblockPartial(idElement) {
    $(idElement).unblock();
}

