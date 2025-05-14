var searchPageIndex = 1;

$(document).ready(function () {
    UserFilterSearchBar.initialize();
    UserFilterSearchBar.executeSearch();
});

let UserFilterSearchBar = function () {
    let btnSearch = ".search";
    let btnClear = ".clear";
    let tableContainer = "#divTableContainer";
    let searchResultpanel = "#divSearchResultPanel"

    let url = window.location.pathname.split("/");
    let controller = url[1];
    let searchEndPoint = `/${controller}/List`;

    return {
        initialize: function () {
            if (App.controlHasLength(tableContainer)) {
                UserFilterSearchBar.executeSearch();
            }

            String.prototype.format = function () {
                a = this;
                for (k in arguments) {
                    a = a.replace("{" + k + "}", arguments[k])
                }
                return a
            }

            $(btnClear).click(function () {
                searchPageIndex = 1;
                UserFilterSearchBar.clearInputs();
            });

            $(btnSearch).click(function () {
                searchPageIndex = 1;
                UserFilterSearchBar.executeSearch();
            });
        },
        getQueryString: function () {
            let searchKeyword = $("#txtUserFilterKeyword").val();
            let enabledFilter = $("#ddlUserFilterActive").val();
            let pageSizeFilter = $("#ddlUserFilterPageSize").val();
            let roleIds = $("#ddlUserFilterRole").val();
            //let projectIds = $("#ddlUserRoleProject").val();
            let facilityIds = $("#ddlUserRoleFacility").val();

            let queryString = `PageNumber=${searchPageIndex}`;
            queryString += `&SearchKeyword=${searchKeyword}`;
            queryString += `&PageSize=${pageSizeFilter}`;

            $.each(enabledFilter, function (i, d) {
                queryString += `&Active=${d}`;
            });

            $.each(roleIds, function (i, d) {
                queryString += `&RoleIds=${d}`;
            });

            //$.each(projectIds, function (i, d) {
            //    queryString += `&ProjectIds=${d}`;
            //});

            $.each(facilityIds, function (i, d) {
                queryString += `&FacilityIds=${d}`;
            });

            return queryString;
        },
        clearInputs: function () {
            $("#txtUserFilterKeyword").val('');
            $("#ddlUserFilterActive").val([]).trigger('change');
            $("#ddlUserFilterPageSize").val('10').trigger('change');
            $("#ddlUserFilterRole").val([]).trigger('change');
            $("#ddlUserRoleFacility").val([]).trigger('change');
        },
        executeSearch: function () {
            let queryString = UserFilterSearchBar.getQueryString();
            let searchUrl = searchEndPoint + `?${queryString}`;

            App.cardBlock($(tableContainer));
            App.ajaxGet(searchUrl
                , "html"
                , function (data) {
                    App.cardUnBlock($(tableContainer));
                    $(tableContainer).html(data);
                    $('[data-toggle="popover"]').popover();
                    App.initializePagingButton(
                        function (pageid) {
                            UserFilterSearchBar.changePage(pageid);
                        }
                    );
                }
                , function () {
                }
            );
        },
        changePage: function (pageId) {
            searchPageIndex = pageId;
            UserFilterSearchBar.executeSearch();
        }
    }
}();