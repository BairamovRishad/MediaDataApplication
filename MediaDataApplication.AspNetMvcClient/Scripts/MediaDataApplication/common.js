function Common() {
    var self = this;
    this.modal = new Modal();

    this.init = function() {

    };

    this.showError = function(xhr) {
        var responseTitle = $(xhr.responseText).filter('title').get(0);
        alert($(responseTitle).text());
    };

}

var common = null;
$(document).ready(function() {
    common = new Common();
    common.init();
});

function Modal() {
    this.show = function(data) {
        $(".modal-backdrop").remove();
        var popupWrapper = $("#PopupWrapper");
        popupWrapper.empty();
        popupWrapper.html(data);
        $(".modal", popupWrapper).modal();
    };

    this.close = function() {
        setTimeout(function() {
            $('.modal').modal('hide');
        }, 170);
    };
}