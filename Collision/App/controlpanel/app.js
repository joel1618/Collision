var controlpanel = angular.module('controlpanel', ['ngRoute', 'ui.grid', 'ui.bootstrap', 'breeze.angular', 'services', 'ngMap']);

controlpanel.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {           
            templateUrl: '../App/controlpanel/views/radar.html'
        })
        .when('/controlpanel', {
            templateUrl: '../App/controlpanel/views/controlpanel.html'
        })
        .otherwise({
            redirectTo: '/'
        });
}])