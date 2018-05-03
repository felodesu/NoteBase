$(document).ready(function () {
    $(".modalShow").click(function () {
        var clickedNoteId = $(this).val();
        $("#note-id-container-modal").val(clickedNoteId);
        $("#shareModal").modal('show');
    });

    $("#modalHide").click(function () {
        $("#shareModal").modal('hide');
    });
});