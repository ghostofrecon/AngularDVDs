'use strict';

(function () {
    'use strict';

    angular.module('dvdApp').factory('toastFactory', factory);

    function factory() {
        var service = {
            showToast: showToast

        };

        return service;

        function showToast(type, timeout, message) {
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
                "timeOut": timeout,
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };
            toastr[type](message);
        }
    }
})();

