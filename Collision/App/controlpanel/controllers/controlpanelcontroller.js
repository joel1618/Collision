(function (moment) {
    "use strict";

    angular.module('controlpanel').controller('controlpanelcontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze', 
    function controller($scope, $http, $timeout, breezeservice, breeze) {
        $scope.isLoading = true;

        var query = breeze.EntityQuery.from('conflictbreezeapi/search').orderByDesc('Id');
        var promise = breezeservice.executeQuery(query).then(function (data) {
            $scope.conflicts = data.results;
            $scope.isLoading = false;
            PlayCanvas();
        });
    }]);

    function PlayCanvas() {
        // Create a PlayCanvas application
        var canvas = document.getElementById("application-canvas");
        var app = new pc.Application(canvas, {});

        app.start();

        // Fill the available space at full resolution
        app.setCanvasFillMode(pc.FILLMODE_NONE, 800, 800);
        app.setCanvasResolution(pc.RESOLUTION_AUTO);

        // Create box entity
        var cube = new pc.Entity();
        cube.addComponent('model', {
            type: "box"
        });

        // Create camera entity
        var camera = new pc.Entity();
        camera.addComponent('camera', {
            clearColor: new pc.Color(0.1, 0.2, 0.3)
        });

        // Create directional light entity
        var light = new pc.Entity();
        light.addComponent('light');

        // Add to hierarchy
        app.root.addChild(cube);
        app.root.addChild(camera);
        app.root.addChild(light);

        // Set up initial positions and orientations
        camera.setPosition(0, 0, 3);
        light.setEulerAngles(45, 0, 0);

        // Register an update event
        app.on("update", function (deltaTime) {
            cube.rotate(10 * deltaTime, 20 * deltaTime, 30 * deltaTime);
        });

        window.addEventListener('resize', function () {
            app.resizeCanvas();
            //app.resizeCanvas(500, 500);
        }); app.resizeCanvas();
    }
})(moment);