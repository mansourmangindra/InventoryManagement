let LiveUsers = function () {
    return {
        initAutoComplete: function () {
            let $this = $("#ddl-livesearch");
            $this.wrap('<div class="position-relative"></div>').select2({
                ajax: {
                    url: function (params) {
                        return "/Select/MsGraphUserSearch?searchString=" + params.term;
                    },
                    dataType: 'json',
                    delay: 250,
                    processResults: function (data) {
                        return {
                            results: $.map(data, function (obj) {
                                return {
                                    id: obj.id,
                                    text: obj.displayName + " (" + obj.userPrincipalName + ")",
                                    email: obj.mail,
                                    displayName: obj.displayName,
                                    userPrincipalName: obj.userPrincipalName,
                                    employeeId: obj.employeeId
                                };
                            })
                        };
                    }
                },
                placeholder: "Name, Email or Employee Id e.g. {Juan Dela Cruz} or {juan.delacruz@email.com}",
                dropdownParent: $this.parent(),
                minimumInputLength: 1,
                templateResult: LiveUsers.formatAutoComplete,
                templateSelection: LiveUsers.formatAutoCompleteSelection
            });
        },
        formatAutoComplete: function (repo) {
            if (repo.loading) {
                return repo.text;
            }
            return $(
                "<div class='row clearfix'>" +
                "<div class='col-2'><img class='w-px-40 h-auto rounded-circle' src='/Image/GetUserPhotoByUpn?upn=" + repo.userPrincipalName + "&imageSize=48x48' /></div>" +
                "<div class='col-10'>" +
                "<div class='fw-bold'>" + repo.displayName + "</div>" + repo.userPrincipalName +
                "</div>" +
                "</div>"
            );
        },
        formatAutoCompleteSelection: function (repo) {
            LiveUsers.populateUserInformation(repo);
            return repo.text;
        },
        populateUserInformation: function (data) {
            if (data != undefined && data.id != '' && data.userPrincipalName != undefined && data.userPrincipalName != '') {
                $("#hdn-msgraphid").val(data.id);
                $("#txt-employeenumber").val(data.employeeId);
                $("#txt-employeename").val(data.displayName);
                $("#txt-emailaddress").val(data.email);
                $("#txt-userprincipalname").val(data.userPrincipalName);
            }
        }
    }
}();

let LiveChats = function () {
    return {
        initAutoComplete: function () {
            let $this = $("#ddl-chatsearch");
            $this.wrap('<div class="position-relative"></div>').select2({
                ajax: {
                    url: function (params) {
                        return "/Select/ChatSearch?searchString=" + params.term;
                    },
                    dataType: 'json',
                    delay: 250,
                    processResults: function (data) {
                        return {
                            results: $.map(data, function (obj) {
                                return {
                                    id: obj.id,
                                    text: obj.topic
                                };
                            })
                        };
                    }
                },
                placeholder: "Case-Sensitive Search",
                dropdownParent: $this.parent(),
                minimumInputLength: 1,
                templateResult: LiveChats.formatAutoComplete,
                templateSelection: LiveChats.formatAutoCompleteSelection
            });
        },
        formatAutoComplete: function (repo) {
            return repo.text;
        },
        formatAutoCompleteSelection: function (repo) {
            LiveChats.populateUserInformation(repo);
            return repo.text;
        },
        populateUserInformation: function (data) {
            if (data != undefined && data.id != '') {
                $("#hdn-chatid").val(data.id);
                $("#txt-chatname").val(data.text);
            }
        }
    }
}();