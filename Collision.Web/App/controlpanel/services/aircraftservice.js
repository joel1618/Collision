(function () {

    angular.module('services').factory('aircraftservice',
    ['$http', '$q', '$timeout', 'breeze', 'breezeservice', service]);

    function service($http, $q, $timeout, breeze, breezeservice) {

        var aircraft1, aircraft2;
        var service = {
            GetConflicts: GetConflicts,
            GetPositions: GetPositions,
            AddAircraft: AddAircraft,
            AddConflict: AddConflict,
            Delete: Delete,
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

        function AddAircraft(position) {
            Delete();
            //https://jsfiddle.net/end3r/auvcLoc4/?utm_source=website&utm_medium=embed&utm_campaign=auvcLoc4
            //CAPSULE
            entity = new pc.Entity();
            entity.addComponent("model", {
                type: "capsule",
                castShadows: true,
            });

            entity.addComponent("rigidbody", {
                type: "capsule",
                mass: 50,
                restitution: 0.5,
            });

            entity.addComponent("collision", {
                type: "capsule",
                radius: 0.5,
            });

            //COLOR
            entity.model.material = CreateMaterial(new pc.Color(1, 0, 0));

            //ROTATE
            var angle = CalculateEulerAngles(position);
            entity.setEulerAngles(angle.x, angle.y, angle.z);

            //POSITION
            entity.setPosition(20, 20, 20);

            //LENGTH, WIDTH, HEIGHT
            entity.setLocalScale(1, CalculateLength(position), 1);

            // Add to hierarchy
            app.root.addChild(entity);
            aircraft1 = entity;
        }

        function AddConflict(position1, position2) {

        }
        
        function Delete() {
            if (aircraft1 !== undefined && aircraft1 !== {}) {
                aircraft1.destroy();
            }
            if (aircraft2 !== undefined && aircraft2 !== {}) {
                aircraft2.destroy();
            }
        }


        function CalculateMidPoint(position) {
            var midpoint = {
                    x: 0, y: 0, z: 10
            };
            midpoint.x = (position.X1 + position.X2) / 2;
            midpoint.y = (position.Y1 + position.Y2) / 2;
            midpoint.z = (position.Z1 + position.Z2) / 2;
            return midpoint;
        }

        //TODO: Handle gimbal lock
        //http://stackoverflow.com/questions/18184848/calculate-pitch-and-yaw-between-two-unknown-points
        //http://www.codeproject.com/Questions/324240/Determining-yaw-pitch-and-roll
        function CalculateEulerAngles(position) {
            var angles = {
                x: 0, y: 0, z: 0
            };
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
            var yaw = position.Heading2;

            angles.x = pitch;
            angles.y = roll;
            angles.z = yaw;

            return angles
        }

        function CalculateLength(position) {
            //distance traveled at current speed for 60 seconds (can make this dynamic later)
            return position.Speed2 / 60;
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