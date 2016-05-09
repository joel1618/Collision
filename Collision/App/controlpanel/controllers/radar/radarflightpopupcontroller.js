(function () {
    "use strict";
    var entity = {
        id: null, entity: null
    };
    var entities = [];
    angular.module('controlpanel').controller('radarflightpopupcontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze', 'radarservice', 'uiGmapGoogleMapApi',
    function controller($scope, $http, $timeout, breezeservice, breeze, radarservice, uiGmapGoogleMapApi) {
        
        $scope.map = {
            center: {
                latitude: 51.5286416,
                longitude: -0.1015987
            },
            zoom: 12
        };
        $scope.options = { scrollwheel: false };

        $scope.windowOpt = {
            position: {
                lat: 51.5286416,
                lng: -0.1015987
            },
            show: true
        }

        $scope.showWindow = function () {
            $scope.windowOpt.show = true;
        }

        $scope.doIt = function () {
            alert("Action!");
        }

        $scope.list = [
          { id: 1, content: "This is first item" },
          { id: 2, content: "This is second item" },
          { id: 3, content: "This is third item" }
        ]

        $scope.windowParams = {
            list: $scope.list,
            doIt: function () {
                return $scope.doIt()
            }
        }
    }]);
})();