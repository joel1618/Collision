(function () {

    angular.module('services').factory('aircraftservice',
    ['$http', '$q', '$timeout', 'breeze', 'breezeservice', service]);

    function service($http, $q, $timeout, breeze, breezeservice) {

        var service = {
            GetConflicts: GetConflicts,
            GetPositions: GetPositions,
            AddAircraft: AddAircraft,
            CalculateMidPoint: CalculateMidPoint,
            CalculateEulerAngles: CalculateEulerAngles
        };
        return service;

        function GetConflicts() {
            var conflictQuery = breeze.EntityQuery.from('conflictbreezeapi/search').orderByDesc('Id');
            return conflictPromise = breezeservice.executeQuery(conflictQuery).then(function (data) {
                return data.httpResponse.data;
            });
        }


        function GetPositions(position) {
            var p1 = new breeze.Predicate('Latitude2', '>', position.coords.latitude - .5);
            var p2 = new breeze.Predicate('Latitude2', '<', position.coords.latitude + .5);
            var p3 = new breeze.Predicate('Longitude2', '>', position.coords.longitude - .5);
            var p4 = new breeze.Predicate('Longitude2', '<', position.coords.longitude + .5);
            var pred = new breeze.Predicate.and([p1, p2, p3, p4]);
            var positionQuery = breeze.EntityQuery.from('positionbreezeapi/search').orderByDesc('Id');//.where(pred).take(10);
            return positionPromise = breezeservice.executeQuery(positionQuery).then(function (data) {
                return data.httpResponse.data;
            });
        }

        function AddAircraft(aircraft) {
            //https://jsfiddle.net/end3r/auvcLoc4/?utm_source=website&utm_medium=embed&utm_campaign=auvcLoc4
            /*
            capsule.rotate(1, 1, 1);
            */
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
            capsule.model.material = CreateMaterial(new pc.Color(1, 0, 0));

            //POSITION
            //capsule.setPosition($scope.capsule.position.x, $scope.capsule.position.y, $scope.capsule.position.z);
            capsule.setPosition(20, 20, 20);
            capsule.setLocalScale(1, 20, 1);

            //TELEPORT
            //capsuleTemplate.rigidbody.teleport(-5, 0, 5);
            //debugger;
            // Add to hierarchy
            app.root.addChild(capsule);
            //camera.setPosition($scope.camera.position.x, $scope.camera.position.y, $scope.camera.position.z);
            //camera.setEulerAngles($scope.camera.rotation.x, $scope.camera.rotation.y, $scope.camera.rotation.z);
            //capsule.setLocalScale($scope.capsule.length.x, $scope.capsule.length.y, $scope.capsule.length.z);
            //capsule.setPosition($scope.capsule.position.x, $scope.capsule.position.y, $scope.capsule.position.z);
            //capsule.setEulerAngles($scope.capsule.rotation.x, $scope.capsule.rotation.y, $scope.capsule.rotation.z);
        }


        function CalculateMidPoint(position) {
            var midpoint = {
                position: {
                    x: 0, y: 0, z: 10
                },
                rotation: {
                    x: 0, y: 0, z: 0
                }
            };
            midpoint.x = (position.X1 + position.X2) / 2;
            midpoint.y = (position.Y1 + position.Y2) / 2;
            midpoint.z = (position.Z1 + position.Z2) / 2;
            return midpoint;
        }

        //TODO: Handle gimbal lock
        //http://stackoverflow.com/questions/18184848/calculate-pitch-and-yaw-between-two-unknown-points
        //http://www.codeproject.com/Questions/324240/Determining-yaw-pitch-and-roll
        function CalculateEulerAngles(position, midpoint) {
            var productX = (position.X2 - position.X1);
            var productY = (position.Y2 - position.Y1);
            var productZ = (position.Z2 - position.Z1);

            var normalizedTotal = Math.sqrt(productX * productX + productY * productY + productZ * productZ);

            var unitVectorX, unitVectorY, unitVectorZ;
            if (normalizedTotal == 0) {
                unitVectorX = productX;
                unitVectorY = productY;
                unitVectorZ = productZ;
            }
            else {
                unitVectorX = productX / normalizedTotal;
                unitVectorY = productY / normalizedTotal;
                unitVectorZ = productZ / normalizedTotal;
            }

            var dX = (position.X2 - position.X1);
            var dY = (position.Y2 - position.Y1);
            var dZ = (position.Z2 - position.Z1);

            var pitch = Math.atan2(Math.sqrt(dZ * dZ + dX * dX), dY) + Math.PI;
            var roll = 0;
            var yaw = position.Heading2; //Math.atan2(dZ, dX);

            midpoint.rotation.x = pitch;
            midpoint.rotation.y = roll;
            midpoint.rotation.z = yaw;

            //distance traveled at current speed for 60 seconds (can make this dynamic later)
            var distance = position.Speed2.Value / 60;

            return position
        }

        function CreateMaterial(color) {
            var material = new pc.PhongMaterial();
            material.diffuse = color;
            // we need to call material.update when we change its properties
            material.update()
            return material;
        }
    }
})();