angular.module("umbraco").controller("RapidUmbracoConverterDashboardController", function ($scope, userService, $http, umbRequestHelper) {
    var vm = this;


    var user = userService.getCurrentUser().then(function (user) {
        console.log(user);
        vm.UserName = user.name;
    });

    vm.BeginConvert = function () {


        return umbRequestHelper.resourcePromise(
            $http.get("backoffice/RapidUmbracoConverter/ConversionController/BeginConvert"),
            "Failed to retrieve all Person data");
    }




});