(function (moment) {
    "use strict";

    var capsule;
    var camera;
    var midpoint = {
        position: {
            x: 0, y: 0, z: 10
        },
        rotation: {
            x: 0, y: 0, z: 0
        }
    };
    angular.module('controlpanel').controller('controlpanelcontroller', ['$scope', '$http', '$timeout', 'breezeservice', 'breeze',
    function controller($scope, $http, $timeout, breezeservice, breeze) {
        $scope.isLoading = true;
        $scope.camera = {
            position: {
                x: 0, y: 0, z: 10
            },
            rotation: {
                x: 0, y: 0, z: 0 
            }
        };
        $scope.capsule = {
            position: {
                x: 0, y: 0, z: 0
            },
            rotation: {
                x: 0, y: 0, z: 0
            },
            length: {
                x:1, y:1, z:1
            },
            angle: {
                x: 0, y:0, z:0
            }
        };

        navigator.geolocation.getCurrentPosition(function (position) {
            $scope.position = position;
            //position.coords.latitude, position.coords.longitude
            var p1 = new breeze.Predicate('Latitude2', '>', $scope.position.coords.latitude - .5);
            var p2 = new breeze.Predicate('Latitude2', '<', $scope.position.coords.latitude + .5);
            var p3 = new breeze.Predicate('Longitude2', '>', $scope.position.coords.longitude - .5);
            var p4 = new breeze.Predicate('Longitude2', '<', $scope.position.coords.longitude + .5);
            var pred = new breeze.Predicate.and([p1, p2, p3, p4]);
            var positionQuery = breeze.EntityQuery.from('positionbreezeapi/search').orderByDesc('Id');//.where(pred).take(10);
            var positionPromise = breezeservice.executeQuery(positionQuery).then(function (data) {
                $scope.positions = data.httpResponse.data;
            });
        });

        var conflictQuery = breeze.EntityQuery.from('conflictbreezeapi/search').orderByDesc('Id');
        var conflictPromise = breezeservice.executeQuery(conflictQuery).then(function (data) {
            $scope.conflicts = data.httpResponse.data;
            $scope.isLoading = false;
            PlayCanvas($scope);
        });

        $scope.change = function () {
            //https://jsfiddle.net/end3r/auvcLoc4/?utm_source=website&utm_medium=embed&utm_campaign=auvcLoc4
            /*
            capsule.rotate(1, 1, 1);
            */
            camera.setPosition($scope.camera.position.x, $scope.camera.position.y, $scope.camera.position.z);
            camera.setEulerAngles($scope.camera.rotation.x, $scope.camera.rotation.y, $scope.camera.rotation.z);

            capsule.setLocalScale($scope.capsule.length.x, $scope.capsule.length.y, $scope.capsule.length.z);
            capsule.setPosition($scope.capsule.position.x, $scope.capsule.position.y, $scope.capsule.position.z);
            capsule.setEulerAngles($scope.capsule.rotation.x, $scope.capsule.rotation.y, $scope.capsule.rotation.z);
        }

        $scope.selectPosition = function (position) {
            var s = position;
            //find mid point between xyz1 and xyz2
            CalculateMidPoint(position);
            CalculateEulerAngles(position);
            //set capsule position to midpoint position
            $scope.capsule.position.x = midpoint.x;
            $scope.capsule.position.y = midpoint.y;
            $scope.capsule.position.z = midpoint.z;
            //find rotation via xyz1,xyz2 xyz vectors to euler angles
            $scope.capsule.rotation.x = midpoint.rotation.x;
            $scope.capsule.rotation.y = midpoint.rotation.y;
            $scope.capsule.rotation.z = midpoint.rotation.z;
            //set camera at mid point xyz - 10 on z
            $scope.camera.position.x = midpoint.x;
            $scope.camera.position.y = midpoint.y;
            if ($scope.capsule.z > 0) {
                $scope.camera.position.z = midpoint.z - 15;
            }
            else {
                $scope.camera.position.z = midpoint.z + 15;
            }
            //may need to set the light

            //distance of the capsule
            $scope.capsule.length.y = position.Speed2 / 60;

            $scope.change();
        }
    }]);

    function CalculateMidPoint(position) {
        midpoint.x = (position.X1 + position.X2) / 2;
        midpoint.y = (position.Y1 + position.Y2) / 2;
        midpoint.z = (position.Z1 + position.Z2) / 2;
        return midpoint;
    }

    //TODO: Handle gimbal lock
    function CalculateEulerAngles(position) {
        var productX = (position.X2 - position.X1);
        var productY = (position.Y2 - position.Y1);
        var productZ = (position.Z2 - position.Z1);

        var normalizedTotal = Math.sqrt(productX * productX + productY * productY + productZ * productZ);

        var unitVectorX, unitVectorY, unitVectorZ;
        if(normalizedTotal == 0)
        {
            unitVectorX = productX;
            unitVectorY = productY;
            unitVectorZ = productZ;
        }
        else
        {
            unitVectorX = productX / normalizedTotal;
            unitVectorY = productY / normalizedTotal;
            unitVectorZ = productZ / normalizedTotal;
        }

        midpoint.rotation.x = unitVectorX;
        midpoint.rotation.y = unitVectorY;
        midpoint.rotation.z = unitVectorZ;

        //distance traveled at current speed for 60 seconds (can make this dynamic later)
        var distance = position.Speed2.Value / 60;

        return position
    }
    
    //http://developer.playcanvas.com/en/tutorials/beginner/manipulating-entities/
    function PlayCanvas($scope) {
        //INITIALIZE
        var canvas = document.getElementById("application-canvas");
        var app = new pc.Application(canvas, {});
        app.start();

        app.setCanvasFillMode(pc.FILLMODE_NONE, 1024, 800);
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
        capsule.setPosition($scope.capsule.position.x, $scope.capsule.position.y, $scope.capsule.position.z);

        //TELEPORT
        //capsuleTemplate.rigidbody.teleport(-5, 0, 5);

        CreateFloor(app);
        // Add to hierarchy
        app.root.addChild(capsule);
        app.root.addChild(camera);
        app.root.addChild(light);

        // Set up initial positions and orientations
        camera.setPosition($scope.camera.position.x, $scope.camera.position.y, $scope.camera.position.z);
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

    function CreateFloor(app) {
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
        floor.setLocalScale(10000, 10000, .1);

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