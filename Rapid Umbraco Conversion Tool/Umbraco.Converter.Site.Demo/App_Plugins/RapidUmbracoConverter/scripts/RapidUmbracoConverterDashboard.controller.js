angular.module("umbraco").controller("RapidUmbracoConverterDashboardController", function ($scope, userService, $http, umbRequestHelper) {
    var vm = this;

    vm.TemplatePath = "~/react-demo";

    var user = userService.getCurrentUser().then(function (user) {
        console.log(user);
        vm.UserName = user.name;
    });


    vm.BeginConvert = function () {


        if (vm.TemplatePath == '') {
            alert("Please enter a valid template directory");
            $("#templatePath").focus();
        }
        else {
            var response = umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/RapidUmbracoConverter/Converter/BeginConvert?templateDirectory=" + vm.TemplatePath),
                "Failed to retrieve all Person data").then(function (data) {
                    vm.Response = data;
                    alert(data);
                    console.log(data);
                });

            vm.Response = response;
        }


    }




});