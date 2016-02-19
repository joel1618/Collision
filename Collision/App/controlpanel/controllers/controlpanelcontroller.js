(function (moment) {
    "use strict";

    var app = angular.module('controlpanel');
    app.controller('controlpanelcontroller', ['$scope', '$http', '$timeout', 'breezeservice', controller]);
    function controller($scope, $http, $timeout, breezeservice) {
        $scope.isLoading = true;

        var query = breeze.EntityQuery.from('conflict/search').orderByDesc('Id');
        var promise = dataserviceprovider.executeQuery(query).then(function (data) {
            $scope.conflicts = data.results;
            $scope.isLoading = false;
        });
    }
})(moment);