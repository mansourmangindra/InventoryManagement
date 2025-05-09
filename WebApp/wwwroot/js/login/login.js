
$(document).ready(function () {
    Login.initialize();
});

let Login = function () {
    return {

        initialize: function () {
            $("#btnLogin").on("click", function (e) {
                e.preventDefault(); 
                Login.loginUser();
            });
        },

        loginUser: function () {
            let model = Login.createModel();
            let createEndpoint = "/Login/Login";

            App.ajaxPost(createEndpoint
                , JSON.stringify(model)
                , "text"
                , function (data) {
                    let result = JSON.parse(data);
                    if (result.redirectUrl) {
                        window.location.href = result.redirectUrl;
                    }
                    
                }

                , function (err) {
                
                }
            );



        },

        createModel: function () {
            let model = {};

            model.EmailAddress = $("#txtEmailAddress").val();
            model.Password = $("#txtPassword").val();

            return model;
        },
    }
}();