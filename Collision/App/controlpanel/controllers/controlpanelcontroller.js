(function (moment) {
    "use strict";    
    var entity = {
        id: null, entity: null
    };
    var entities = [];
    angular.module('controlpanel').controller('controlpanelcontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze', 'aircraftservice',
    function controller($scope, $http, $timeout, breezeservice, breeze, aircraftservice) {
        $scope.isLoading = true;
        navigator.geolocation.getCurrentPosition(function (position) {
            $scope.position = position;
            aircraftservice.GetPositions($scope.position).then(function (data) {
                $scope.positions = data;
            });
            aircraftservice.GetConflicts().then(function (data) {
                $scope.conflicts = data;
            });
            $scope.isLoading = false;
        });

        $scope.selectPosition = function (position) {
            $scope.selectedposition = position;
            aircraftservice.AddAircraft(position);
        }

        $scope.selectConflict = function (conflict) {

        }
    }]);

})(moment);