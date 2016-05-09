﻿//TODO: Update markers based on time but not by deleting all the markers first
(function (moment) {
    "use strict";
    angular.module('controlpanel').controller('radarcontroller', ['$scope', '$http', '$timeout', '$interval', 'breezeservice', 'breeze', 'radarservice', 'NgMap',
    function controller($scope, $http, $timeout, $interval, breezeservice, breeze, radarservice, NgMap) {
        $scope.isLoading = true;
        $scope.markers = [];
        $scope.bounds = {
            northeast: {
                latitude: null,
                longitude: null
            },
            southwest: {
                latitude: null,
                longitude: null
            }
        }
        NgMap.getMap().then(function (map) {
            $scope.SetBounds(map);
            map.addListener('idle', function () {
                $scope.SetBounds(map);
                GetFlights($scope, radarservice);
            });
            $timeout(function() {                
                $interval($scope.GetFlights(), 3000);
            }, 1000);
        });

        $scope.GetFlights = function () {
            GetFlights($scope, radarservice);
        }
        $scope.isLoading = false;

        $scope.SetBounds = function (map) {
            $scope.bounds.northeast.latitude = map.getBounds().H.j;
            $scope.bounds.northeast.longitude = map.getBounds().j.H;
            $scope.bounds.southwest.latitude = map.getBounds().H.H;
            $scope.bounds.southwest.longitude = map.getBounds().j.j;
        }

        $scope.GetIcon = function (marker) {
            var customIcon = {
                "scaledSize": [20, 20],
                "url": GetIconUrl(marker)
            };
            return customIcon;
        }
        $scope.ShowMarkerWindow = function (event, marker) {
            $scope.marker = marker;
            $scope.map.showInfoWindow('myInfoWindow', this);
        };    
    }]);

    

    function GetFlights($scope, radarservice) {
        var p1 = new breeze.Predicate('Latitude2', '<', parseFloat($scope.bounds.northeast.latitude.toFixed(6) + "M"));
        var p2 = new breeze.Predicate('Longitude2', '<', parseFloat($scope.bounds.northeast.longitude.toFixed(6) + "M"));
        var p3 = new breeze.Predicate('Latitude2', '>', parseFloat($scope.bounds.southwest.latitude.toFixed(6) + "M"));
        var p4 = new breeze.Predicate('Longitude2', '>', parseFloat($scope.bounds.southwest.longitude.toFixed(6) + "M"));
        var predicate = new breeze.Predicate.and([p1, p2, p3, p4]);
        radarservice.search(predicate, 0, 100, false).then(function (data) {
            ManageMarkers($scope.markers, data);
        });
    }


    //http://stackoverflow.com/questions/14966207/javascript-sync-two-arrays-of-objects-find-delta
    function ManageMarkers(markers, data) {
        var mapMap = MapFromArray(markers, 'Id');
        var mapData = MapFromArray(data, 'Id');
        for (var id in mapMap) {
            if (!mapData.hasOwnProperty(id)) {
                DeleteMarker(markers, mapMap[id]);
            } else if (IsEqual(mapData[id], mapMap[id])) {
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
        markers.push(item);
    }

    function UpdateMarker(currentItem, newItem) {
        currentItem = newItem;
    }

    function DeleteMarker(markers, item) {
        markers.splice(markers.indexOf(item), 1);
    }
    //HELPERS
    //TODO: Have different images for the differnt rotations and supply the location depending on the heading of the flight.
    function GetIconUrl(item) {
        var url = "/Content/images/controlpanel/radar/plane";
        if (!item.IsConflict || item.IsConflict == null) {
            url += "blacksmall";
        }
        else {
            url += "redsmall";
        }

        if (item.Heading2 >= 337.5 && item.Heading2 <= 22.5) { url += ".png"; }
        else if (item.Heading2 >= 22.5 && item.Heading2 <= 67.5) { url += "45.png"; }
        else if (item.Heading2 >= 67.5 && item.Heading2 <= 112.5) { url += "90.png"; }
        else if (item.Heading2 >= 112.5 && item.Heading2 <= 157.5) { url += "135.png"; }
        else if (item.Heading2 >= 157.5 && item.Heading2 <= 202.5) { url += "180.png"; }
        else if (item.Heading2 >= 202.5 && item.Heading2 <= 247.5) { url += "225.png"; }
        else if (item.Heading2 >= 247.5 && item.Heading2 <= 292.5) { url += "270.png"; }
        else if (item.Heading2 >= 292.5 && item.Heading2 <= 337.5) { url += "315.png"; }
        else { url += ".png"; }
        return url;
    }

    function MapFromArray(array, property) {
        var map = {};
        for (var i = 0; i < array.length; i++) {
            if (array[i] !== undefined && array[i] != null)
                map[array[i][property]] = array[i];
        }
        return map;
    }

    function IsEqual(a, b) {
        return a.Id === b.Id;
    }

})(moment);