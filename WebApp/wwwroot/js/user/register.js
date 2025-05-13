
$(document).ready(function () {
    register.initialize();
});

let register = function () {
    return {

        initialize: function () {
            $("#btnSignUp").on('click', function (e) {
                register.signUp();
            });
        },

        signUp: function () {
            let model = register.createModel();
            register.clearValidation();
            let validationMessage = register.validateModel(model);
            let createEndpoint = '/User/Register';
            if (validationMessage.length > 0) {
                App.showValidationMessage(validationMessage);

            }
            else {
                App.ajaxPost(createEndpoint
                    , JSON.stringify(model)
                    , "text"
                    , function (data) {
                        App.alert("success", "Register successfully!");

                    }

                    , function (err) {
                    }
                );
            }
            
        },

        createModel: function () {
            let model = {};

            model.Name = $("#txtFullName").val();
            model.EmailAddress = $("#txtEmailAddress").val();
            model.Password = $("#txtPassword").val();

            return model;
        },

        validateModel: function (model) {

            let validationMessages = [];

            validationMessages = App.requiredText($("#txtFullName"), "-Full Name is required", validationMessages);
            validationMessages = App.requiredText($("#txtEmailAddress"), "-Email Address is required", validationMessages);
            validationMessages = App.requiredText($("#txtPassword"), "-Password is required", validationMessages);
            return validationMessages;
        },


        clearValidation: function () {
            $("#txtFullName").removeClass("is-invalid");
            $("#txtEmailAddress").removeClass("is-invalid");
            $("#txtPassword").removeClass("is-invalid");

        }
    }
}();