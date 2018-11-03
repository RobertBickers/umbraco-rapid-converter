angular.module("umbraco").controller("RapidUmbracoConverterDashboardController", function ($scope, userService, $http, umbRequestHelper) {
    var vm = this;


    var user = userService.getCurrentUser().then(function (user) {
        console.log(user);
        vm.UserName = user.name;
    });



    vm.BeginConvert = function () {

        var response = umbRequestHelper.resourcePromise(
            $http.get("/umbraco/backoffice/RapidUmbracoConverter/ConverterApi/BeginConvert"),
            "Failed to retrieve all Person data").then(function (data) {
                vm.Response = data;
                alert(data);
                console.log(data);
            });



        return response;
    }




});