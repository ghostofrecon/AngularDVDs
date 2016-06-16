'use strict';

(function () {
    'use strict';

    angular.module('dvdApp').directive("addGenreModal", addGenreModal).controller('genreListCtrl', genreListCtrl);

    genreListCtrl.$inject = ['$scope', "$http", "toastFactory", "usSpinnerService"];

    function addGenreModal() {
        return {
            templateUrl: "templates/addGenreModal",
            restrict: "AE",
            replace: true,
            scope: true,
            link: function link() {}
        };
    }

    function genreListCtrl($scope, $http, toastFactory, usSpinnerService) {
        $scope.title = 'genreListCtrl';
        $scope.genres = [];
        function startSpin() {
            usSpinnerService.spin('genSpinner');
        };
        function stopSpin() {
            usSpinnerService.stop('genSpinner');
        };
        $scope.addGenre = function (nGenreName, nGenreDesc) {
            if (nGenreDesc === undefined) {
                nGenreDesc = " ";
            }
            for (var i = 0; i < $scope.genres.length; i++) {
                if ($scope.genres[i].GENRE_NAME.toLowerCase() === nGenreName.toLowerCase()) {
                    toastFactory.showToast("error", 3000, "Genre alredy exists in database");
                    $("#newGenreNameInput").popover({ placement: "top" }).popover("show");
                    return;
                }
            }
            $http.post("api/genres", { GENRE_NAME: nGenreName, GENRE_DESC: nGenreDesc }).then(function () {
                toastFactory.showToast("success", 3000, "Genre added to database");
                $("#addGenreModal").modal("hide");
                $scope.refreshGenres();
            }, function () {
                toastFactory.showToast("error", 3000, "Genre could not be added to database");
            });
        };
        activate();

        function activate() {
            $http.get("api/genres").then(function (response) {
                $scope.genres = response.data.sort(function (a, b) {
                    return a.DIRECTOR_ADDMOD_Datetime < b.GENRE_ADDMOD_Datetime ? 1 : a.GENRE_ADDMOD_Datetime > b.GENRE_ADDMOD_Datetime ? -1 : 0;
                });
            });
        }

        $scope.refreshGenres = function () {
            startSpin();
            $scope.genres = [];
            setTimeout(function () {
                $http.get("api/genres").then(function (response) {
                    $scope.genres = response.data.sort(function (a, b) {
                        return a.DIRECTOR_ADDMOD_Datetime >= b.GENRE_ADDMOD_Datetime ? a.GENRE_ADDMOD_Datetime > b.GENRE_ADDMOD_Datetime ? -1 : 0 : 1;
                    });
                    setTimeout(function () {
                        stopSpin();
                    }, 300);
                });
            }, 500);

            toastFactory.showToast("info", 3000, "Genres refreshed.");
        };
    }
})();

