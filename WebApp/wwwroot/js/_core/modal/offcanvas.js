let OffCanvas = function () {
    return {
        show: function (_title, _content, _callbackFn) {
            $("#offCanvasTitle").html(_title);
            $("#divOffCanvasBody").html(_content);

            if (_callbackFn != undefined) {
                _callbackFn();
            }

            const offCanvasControl = document.getElementById('divOffCanvas');
            const bsOffcanvas = new bootstrap.Offcanvas(offCanvasControl);
            bsOffcanvas.show();
        },
        hide: function () {
            const offCanvasControl = document.getElementById('divOffCanvas');
            const bsOffcanvas = new bootstrap.Offcanvas(offCanvasControl);
            bsOffcanvas.hide();
        }
    }
}();