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
        $scope.addDirector = function (newDir) {
            if ($scope.directorNameList) {
                for (var i = 0; i < $scope.directorNameList.length; i++) {
                    if ($scope.directorNameList[i].DIRECTOR_NAME == newDir) {
                        $('#newDirInput').popover({ placement: "bottom" });
                        return;
                    }
                }
            }
            $http.post("api/directors", { DIRECTOR_NAME: newDir })
                .then(function(response) {
                    toastFactory.showToast("suceess", 3000, response.DIRECTOR_NAME + " added to database");
                    $scope.dirToAdd = "";
                    activate();
                    $('#addDirectorModal').modal('hide');
                }, function(response) {
                    toastFactory.showToast("error", 3000, response.DIRECTOR_NAME + " could not be added to database");

                });
        }
        activate();

        function activate() {
            $http.get("api/directors").then(function (response) { $scope.DirectorsNameList = response.data.slice(0, 3).sort(function (a, b) { return (a.DIRECTOR_ADDMOD_Datetime < b.DIRECTOR_ADDMOD_Datetime) ? 1 : ((a.DIRECTOR_ADDMOD_Datetime > b.DIRECTOR_ADDMOD_Datetime) ? -1 : 0)}) });
        }
    }
})();
