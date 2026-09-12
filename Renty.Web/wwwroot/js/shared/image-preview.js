export function createImagePreview({ inputId, previewId }) {
    var input = document.getElementById(inputId);
    var preview = document.getElementById(previewId);

    input.addEventListener('change', function () {
        preview.innerHTML = '';
        Array.from(input.files).forEach(function (file) {
            var img = document.createElement('img');
            img.src = URL.createObjectURL(file);
            preview.appendChild(img);
        });
    });
}
