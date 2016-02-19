﻿var app = angular.module('controlpanel', ['ngRoute', 'ui.grid', 'ui.bootstrap', 'breeze.angular', 'services']);

app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: '../App/controlpanel/views/controlpanel.html'
        }).otherwise({
            redirectTo: '/'
        });
}])