var usersettings = angular.module('usersettings', ['ngRoute', 'ui.grid', 'ui.bootstrap', 'breeze.angular', 'services']);

usersettings.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: '../App/usersettings/views/usersettings.html'
        }).otherwise({
            redirectTo: '/'
        });
}])