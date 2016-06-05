(function () {
    'use strict';

    angular
        .module('dvdApp')
        .controller('directorListCtrl', directorListCtrl);

    directorListCtrl.$inject = ['$scope', '$http', 'toastFactory']; 

    function directorListCtrl($scope, $http, toastFactory) {
        $scope.title = 'directorListCtrl';
        $scope.DirectorsNameList = [];
        $scope.dirToAdd = "";
        $scope.directors = [];
        $scope.reactivate = function() {
            $http.get("api/directors").then(function (response) { $scope.DirectorsNameList = response.data.sort(function (a, b) { return (a.DIRECTOR_ADDMOD_Datetime < b.DIRECTOR_ADDMOD_Datetime) ? 1 : ((a.DIRECTOR_ADDMOD_Datetime > b.DIRECTOR_ADDMOD_Datetime) ? -1 : 0) }) });
            toastFactory.showToast("info", 3000, "Directors refreshed");
        }
        $scope.addDirector = function (newDir) {
            if ($scope.DirectorsNameList) {
                for (var i = 0; i < $scope.DirectorsNameList.length; i++) {
                    if ($scope.DirectorsNameList[i].DIRECTOR_NAME === newDir) {
                        $('#newDirInput').popover({ placement: "top" }).popover('show');
                        toastFactory.showToast("error", 3000, newDir + " alreay exists in database");
                        return;
                    }
                }
            }
            $http.post("api/directors", { DIRECTOR_NAME: newDir })
                .then(function(response) {
                    toastFactory.showToast("success", 3000, response.DIRECTOR_NAME + " added to database");
                    $scope.dirToAdd = "";
                    activate();
                    $('#addDirectorModal').modal('hide');
                }, function(response) {
                    toastFactory.showToast("error", 3000, response.DIRECTOR_NAME + " could not be added to database");
                });
        }
        activate();

        function activate() {
            $http.get("api/directors").then(function (response) { $scope.DirectorsNameList = response.data.sort(function (a, b) { return (a.DIRECTOR_ADDMOD_Datetime < b.DIRECTOR_ADDMOD_Datetime) ? 1 : ((a.DIRECTOR_ADDMOD_Datetime > b.DIRECTOR_ADDMOD_Datetime) ? -1 : 0) }) });
            
            //$scope.DirectorsNameList = $scope.directors.slice(0, 3)
        }
    }
})();
