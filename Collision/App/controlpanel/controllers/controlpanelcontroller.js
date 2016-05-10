(function (moment) {
    "use strict";    
    var entity = {
        id: null, entity: null
    };
    var entities = [];
    angular.module('controlpanel').controller('controlpanelcontroller', ['$scope', '$routeParams', '$http', '$timeout', 'breezeservice', 'breeze', 'aircraftservice', 'positionservice',
    function controller($scope, $routeParams, $http, $timeout, breezeservice, breeze, aircraftservice, positionservice) {

        $timeout(function () {
                var predicate = new breeze.Predicate('Id', '==', parseInt($routeParams.id));
                positionservice.search(predicate, 0, 1, false).then(function (data) {
                    aircraftservice.AddAircraft(data[0]);
                });
            }, 1000);
        
    }]);

})(moment);