(function () {
    "use strict";
    angular.module('services')
    .service('positionservice', ['$http', '$q', 'breeze', 'breezeservice',
        function ($http, $q, breeze, breezeservice) {
        var _self = this;
        this.deferredRequest = null;

        this.search = function (predicate, page, pageSize, cancelExistingSearch) {
            cancelExistingSearch = cancelExistingSearch || false;

            if (this.deferredRequest !== null && cancelExistingSearch) {
                this.deferredRequest.reject("Cancelled Search Request.");
                this.deferredRequest = null;
            }

            var deferred = $q.defer();

            var query = breeze.EntityQuery.from('PositionBreezeApi/Search');
            if (predicate != null) {
                query = query.where(predicate);
            }
            query = query.orderByDesc('Id').skip(page * pageSize).take(pageSize);
                        
            breezeservice.executeQuery(query).then(function (data) {
                deferred.resolve(data.httpResponse.data);
                _self.deferredRequest = null;
            }, function (msg, code) {
                deferred.reject(msg);
                _self.deferredRequest = null;
            });
            
            this.deferredRequest = deferred;

            return deferred.promise;
        };

        this.get = function (id) {
            var deferred = $q.defer();

            $http({
                method: 'Get',
                url: '/PositionApi/Get/' + id,
            }).success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (msg, code) {
                deferred.reject(msg);
            });

            return deferred.promise;
        };

        this.create = function (item) {
            var deferred = $q.defer();

            $http.post('/PositionApi/Create', item)
            .then(function (response) {
                deferred.resolve(response);
            }, function (response) {
                if (response.statusText.length > 0) {
                    deferred.reject(response.statusText);
                } else {
                    deferred.reject("Failed to create the new requirement.");
                }
            });

            return deferred.promise;
        };

        this.update = function (id, item) {
            var deferred = $q.defer();

            $http.put('/PositionApi/Update/' + id, item)
            .then(function (response) {
                deferred.resolve(response);
            }, function (response) {
                if (response.statusText.length > 0) {
                    deferred.reject(response);
                } else {
                    deferred.reject("Failed to update the requirement.");
                }
            });

            return deferred.promise;
        }
    }]);
})();