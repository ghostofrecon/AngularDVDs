(function () {
    'use strict';

    angular
        .module('dvdApp')
        .directive("addDvdInline", addDvdInline)
        .controller('dvdListCtrl', dvdListCtrl);

    dvdListCtrl.$inject = ["$scope", "$http", "$compile"];

    function dvdListCtrl($scope, $http, $compile) {
        $scope.dvds = [];
        $scope.directors = [];
        $scope.genres = [];
        $scope.addMode = false;
        $scope.enableAddMode = function (state) {
            $http.get("api/directors").then(function (response) { $scope.directors = response.data; });
            $http.get("api/genres").then(function (response) { $scope.genres = response.data; });
            $scope.addMode = state;
        }
        $scope.jsonDebugging = "";
        $scope.dvdToAdd = {
            DVD_TITLE: "",
            DVD_DIRECTOR_ID: "",
            DVD_GENRE_ID: "",
            DVD_RELEASE_YEAR: new Date().getFullYear(),
            DVD_ADDMOD_Datetime: new Date()
        }

        $scope.refreshData = function (showToast) {
            $http.get("api/directors").then(function (response) { $scope.directors = response.data; });
            $http.get("api/genres").then(function (response) { $scope.genres = response.data; });
            $http.get("api/dvds").then(function (response) { $scope.dvds = response.data });

            console.log("DVDs Refreshed");

            if (showToast !== false) {
                toastr.options = {
                    "closeButton": false,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-bottom-left",
                    "preventDuplicates": false,
                    "onclick": null,
                    "showDuration": "300",
                    "hideDuration": "500",
                    "timeOut": "3000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };
                toastr["info"]("DVD's Refreshed.");
            }
            
        }
        $scope.refreshData(false);
        $scope.getDirector = function (id) {
            for (var i = 0; i < $scope.directors.length; i++) {
                if ($scope.directors[i].DIRECTOR_ID === id) {
                    return $scope.directors[i];
                }
            }
            return "";
        }
        $scope.getGenre = function (id) {
            for (var i = 0; i < $scope.genres.length; i++) {
                if ($scope.genres[i].GENRE_ID === id) {
                    return $scope.genres[i];
                }
            }
            return "";
        }
        $scope.removeDVD = function (id) {
            $http.delete("api/dvds/"+id).then(function(response) {
                toastr.options = {
                    "closeButton": false,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-bottom-left",
                    "preventDuplicates": false,
                    "onclick": null,
                    "showDuration": "300",
                    "hideDuration": "500",
                    "timeOut": "3000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };
                toastr["success"]("DVD Removed from database.");
            }, function (response){
                toastr.options = {
                    "closeButton": false,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": true,
                    "positionClass": "toast-bottom-left",
                    "preventDuplicates": false,
                    "onclick": null,
                    "showDuration": "300",
                    "hideDuration": "500",
                    "timeOut": "3000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };
                toastr["error"]("DVD Couldn't be removed. Code: " + response.statusCode);
        });
            for (var i = 0; i < $scope.dvds.length; i++) {
                if ($scope.dvds[i].DVD_ID === id) {

                    $scope.dvds.splice(i, 1);
                }
            }
        }
        $scope.dvdsIsEmpty = function () {
            if ($scope.dvds.length === 0) {
                return true;
            }
            return false;
        }
        $scope.addDVD = function () {
            var html = "<tr id='addDvdRow' add-dvd-inline></tr>";
            var elem = angular.element(html);
            $compile(elem);
            $("#AddDvdRow").replaceWith(elem);
        }
        $scope.saveDVD = function () {
           
            var dvd = $scope.dvdToAdd;
            $scope.dvdToAdd = {
                DVD_ID: "",
                DVD_TITLE: "",
                DVD_DIRECTOR_ID: "",
                DVD_GENRE_ID: "",
                DVD_RELEASE_YEAR: new Date().getFullYear(),
                DVD_ADDMOD_Datetime: new Date()
            }
            
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-bottom-left",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "500",
                "timeOut": "3000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
            toastr["info"](dvd.DVD_TITLE + " Added to list.");


            postDvdtoApi(dvd);

            

            $scope.enableAddMode(false);
        }
        $scope.getJson = function(object) {
            return angular.fromJson(object);
        }
        function postDvdtoApi(dvd) {
            $http.post("api/dvds", dvd)
                .then(function (response) {
                    var code = response.statusCode;
                    console.log("dvd post action succeded: " + code);
                    toastr.options = {
                        "closeButton": false,
                        "debug": false,
                        "newestOnTop": false,
                        "progressBar": true,
                        "positionClass": "toast-bottom-left",
                        "preventDuplicates": false,
                        "onclick": null,
                        "showDuration": "300",
                        "hideDuration": "500",
                        "timeOut": "3000",
                        "extendedTimeOut": "1000",
                        "showEasing": "swing",
                        "hideEasing": "linear",
                        "showMethod": "fadeIn",
                        "hideMethod": "fadeOut"
                    };
                    toastr["success"](dvd.DVD_TITLE + " successfully added to database.");
                    var id = response.data.DVD_ID;
                        dvd.DVD_ID = id;
                    $scope.dvds.push(dvd);

                    },
                    function (response) {
                        console.log("DVD Post action failed: " + response.statusCode);
                        toastr.options = {
                            "closeButton": false,
                            "debug": false,
                            "newestOnTop": true,
                            "progressBar": true,
                            "positionClass": "toast-bottom-left",
                            "preventDuplicates": false,
                            "onclick": null,
                            "showDuration": "300",
                            "hideDuration": "500",
                            "timeOut": "3000",
                            "extendedTimeOut": "1000",
                            "showEasing": "swing",
                            "hideEasing": "linear",
                            "showMethod": "fadeIn",
                            "hideMethod": "fadeOut"
                        };
                        toastr["error"]("Addition of " + dvd.DVD_TITLE + " failed.");
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
            link: function (scope) {
                scope.dvdToAdd = $scope.dvds;
                scope.directors = $scope.directors;
                scope.genres = $scope.genres;
                
            }
        }
    }
})();
