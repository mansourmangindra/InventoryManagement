let searchPageIndex = 1;
$(document).ready(function () {
    Search.initialize();
});

let Search = function () {
    return {
        initialize: function () {
            App.dateRangePicker("#txtDateRange");

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
            let startDate = $("#txtDateRange").val() != "" ? $("#txtDateRange").data('daterangepicker').startDate.format(AppConstant.queryStringDateTimeFormat) : "";
            let endDate = $("#txtDateRange").val() != "" ? $("#txtDateRange").data('daterangepicker').endDate.format(AppConstant.queryStringDateTimeFormat) : "";
            let searchKeyword = $("#txtKeyword").val();
            let pageSize = $("#ddlPageSize").val();

            let url = `/UserAccount/ListUserAccount?PageNumber=${searchPageIndex}`;
            url += `&StartDate=${startDate}&EndDate=${endDate}`;
            url += `&Keyword=${searchKeyword}`;
            url += `&PageSize=${pageSize}`;
            App.cardBlock("#card-error-search");
            App.ajaxGet(url
                , "html"
                , function (data) {
                    $("#divSearchResult").html(data);

                    App.initializePagingButton(
                        function (pageid) {
                            Search.changePage(pageid);
                        }
                    );

                    App.cardUnBlock("#card-error-search");
                }, function (data) {
                    App.cardUnBlock("#card-error-search");
                }
            );
        },
        clearInputs: function () {
            $("#txtDateRange").val('');
            $("#txtKeyword").val('');
            $("#ddlPageSize").val("10").trigger('change');
        },
        changePage: function (pageId) {
            searchPageIndex = pageId;
            Search.executeSearch();
        }
    }
}();