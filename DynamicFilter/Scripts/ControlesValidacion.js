$(document).ready(function () {
    $('#BtnSave').attr("readonly", true);
    
    $("#TypeID option:selected").attr('disabled', 'disabled');
    $("#StateID option:selected").attr('disabled', 'disabled');
});