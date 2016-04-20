(function () {
    "use strict";
    angular.module('usersettings').controller('usersettingscontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze',
    function controller($scope, $http, $timeout, breezeservice, breeze) {
        $scope.isLoading = true;
        }
    }]);

})();