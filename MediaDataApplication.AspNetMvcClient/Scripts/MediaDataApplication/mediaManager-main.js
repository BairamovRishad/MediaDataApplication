function MediaManagerMain() {
    var self = this;

    this.init = function() {
        $(document).on('click', 'button.details', function() {
            $.ajax({
                type: 'GET',
                url: '/MediaManager/MediaMetadataEdit',
                data: {
                    fileName: $(this).data('url')
                },
                success: function(data) {
                    common.modal.show(data);
                }
            });

            return false;
        });

        $(document).on('click', 'a[download]', function() {
            var $this = $(this);

            $this.parent().prepend("<span>Please wait, downloading...</span>");

            $.ajax({
                type: 'GET',
                url: '/MediaManager/DownloadMedia',
                data: {
                    fileName: $this.attr('download'),
                    fileUrl: $this.attr('href')
                },
                async: false
            });

            $this.parent().find('span').remove();
        });

        $('button.search').click(function() {
            var term = $(this).parent().siblings('input[type="search"]').val();
            search(term);
            return false;
        });

        $('input[type="search"]').keypress(function(event) {
            if (event.which == 13) {
                event.preventDefault();
                var term = $(this).val();
                search(term);
            }
        });

    };

    this.onMetadataEditFailure = function(xhr) {
        var errorMsg = JSON.parse(xhr.responseText).errorMsg;
        alert(errorMsg);
    };

    this.onMetadataEditSuccess = function() {
        location.reload();
    };

    function search(term) {
        // if term is empty then show all media
        if (!term) {
            $('.noresults').hide();
            $('.files').children().show();
            return;
        }

        $.ajax({
            type: 'GET',
            url: '/MediaManager/Search',
            data: { term: term },
            success: function(data) {
                showSearchResults(data);
            }
        });
    }

    function showSearchResults(foundFilesName) {
        if (!foundFilesName || foundFilesName.length === 0) {
            $('.noresults').show();
            $('.files').hide();
            return;
        } else {
            $('.noresults').hide();
            $('.files').show();
        }

        $('.files').find('.name a').each(function() {
            var $this = $(this);
            for (var i = 0; i < foundFilesName.length; i++) {
                if (foundFilesName[i] === $this.text()) {
                    $this.closest('tr').show();
                    break;
                }
            }

            // if current filename is not in foundFilesName
            if (i === foundFilesName.length) {
                $this.closest('tr').hide();
            }
        });
    }
};

var mediaManagerMain = null;
$(document).ready(function() {
    mediaManagerMain = new MediaManagerMain();
    mediaManagerMain.init();
});