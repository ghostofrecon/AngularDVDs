(function () {
    'use strict';

    angular
        .module('dvdApp')
        .controller('genreListCtrl', genreListCtrl);

    genreListCtrl.$inject = ['$scope', "$http"]; 

    function genreListCtrl($scope, $http) {
        $scope.title = 'genreListCtrl';
        $scope.genres = [];
        activate();
        
        function activate() {
            $http.get("api/genres").then(function (response) { $scope.genres = response.data.slice(0,3).sort(function (a, b) { return (a.DIRECTOR_ADDMOD_Datetime < b.GENRE_ADDMOD_Datetime) ? 1 : ((a.GENRE_ADDMOD_Datetime > b.GENRE_ADDMOD_Datetime) ? -1 : 0)}) });
        }
    }
})();
