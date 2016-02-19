(function (moment) {
    "use strict";

    angular.module('controlpanel').controller('controlpanelcontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze', 
    function controller($scope, $http, $timeout, breezeservice, breeze) {
        $scope.isLoading = true;

        var query = breeze.EntityQuery.from('conflict/search');//.orderByDesc('Id');
        var promise = breezeservice.executeQuery(query).then(function (data) {
            $scope.conflicts = data.results;
            $scope.isLoading = false;
        });
    }]);
})(moment);