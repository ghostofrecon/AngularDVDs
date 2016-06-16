// Write your Javascript code.
$(function() {
    $(".body-content").removeClass("hidden");
    $("#addDirectorModal")
        .on("hidden.bs.modal",
            function() {
                $("#addDirectorModal input").val("");
            });
    $('#addGenreModal')
        .on("hidden.bs.modal",
            function() {
                $("newGenreNameInput").val("");
                $("newGenreDescInput").val("");

            });
    $("#genreIdSelect").select2();
    $("#logoffBtn")
                .click(function () {
            event.preventDefault();
                    $("#logoutForm").submit();
                });
});