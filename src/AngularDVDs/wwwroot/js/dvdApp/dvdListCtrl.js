﻿(function () {
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
        $scope.enableAddMode = function(state) {
            $scope.addMode = state;
        }
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

            console.log("Data Refreshed");

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
                    "timeOut": "1000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };
                toastr["info"]("Data Refreshed.");
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
            $scope.dvds.push($scope.dvdToAdd);
            $scope.enableAddMode(false);
            $scope.dvdToAdd = {
                DVD_TITLE: "",
                DVD_DIRECTOR_ID: "",
                DVD_GENRE_ID: "",
                DVD_RELEASE_YEAR: new Date().getFullYear(),
                DVD_ADDMOD_Datetime: new Date()
            }
        }
        $scope.getJson = function(object) {
            return angular.fromJson(object);
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
