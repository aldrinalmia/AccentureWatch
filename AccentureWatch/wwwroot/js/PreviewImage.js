$(document).ready(function () {
    $('#FormFile').change(function () {
        const [file] = FormFile.files;
        if (file) {
            imgPreview.src = URL.createObjectURL(file);
        }
    });
})