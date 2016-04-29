(function (moment) {
    "use strict";
    var entity = {
        id: null, entity: null
    };
    var entities = [];
    angular.module('controlpanel').controller('radarcontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze', 'radarservice',
    function controller($scope, $http, $timeout, breezeservice, breeze, radarservice) {
        $scope.isLoading = true;
        var entities = [];
        navigator.geolocation.getCurrentPosition(function (position) {
            /*get corners from google maps
            latitude < toprightcorner.latitude && latitude > bottomleftcorner.latitude
            longitude < toprightcorner.longitude && longitude > bottomleftcorner.logitude
            */
            $scope.map = { center: { latitude: position.coords.latitude, longitude: position.coords.longitude }, zoom: 4 };
            $scope.isLoading = false;

            //var bounds = $scope.map.getBounds();
            //var p1 = new breeze.Predicate('Latitude2', '>', position.coords.latitude - .5);
            //var p2 = new breeze.Predicate('Latitude2', '<', position.coords.latitude + .5);
            //var p3 = new breeze.Predicate('Longitude2', '>', position.coords.longitude - .5);
            //var p4 = new breeze.Predicate('Longitude2', '<', position.coords.longitude + .5);
            //var pred = new breeze.Predicate.and([p1, p2, p3, p4]);
            //var positionQuery = breeze.EntityQuery.from('radarbreezeapi/search')/*.where(pred)*/.orderByDesc('Id').skip(0).take(100);
            //return positionPromise = breezeservice.executeQuery(positionQuery).then(function (data) {
            //    entities = data.httpResponse.data;
            //});
        });
    }]);

})(moment);