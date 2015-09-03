
// show more list include in-active
function maintain_organisation_include_inactive(checkbox) {
    var url = "/MaintainOrganisation/GetListOrganisationsFromCheckbox";

    if (checkbox.checked) {
        $.post(url, { IncludeInActive: true }, function (data) {
            $("#organisation_table").html(data);
        });
    }
    else {
        $.post(url, { IncludeInActive: false }, function (data) {
            $("#organisation_table").html(data);
        });
    }
}


// idLink: id of actionlink, idPopupShow id of popup will be show, idPopupHide id of popup will be hide
function set_attr_link_popup(idLink, idPopupShow, idPopupHide) {

    idLink = "#" + idLink;
    idPopupShow = "#" + idPopupShow;
    idPopupHide = "#" + idPopupHide;

    $(idLink).attr('data-toggle', 'modal');
    $(idLink).attr('data-target', idPopupShow);
    $(idPopupShow).modal('show');
    $(idPopupHide).modal('hide');
}