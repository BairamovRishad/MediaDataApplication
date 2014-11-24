function AccountRegister() {
    var self = this;

    this.init = function() {
        $("#showLogin").click(function() {
            showLoginPanel();
        });
    };

    this.onBegin = function() {
        $('input.register').addClass("disabled");
    };

    this.onSuccess = function() {
        $('input.register').removeClass("disabled");
        showLoginPanel();
    };

    this.onFailure = function(data) {
        $('#registerPanel').html(data.responseText);
    };

    function showLoginPanel() {
        $('#registerPanel').hide('slide', function() {
            $('#loginPanel').show('slide', function() {
                var userName = $('#registerPanel input[name="UserName"]').val();
                $('input[name="UserName"]').val(userName);
                $('input[name="Password"]').val("").focus();
            });
        });
    }
}

var accountRegister = null;
$(document).ready(function() {
    accountRegister = new AccountRegister();
    accountRegister.init();
});