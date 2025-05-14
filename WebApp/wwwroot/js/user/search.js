let searchPageIndex = 1;
$(document).ready(function () {
    Search.initialize();
});

let Search = function () {
    return {
        initialize: function () {
            $("#btnSearch").on("click", function () {
                searchPageIndex = 1;
                Search.executeSearch();
            });

            $("#btnClearInput").on("click", function () {
                Search.clearInputs();
            });

            Search.executeSearch();
        },
        executeSearch: function () {
            let pageSize = $("#ddlPageSize").val();
            let searchKeyword = $("#txtKeyword").val();

            let url = `/User/_searchresult?PageNumber=${searchPageIndex}`;

            url += `&PageSize=${pageSize}`;
            url += `&Keyword=${searchKeyword}`;
            App.cardBlock("#card-user-search");
            App.ajaxGet(url
                , "html"
                , function (data) {
                    $("#divSearchResult").html(data);

                    App.initializePagingButton(
                        function (pageid) {
                            Search.changePage(pageid);
                        }
                    );

                    $(".btn-user-edit").on("click", function () {
                        let id = $(this).data("id");
                        Crud.edit(id);
                    });

                    $(".btn-user-delete").on("click", function () {
                        let id = $(this).data("id");
                        ConfirmationModal.show(
                            "Are you sure you want to delete this user?<br/>You won't be able to revert this transaction!",
                            function () {
                                Crud.delete(id);
                            }
                        );
                    });

                    App.cardUnBlock("#card-user-search");
                }, function (data) {
                    App.cardUnBlock("#card-user-search");
                }
            );
        },
        clearInputs: function () {
            $("#txtKeyword").val([]).trigger('change'); 
            $("#ddlPageSize").val("10").trigger('change');
        },
        changePage: function (pageId) {
            searchPageIndex = pageId;
            Search.executeSearch();
        }
    }
}();