(function (moment) {
    "use strict";    
    var entity = {
        id: null, entity: null
    };
    var entities = [];
    angular.module('controlpanel').controller('controlpanelcontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze', 'aircraftservice', 'positionservice',
    function controller($scope, $http, $timeout, breezeservice, breeze, aircraftservice, positionservice) {
        $scope.isLoading = true;
        var entities = [];
        navigator.geolocation.getCurrentPosition(function (position) {
            $scope.position = position;
            //aircraftservice.GetPositions($scope.position).then(function (data) {
            //    $scope.positions = data;
            //});
            var p1 = new breeze.Predicate('Latitude2', '>', position.coords.latitude - .5);
            var p2 = new breeze.Predicate('Latitude2', '<', position.coords.latitude + .5);
            var p3 = new breeze.Predicate('Longitude2', '>', position.coords.longitude - .5);
            var p4 = new breeze.Predicate('Longitude2', '<', position.coords.longitude + .5);
            var predicate = new breeze.Predicate.and([p1, p2, p3, p4]);
            positionservice.search(null, 0, 10, null).then(function (data) {
                $scope.positions = data;
            })
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