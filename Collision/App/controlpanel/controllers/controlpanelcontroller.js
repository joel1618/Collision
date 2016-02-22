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

    //http://developer.playcanvas.com/en/tutorials/beginner/manipulating-entities/
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

        // A capsule...
        var capsuleTemplate = new pc.Entity();
        capsuleTemplate.addComponent("model", {
            type: "capsule",
            castShadows: true,
            height:200
        });

        capsuleTemplate.addComponent("rigidbody", {
            type: "dynamic",
            mass: 50,
            restitution: 0.5,
            height: 200
        });

        capsuleTemplate.addComponent("collision", {
            type: "capsule",
            radius: 0.5,
            height: 200
        });


        // make the capsule blue
        var blue = createMaterial(new pc.Color(0, 0, 1));
        capsuleTemplate.model.material = blue;

        capsuleTemplate.setPosition(-5, 0, 5);
        //capsuleTemplate.rigidbody.teleport(-5, 0, 5);


        CreateFloor(app);
        // Add to hierarchy
        //app.root.addChild(cube);
        app.root.addChild(capsuleTemplate);
        app.root.addChild(camera);
        app.root.addChild(light);

        // Set up initial positions and orientations
        camera.setPosition(0, 3, 20);
        light.setEulerAngles(45, 0, 0);

        // Register an update event
        app.on("update", function (deltaTime) {
            //cube.rotate(10 * deltaTime, 20 * deltaTime, 30 * deltaTime);
        });

        window.addEventListener('resize', function () {
            app.resizeCanvas();
            //app.resizeCanvas(500, 500);
        });
    }

    function CreateFloor(app)
    {
        var floor = new pc.Entity();
        floor.addComponent("model", {
            type: "box"
        });

        // make the floor white
        var white = createMaterial(new pc.Color(1, 1, 1));
        var red = createMaterial(new pc.Color(1, 0, 0));
        var green = createMaterial(new pc.Color(0, 1, 0));

        floor.model.material = green;

        // scale it
        floor.setLocalScale(10, .1, 10);

        // add a rigidbody component so that other objects collide with it
        floor.addComponent("rigidbody", {
            type: "static",
            restitution: 0.1
        });

        // add a collision component
        floor.addComponent("collision", {
            type: "box",
            halfExtents: new pc.Vec3(5, 0.1, 5)
        });

        // add the floor to the hierarchy
        app.root.addChild(floor);
    }

    function createMaterial(color) {
        var material = new pc.PhongMaterial();
        material.diffuse = color;
        // we need to call material.update when we change its properties
        material.update()
        return material;
    }
})(moment);