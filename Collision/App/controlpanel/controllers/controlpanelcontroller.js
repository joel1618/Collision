(function (moment) {
    "use strict";

    var capsule;
    var camera;
    angular.module('controlpanel').controller('controlpanelcontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze',
    function controller($scope, $http, $timeout, breezeservice, breeze) {
        $scope.isLoading = true;
        $scope.camera = {};
        $scope.capsule = {};
        $scope.camera.xposition = 0; $scope.camera.yposition = 0; $scope.camera.zposition = 10;
        $scope.camera.xrotation = 0; $scope.camera.yrotation = 0; $scope.camera.zrotation = 0;

        $scope.capsule.xposition = 0; $scope.capsule.yposition = 0; $scope.capsule.zposition = 0;
        $scope.capsule.xlength = 1; $scope.capsule.ylength = 1; $scope.capsule.zlength = 1;
        $scope.capsule.xrotation = 0; $scope.capsule.yrotation = 0; $scope.capsule.zrotation = 0;

        navigator.geolocation.getCurrentPosition(function (position) {
            $scope.position = position;
            //alert(position.coords.latitude);
        });

        var query = breeze.EntityQuery.from('conflictbreezeapi/search').orderByDesc('Id');
        var conflictPromise = breezeservice.executeQuery(query).then(function (data) {
            $scope.conflicts = data.results;
            $scope.isLoading = false;
            PlayCanvas($scope);
        });
    
        query = breeze.EntityQuery.from('positionbreezeapi/search').orderByDesc('Id');
        var positionPromise = breezeservice.executeQuery(query).then(function (data) {
            $scope.positions = data.results;
        });

        $scope.change = function () {
            //https://jsfiddle.net/end3r/auvcLoc4/?utm_source=website&utm_medium=embed&utm_campaign=auvcLoc4
            /*
            capsule.rotate(1, 1, 1);
            capsule.setLocalScale(1, 1, 1);
            capsule.setPosition(1, 1, 1);
            */
            camera.setPosition($scope.camera.xposition, $scope.camera.yposition, $scope.camera.zposition);
            camera.setEulerAngles($scope.camera.xrotation, $scope.camera.yrotation, $scope.camera.zrotation);

            capsule.setLocalScale($scope.capsule.xlength, $scope.capsule.ylength, $scope.capsule.zlength);
            capsule.setPosition($scope.capsule.xposition, $scope.capsule.yposition, $scope.capsule.zposition);
            capsule.setEulerAngles($scope.capsule.xrotation, $scope.capsule.yrotation, $scope.capsule.zrotation);
           
        }
    }]);

    //http://developer.playcanvas.com/en/tutorials/beginner/manipulating-entities/
    function PlayCanvas($scope) {
        //INITIALIZE
        var canvas = document.getElementById("application-canvas");
        var app = new pc.Application(canvas, {});
        app.start();

        app.setCanvasFillMode(pc.FILLMODE_NONE, 800, 800);
        app.setCanvasResolution(pc.RESOLUTION_AUTO);

        //GRAVITY
        //app.systems.rigidbody.setGravity(0, -9.8, 0);

        //CAMERA
        camera = new pc.Entity();
        camera.addComponent('camera', {
            clearColor: new pc.Color(0.1, 0.2, 0.3)
        });

        //LIGHT
        var light = new pc.Entity();
        light.addComponent('light');

        //CAPSULE
         capsule = new pc.Entity();
        capsule.addComponent("model", {
            type: "capsule",
            castShadows: true,
        });

        capsule.addComponent("rigidbody", {
            type: "capsule",
            mass: 50,
            restitution: 0.5,
        });

        capsule.addComponent("collision", {
            type: "capsule",
            radius: 0.5,
        });

        //ROTATE
        capsule.setEulerAngles(0, 0, 0);

        //COLOR
        capsule.model.material = createMaterial(new pc.Color(0, 0, 1));

        //POSITION
        capsule.setPosition($scope.capsule.xposition, $scope.capsule.yposition, $scope.capsule.zposition);

        //TELEPORT
        //capsuleTemplate.rigidbody.teleport(-5, 0, 5);

        CreateFloor(app);
        // Add to hierarchy
        app.root.addChild(capsule);
        app.root.addChild(camera);
        app.root.addChild(light);

        // Set up initial positions and orientations
        camera.setPosition($scope.camera.xposition, $scope.camera.yposition, $scope.camera.zposition);
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
        floor.setLocalScale(50, .1, 50);

        // add a rigidbody component so that other objects collide with it
        floor.addComponent("rigidbody", {
            type: "static",
            restitution: 0.1
        });

        // add a collision component
        floor.addComponent("collision", {
            type: "box",
            //halfExtents: new pc.Vec3(5, 0.1, 5)
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