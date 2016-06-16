'use strict';

(function () {
    'use strict';

    angular.module('dvdApp').directive("addDvdInline", addDvdInline).controller('dvdListCtrl', dvdListCtrl);

    dvdListCtrl.$inject = ["$scope", "$http", "$compile", "toastFactory", "usSpinnerService"];

    function dvdListCtrl($scope, $http, $compile, toastFactory, usSpinnerService) {
        var self = this;
        $scope.dvds = [];
        $scope.directors = [];
        $scope.genres = [];
        $scope.dvdSearch = "";
        $scope.addMode = false;
        $scope.orderColumn = "-DVD_ADDMOD_Datetime";
        $scope.orderTableBy = function (column) {
            switch ($scope.orderColumn) {
                case column:
                    $scope.orderColumn = "-" + column;
                    break;
                default:
                    $scope.orderColumn = column;
                    break;
            }
        };
        $scope.enableAddMode = function (state) {
            $http.get("api/directors").then(function (response) {
                $scope.directors = response.data;
            });
            $http.get("api/genres").then(function (response) {
                $scope.genres = response.data;
            });
            $scope.addMode = state;
        };
        $scope.jsonDebugging = "";
        $scope.dvdToAdd = {
            DVD_TITLE: "",
            DVD_DIRECTOR_ID: "",
            DVD_GENRE_ID: "",
            DVD_RELEASE_YEAR: new Date().getFullYear(),
            DVD_ADDMOD_Datetime: new Date()
        };
        $scope.spinnerActive = false;
        function startSpin() {
            usSpinnerService.spin('dvdSpinner');
        };
        function stopSpin() {
            usSpinnerService.stop('dvdSpinner');
        };
        $scope.refreshData = function (showToast) {

            startSpin();
            $scope.dvds = [];
            setTimeout(function () {
                $http.get("api/directors").then(function (response) {
                    $scope.directors = response.data;
                });
            }, 500);

            setTimeout(function () {
                $http.get("api/genres").then(function (response) {
                    $scope.genres = response.data;
                });
            }, 500);
            setTimeout(function () {
                $http.get("api/dvds").then(function (response) {
                    $scope.dvds = response.data;
                    setTimeout(function () {
                        stopSpin();
                    }, 300);
                });
            }, 500);

            $scope.dvdSearch = "";

            if (showToast !== false) {
                toastFactory.showToast("info", 3000, "DVD Data Refreshed");
            }
        };
        $scope.refreshData(false);
        $scope.removeDVD = function (id) {
            $http['delete']("api/dvds/" + id).then(function (response) {
                toastFactory.showToast("success", 3000, "DVD removed from database");
            }, function (response) {
                toastFactory.showToast("error", 3000, "DVD couldn't be removed: Code: " + response.statusCode);
            });
            for (var i = 0; i < $scope.dvds.length; i++) {
                if ($scope.dvds[i].DVD_ID === id) {

                    $scope.dvds.splice(i, 1);
                }
            }
        };
        $scope.dvdsIsEmpty = function () {
            if ($scope.dvds.length === 0) {
                return true;
            }
            return false;
        };
        $scope.addDVD = function () {
            var html = "<tr id='addDvdRow' add-dvd-inline></tr>";
            var elem = angular.element(html);
            $compile(elem);
            $("#AddDvdRow").replaceWith(elem);
        };
        $scope.saveDVD = function () {

            if ($scope.dvdToAdd.DVD_GENRE_ID === "") {
                $('#genreIdSelect').popover({ placement: "bottom" }).popover("show");
            }
            if ($scope.dvdToAdd.DVD_DIRECTOR_ID === "") {
                $('#directorIdSelect').popover({ placement: "top" }).popover("show");
            }

            if ($scope.dvdToAdd.DVD_DIRECTOR_ID === "" || $scope.dvdToAdd.DVD_GENRE_ID === "") {
                return;
            }

            var dvd = $scope.dvdToAdd;
            $scope.dvdToAdd = {
                DVD_TITLE: "",
                DVD_DIRECTOR_ID: "",
                DVD_GENRE_ID: "",
                DVD_RELEASE_YEAR: new Date().getFullYear(),
                DVD_ADDMOD_Datetime: new Date()
            };
            toastFactory.showToast("info", 3000, dvd.DVD_TITLE + " added to list.");
            postDvdtoApi(dvd);
            $scope.enableAddMode(false);
        };
        $scope.getJson = function (object) {
            return angular.fromJson(object);
        };
        function postDvdtoApi(dvd) {
            $http.post("api/dvds", dvd).then(function (response) {
                var code = response.statusCode;
                console.log("dvd post action succeded: " + code);
                toastFactory.showToast("success", 3000, dvd.DVD_TITLE + " successfully added to database.");
                var id = response.data.DVD_ID;
                dvd.DVD_ID = id;
                $scope.dvds.push(dvd);
            }, function (response) {
                console.log("DVD Post action failed: " + response.statusCode);
                toastFactory.showToast("error", 3000, "Addition of " + dvd.DVD_TITLE + " failed.");
                var ind = $scope.dvds.indexOf(dvd);
                $scope.slice(ind, 1);
            });
        }
    }

    function addDvdInline() {
        return {
            templateUrl: "templates/AddDvdInline",
            restrict: "AE",
            replace: true,
            scope: true,
            link: function link() {}
        };
    }
})(window);

