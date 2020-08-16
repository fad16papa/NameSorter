// This will run the spinner when the user click the upload button 
$(document).ready(function () {
    $("#btnUpload").click(function () {
        // add spinner to button
        $(this).html(
            `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Loading...`
        );
    });
});

//This will show the file name when the user upload a file
$('input[type="file"]').change(function (e) {
    var fileName = e.target.files[0].name;
    $('.custom-file-label').html(fileName);
});

