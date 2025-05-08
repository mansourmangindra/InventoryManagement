let ConfirmationModal = function () {
    return {
        show: function (_customMessage, _callbackFn) {
            let _html = "Are you sure you want to do this?<br/>You won't be able to revert this transaction!" 
            if (_customMessage != undefined) {
                _html = _customMessage;
            }

            Swal.fire({
                html: _html,
                icon: 'question',
                showClass: {
                    popup: 'animate__animated animate__bounceIn'
                },
                reverseButtons: true,
                confirmButtonText: '<span class="mdi mdi-thumb-up-outline me-1"></span>Confirm',
                cancelButtonText: '<span class="mdi mdi-thumb-down-outline me-1"></span>Cancel',
                customClass: {
                    confirmButton: 'btn btn-sm btn-primary me-3 waves-effect waves-light',
                    cancelButton: 'btn btn-sm btn-outline-secondary waves-effect'
                },
                buttonsStyling: false
            }).then(function (result) {
                if (result.value) {
                    _callbackFn();
                }
            });
        }
    }
}();