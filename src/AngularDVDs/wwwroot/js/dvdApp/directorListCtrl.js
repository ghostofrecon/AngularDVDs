(function () {
    'use strict';

    angular
        .module('dvdApp')
        .controller('directorListCtrl', directorListCtrl);

    directorListCtrl.$inject = ['$scope', '$http']; 

    function directorListCtrl($scope, $http) {
        $scope.title = 'directorListCtrl';
        $scope.DirectorsNameList = [];
        activate();

        function activate() {
            $http.get("api/directors").then(function (response) { $scope.DirectorsNameList = response.data.slice(0, 3).sort(function (a, b) { return (a.DIRECTOR_ADDMOD_Datetime < b.DIRECTOR_ADDMOD_Datetime) ? 1 : ((a.DIRECTOR_ADDMOD_Datetime > b.DIRECTOR_ADDMOD_Datetime) ? -1 : 0)}) });
        }
    }
})();
