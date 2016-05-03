//TODO: Update markers based on time but not by deleting all the markers first
(function (moment) {
    "use strict";
    var entity = {
        id: null, entity: null
    };
    var entities = [];
    angular.module('controlpanel').controller('radarcontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze', 'radarservice','uiGmapGoogleMapApi',
    function controller($scope, $http, $timeout, breezeservice, breeze, radarservice, uiGmapGoogleMapApi) {
        $scope.isLoading = true;
        $scope.map = {
            center: { latitude: "", longitude: "" },
            zoom: 14,
            bounds: {}
        };
        var entities = [];
        $scope.markers = [];
        $scope.randomMarkers = [];
        navigator.geolocation.getCurrentPosition(function (position) {
            $scope.map.center = { latitude: position.coords.latitude, longitude: position.coords.longitude };
        });

        $scope.$watch(function () { return $scope.map.bounds }, function(newValue, oldValue) {
            if (newValue !== oldValue && !angular.equals({}, oldValue)) {
                $scope.markers = [];
                $scope.GetFlights($scope.map.bounds);
            }
        }, true);
        
        $scope.isLoading = false;

        $scope.GetFlights = function () {
            var p1 = new breeze.Predicate('Latitude2', '<', parseFloat($scope.map.bounds.northeast.latitude.toFixed(6) + "M"));
            var p2 = new breeze.Predicate('Longitude2', '<', parseFloat($scope.map.bounds.northeast.longitude.toFixed(6) + "M"));
            var p3 = new breeze.Predicate('Latitude2', '>', parseFloat($scope.map.bounds.southwest.latitude.toFixed(6) + "M"));
            var p4 = new breeze.Predicate('Longitude2', '>', parseFloat($scope.map.bounds.southwest.longitude.toFixed(6) + "M"));
            var predicate = new breeze.Predicate.and([p1, p2, p3, p4]);
            radarservice.search(predicate, 0, 100, false).then(function (data) {            
                entities = data;
                ManageMarkers($scope.markers, entities);
            });
        }
    }]);

    //http://stackoverflow.com/questions/14966207/javascript-sync-two-arrays-of-objects-find-delta
    function ManageMarkers(markers, data) {
        var mapMap = MapFromArray(markers, 'Id');
        var mapData = MapFromArray(data, 'Id');
        for (var id in mapMap) {
            if (!mapData.hasOwnProperty(id)) {
                DeleteMarker(markers, mapMap[id]);
            } else if (isEqual(mapData[id], mapMap[id])) {
                UpdateMarker(mapMap[id], mapData[id]);
            }
        }
        for (var id in mapData) {
            if (!mapMap.hasOwnProperty(id)) {
                CreateMarker(markers, mapData[id]);
            }
        }
    }

    function CreateMarker(markers, item) {
        
        var marker = {
            id: item.Id,
            latitude: item.Latitude2,
            longitude: item.Longitude2,
            title: item.CarrierName + ' ' + item.FlightNumber,
            icon: GetIconUrl(item),
            options: {
                labelClass: 'marker_labels',
                labelAnchor: '0 0',
                fit: "true",
                labelContent:
                    '<div ng-show="map.zoom <= 4">' +
                    item.CarrierName + ' ' + item.FlightNumber + "<br />" + "Speed: " + item.Speed2 + ' km/h' + '<br />' + "Altitude: " + item.Altitude2 + ' km' + '<br />' + "Heading: " + item.Heading2 +
                    "</div>"
            }
        };
        markers.push(marker);
    }

    function UpdateMarker(currentItem, newItem) {
        currentItem = newItem;
    }

    function DeleteMarker(markers, item) {
        markers.splice(markers.indexOf(item), 1);
    }
    //HELPERS
    function GetIconUrl(item) {
        if (!item.IsConflict || item.IsConflict == null) {
            return "/Content/images/planeblacksmall.png";
        }
        else {
            return "/Content/images/planeredsmall.png"
        }
    }

    function MapFromArray(array, property) {
        var map = {};
        for (var i = 0; i < array.length; i++) {
            if (array[i] !== undefined && array[i] != null)
                map[array[i][property]] = array[i];
        }
        return map;
    }

    function isEqual(a, b) {
        return a.Id === b.Id;
    }

})(moment);