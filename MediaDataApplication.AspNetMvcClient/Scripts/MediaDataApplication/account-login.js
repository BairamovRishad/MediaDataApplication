function AccountLogin() {
    var self = this;

    this.init = function() {
        $("#showRegister").click(function() {
            $("#loginPanel").hide("slide", function() {
                $("#registerPanel").show("slide", function() {
                    $("input[name='UserName']").focus();
                });
            });
        });
    };

    this.onBegin = function() {
        $('input.login').addClass("disabled");
    };

    this.onFailure = function(data) {
        $('input.login').removeClass("disabled");
        $('#loginPanel').html(data.responseText);
    };

    this.onSuccess = function() {
        $('input.login').removeClass("disabled");
        window.location.href = '/MediaManager/Main';
    };

}

var accountLogin = null;
$(document).ready(function() {
    accountLogin = new AccountLogin();
    accountLogin.init();
});