'use strict';

(function () {
    'use strict';

    angular.module('dvdApp').directive('addDirectorModal', addDirectorModal).directive("fullDirectorListModal", fullDirectorListModal).filter("startFrom", startFromFilter).controller('directorListCtrl', directorListCtrl);

    directorListCtrl.$inject = ['$scope', '$http', 'toastFactory'];

    function fullDirectorListModal() {
        return {
            templateUrl: "templates/fullDirectorListModal",
            restrict: "AE",
            replace: true,
            scope: true,
            link: function link() {}
        };
    }

    function addDirectorModal() {
        return {
            templateUrl: "templates/AddDirectorModal",
            restrict: "AE",
            replace: true,
            scope: true,
            link: function link() {}
        };
    }

    function startFromFilter() {
        return function (input, start) {
            start = +start; //parse to int
            return input.slice(start);
        };
    }

    function directorListCtrl($scope, $http, toastFactory) {
        $scope.title = 'directorListCtrl';
        $scope.DirectorsNameList = [];
        $scope.dirToAdd = "";
        $scope.directors = [];
        $scope.start = 0;
        $scope.currentPage = 0;
        $scope.reactivate = function () {
            $http.get("api/directors").then(function (response) {
                $scope.DirectorsNameList = response.data.sort(function (a, b) {
                    return a.DIRECTOR_ADDMOD_Datetime < b.DIRECTOR_ADDMOD_Datetime ? 1 : a.DIRECTOR_ADDMOD_Datetime > b.DIRECTOR_ADDMOD_Datetime ? -1 : 0;
                });
            });
            toastFactory.showToast("info", 3000, "Directors refreshed");
        };
        $scope.numberOfPages = function () {
            return Math.ceil($scope.DirectorsNameList / 10);
        };
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
            $http.post("api/directors", { DIRECTOR_NAME: newDir }).then(function (response) {
                toastFactory.showToast("success", 3000, newDir + " added to database");
                $scope.dirToAdd = "";
                activate();
                $('#addDirectorModal').modal('hide');
            }, function (response) {
                toastFactory.showToast("error", 3000, newDir + " could not be added to database");
            });
        };
        activate();
        $scope.dirOrderColumn = "DIRECTOR_NAME";
        $scope.orderList = function (col) {
            if ($scope.dirOrderColumn === col) {
                $scope.dirOrderColumn = '-' + col;
            } else {
                $scope.dirOrderColumn = col;
            }
        };

        function activate() {
            $http.get("api/directors").then(function (response) {
                $scope.DirectorsNameList = response.data.sort(function (a, b) {
                    return a.DIRECTOR_ADDMOD_Datetime < b.DIRECTOR_ADDMOD_Datetime ? 1 : a.DIRECTOR_ADDMOD_Datetime > b.DIRECTOR_ADDMOD_Datetime ? -1 : 0;
                });
            });

            //$scope.DirectorsNameList = $scope.directors.slice(0, 3)
        }
    }
})();

