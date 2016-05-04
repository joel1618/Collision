(function (moment) {
    "use strict";    
    var entity = {
        id: null, entity: null
    };
    var entities = [];
    angular.module('controlpanel').controller('controlpanelcontroller', ['$scope', '$routeParams', '$http', '$timeout', 'breezeservice', 'breeze', 'aircraftservice', 'positionservice',
    function controller($scope, $routeParams, $http, $timeout, breezeservice, breeze, aircraftservice, positionservice) {
        $scope.isLoading = true;
        debugger;
        positionservice.get($routeParams.id).then(function (data) {
            aircraftservice.AddAircraft(position);
            $scope.isLoading = false;
        });

        $scope.selectConflict = function (conflict) {

        }
    }]);

})(moment);