var controlpanel = angular.module('controlpanel', ['ngRoute', 'ui.grid', 'ui.bootstrap', 'breeze.angular', 'services', 'uiGmapgoogle-maps']);

controlpanel.config(function(uiGmapGoogleMapApiProvider) {
    uiGmapGoogleMapApiProvider.configure({
        //    key: 'your api key',
        v: '3.20', //defaults to latest 3.X anyhow
        libraries: 'weather,geometry,visualization'
    });
})

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