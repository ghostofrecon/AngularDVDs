// Write your Javascript code.
$(function() {
    $(".body-content").removeClass("hidden");
    $("#addDirectorModal")
                .on("hidden.bs.modal",
                    function () {
                        $("#addDirectorModal input").val("");
                    });
})