var controlpanel = angular.module('controlpanel', ['ngRoute', 'ui.grid', 'ui.bootstrap', 'breeze.angular', 'services']);

controlpanel.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: '../App/controlpanel/views/controlpanel.html'
        })
        .when('/radar', {
            templateUrl: '../App/controlpanel/views/radar.html'
        })
        .otherwise({
            redirectTo: '/radar'
        });
}])