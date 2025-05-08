let CrudModal = function () {
    return {
        show: function (_title, _content, _callbackFn, _size) {
            $("#divCrudModalSize").removeClass("modal-sm");
            $("#divCrudModalSize").removeClass("modal-lg");
            $("#divCrudModalSize").removeClass("modal-xl");
            if (_size != undefined) {
                $("#divCrudModalSize").addClass(_size);
            }

            $("#h3CrudModalTitle").html(_title);
            $("#divCrudModalBody").html(_content);

            if (_callbackFn != undefined) {
                _callbackFn();
            }
            $("#divCrudModal").modal("show");
        },
        hide: function () {
            $("#divCrudModal").modal("hide");
        }
    }
}();