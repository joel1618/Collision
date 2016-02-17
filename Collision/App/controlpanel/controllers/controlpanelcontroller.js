(function (moment) {
    "use strict";

    var app = angular.module('controlpanel');
    app.controller('controlpanelcontroller', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
        $scope.isLoading = true;
        
    }]);
})(moment);