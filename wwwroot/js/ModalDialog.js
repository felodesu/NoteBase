$(document).ready(function () {
    $(".modalShow").click(function () {
        var clickedNoteId = $(this).val();
        $("#note-id-container-modal").val(clickedNoteId);
        $("#shareModal").modal('show');
    });

    $("#modalHide").click(function () {
        $("#shareModal").modal('hide');
    });

    $("#user-selector").change(function () {
        var selectedUsers = $(".selectpicker").selectpicker('val');
        if (selectedUsers.includes("-1")) {
            $(".selectpicker").selectpicker("deselectAll");
            $(".selectpicker").selectpicker("val", "-1");
        }
    });
});
