$(document).ready(function () {
    App.initializeSelect2();
    App.initializeCollapsibleCard();

    if (window.localStorage.getItem("toastrMsg") !== null) {
        let toastrMsg = JSON.parse(window.localStorage.getItem("toastrMsg"));
        App.alert(toastrMsg.type, toastrMsg.message);
        window.localStorage.clear();
    }
});

const AppConstant = {
    dateFormat: "MM/DD/YYYY",
    jsonDateFormat: "YYYY-MM-DD",
    queryStringDateTimeFormat: "D MMMM YYYY hh:mm:ss A",
    deleteTitle: "Delete ",
    updateTitle: "Update ",
    deleteConfirmationMessage: "Are you sure you want to delete this data? You cannot undo this action.",
    updateConfirmationMessage: "Any modified data will be saved. Would you like to proceed?",
    companyDomains: ["sitel.com", "foundever.com"]
};

let App = function () {
    return {
        initializeToastr: function () {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "500",
                "hideDuration": "1000",
                "timeOut": "12000",
                "extendedTimeOut": "2000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        },
        initializeSelect2: function () {
            let select2 = $('.select2').not('.select2-container--default');
            if (select2.length) {
                select2.each(function () {
                    let $this = $(this);

                    // Initialize only if Select2 is not already initialized to avoid wrapping over and over
                    if (!$this.data('select2')) {
                        $this.wrap('<div class="position-relative"></div>').select2({
                            placeholder: 'Select value',
                            dropdownParent: $this.parent()
                        });
                    }
                });
            }
        },
        initializeCollapsibleCard: function () {
            let collapseElementList = [].slice.call(document.querySelectorAll('.card-collapsible'));
            if (collapseElementList) {
                collapseElementList.map(function (collapseElement) {
                    collapseElement.addEventListener('click', event => {
                        event.preventDefault();
                        // Toggle collapsed class in `.card-header` element
                        collapseElement.closest('.card-header').classList.toggle('collapsed');
                        // Toggle class mdi-chevron-down & mdi-chevron-up
                        Helpers._toggleClass(collapseElement.firstElementChild, 'mdi-chevron-down', 'mdi-chevron-up');
                    });
                });
            }
        },
        dateRangePicker: function (controlSelector) {
            $(controlSelector).daterangepicker({
                timePicker: false,
                autoUpdateInput: false,
                locale: {
                    cancelLabel: 'Clear',
                    format: AppConstant.dateFormat
                },
                ranges: {
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                    'This Year': [moment().startOf('year'), moment().endOf('year')]
                }
            });
            $(controlSelector).on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format(AppConstant.dateFormat) + ' - ' + picker.endDate.format(AppConstant.dateFormat));
            });
        },
        cardBlock: function (controlSelector) {
            $(controlSelector).block({
                message:
                    '<div class="sk-wave mx-auto"><div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div> <div class="sk-rect sk-wave-rect"></div></div>',
                timeout: 0,
                css: {
                    backgroundColor: 'transparent',
                    color: '#fff',
                    border: '0'
                },
                overlayCSS: {
                    opacity: 0.5
                }
            });
        },
        cardUnBlock: function (controlSelector) {
            $(controlSelector).unblock();
        },
        ajaxGet: function (_url, _dataType, _successFn, _errorFn) {
            App.ajaxCall('GET', _url, undefined, _dataType, _successFn, _errorFn);
        },
        ajaxPost: function (_url, _data, _dataType, _successFn, _errorFn) {
            App.ajaxCall('POST', _url, _data, _dataType, _successFn, _errorFn);
        },
        ajaxPut: function (_url, _data, _dataType, _successFn, _errorFn) {
            App.ajaxCall('PUT', _url, _data, _dataType, _successFn, _errorFn);
        },
        ajaxDelete: function (_url, _dataType, _successFn, _errorFn) {
            App.ajaxCall('DELETE', _url, undefined, _dataType, _successFn, _errorFn);
        },
        ajaxCall: function (_methodType, _url, _data, _dataType, _successFn, _errorFn) {
            $.ajax({
                url: _url,
                type: _methodType,
                data: _data,
                dataType: _dataType,
                contentType: "application/json; charset=utf-8",
                headers: _methodType === 'GET' ? {} : { 'Inventory-RV-Token': $('input[name="__RequestVerificationToken"]').val() },
                success: function (data) {
                    App.ajaxSuccess(_successFn, data);
                },
                error: function (jqXHR, textStatus) {
                    App.ajaxError(jqXHR, textStatus, _errorFn);
                }
            });
        },
        ajaxPostFileUpload: function (_url, _data, _dataType, _successFn, _errorFn) {
            App.ajaxCallFileUpload('POST', _url, _data, _dataType, _successFn, _errorFn);
        },
        ajaxCallFileUpload: function (_methodType, _url, _data, _dataType, _successFn, _errorFn) {
            $.ajax({
                url: _url,
                type: _methodType,
                data: _data,
                headers: { 'WFM-Hub-RV-Token': $('input[name="__RequestVerificationToken"]').val() },
                contentType: false,
                processData: false,
                cache: false,
                success: function (data) {
                    App.ajaxSuccess(_successFn, data);
                },
                error: function (jqXHR, textStatus) {
                    App.ajaxError(jqXHR, textStatus, _errorFn);
                }
            });
        },
        alert: function (_type, _message, _urlRedirect) {
            /*_type: value should be any of the following: success, warning, info, error
             _message: alert message to show
             _urlRedirect: used to delay the message after redirect to URL*/
            if (_urlRedirect != undefined) {
                localStorage.setItem("toastrMsg",
                    JSON.stringify({
                        type: _type,
                        message: _message
                    }));

                window.location.replace(_urlRedirect);
            }
            else {
                window.toastr[_type](_message, _type.toUpperCase());
            }
        },
        showValidationMessage: function (validationMessage) {
            if (validationMessage.length > 0) {
                let message = "";

                for (let value of validationMessage) {
                    message += "*" + value + "<br/>";
                }

                App.alert("error", message);
            }
        },
        ajaxError: function (_jqXHR, _textStatus, _errorFn) {

            if (_errorFn != undefined) {
                _errorFn(_jqXHR);
            }

            if (_textStatus == 'parsererror') {
                App.alert("error", 'parsererror: Unexpected error occur');
            }
            else {
                try {
                    let errorMessage = JSON.parse(_jqXHR.responseText);
                    if (errorMessage.message != null) {
                        App.alert("error", errorMessage.message);
                    }
                    else {
                        App.alert("error", errorMessage.traceId);
                    }
                } catch (err) {
                    App.alert("error", _jqXHR.responseText);
                }
            }
        },
        ajaxSuccess: function (_successFn, _data,) {
            if (_successFn != undefined) {
                _successFn(_data);
            }
        },
        requiredTextValidator: function (value, message, control, validationMessages) {
            if (value == undefined || value.trim() == '') {
                validationMessages.push(message);
                control.addClass("is-invalid");
            }
            return validationMessages;
        },
        requiredEmailValidator: function (value, message, control, allowedDomains, validationMessages) {
            validationMessages = App.requiredTextValidator(value, `${message} is required`, control, validationMessages);
            const emails = value.split(';');
            for (const email of emails) {
                const trimmedEmail = email.trim();
                if (trimmedEmail != '') {
                    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                    if (!emailRegex.test(trimmedEmail)) {
                        validationMessages.push(message + ' - Invalid format: ' + trimmedEmail);
                        control.addClass("is-invalid");
                    }
                    else if (allowedDomains != undefined && allowedDomains.length > 0) {
                            const domain = trimmedEmail.split('@')[1];
                            if (!allowedDomains.includes(domain)) {
                                validationMessages.push(message + ' - Invalid domain: ' + trimmedEmail);
                                control.addClass("is-invalid");
                        }
                    }
                }
            }
            return validationMessages;
        },
        requiredText: function (_control, _message, _validationMessages) {
            _control.removeClass("is-invalid");
            let value = _control.val();
            if (value == undefined || value.trim() == '') {
                _validationMessages.push(_message);
                _control.addClass("is-invalid");
            }

            return _validationMessages;
        },
        requiredSingleSelect: function (_control, _allowZero, _message, _validationMessages) {
            _control.parent().removeClass("is-invalid");
            let value = _control.val();
            if (value == undefined || value == '' || (!_allowZero && value == '0')) {
                _validationMessages.push(_message);
                _control.parent().addClass("is-invalid");
            }

            return _validationMessages;
        },
        //This is for Dropdown validator
        requiredSingleSelectValidator: function (value, message, control, allowZero, validationMessages) {
            if (value == undefined || value == '' || (!allowZero && value == '0')) {
                validationMessages.push(message);
                control.parent().addClass("is-invalid");
            }
            return validationMessages;
        },
        requiredMultiSelectValidator: function (value, message, control, validationMessages) {
            if (value.length == 0) {
                validationMessages.push(message);
                control.parent().addClass("is-invalid");
            }
            return validationMessages;
        },
        initializePagingButton: function (pagingFn, btnClassSelector) {
            let btnClassName = ".btnSearchResultPaging";

            if (btnClassSelector != undefined) {
                btnClassName = "." + btnClassSelector;
            }

            $(btnClassName).on("click", function () {
                let pageNumber = $(this).data("pagenumber");
                pagingFn(pageNumber);
            });
        },
        getCheckboxValue: function (controlSelector) {
            return $(controlSelector).prop("checked");
        }
    }
}();