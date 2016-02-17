var app = angular.module('controlpanel', ['ngRoute', 'ui.grid']);

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: '../App/controlpanel/views/controlpanel.html'
        })
}])