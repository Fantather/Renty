var picker = document.getElementById('photoPicker');
var imagesInput = document.getElementById('Images');
var grid = document.getElementById('photoGrid');
var submitButton = document.querySelector('button[type="submit"][form="' + imagesInput.form.id + '"]');

var MIN_PHOTOS = 5;

var photos = [];
var draggedIndex = null;

function renderGrid() {
    grid.innerHTML = '';

    photos.forEach(function (photo, index) {
        var tile = document.createElement('div');
        tile.className = 'photo-tile';
        tile.draggable = true;

        var img = document.createElement('img');
        img.src = photo.url;
        img.className = 'photo-tile__image';
        tile.appendChild(img);

        if (index === 0) {
            var badge = document.createElement('span');
            badge.className = 'photo-tile__badge';
            badge.textContent = 'Фото обложки';
            tile.appendChild(badge);
        }

        var removeBtn = document.createElement('button');
        removeBtn.type = 'button';
        removeBtn.className = 'photo-tile__remove';
        removeBtn.textContent = '✕';
        removeBtn.addEventListener('click', function () {
            URL.revokeObjectURL(photo.url);
            photos.splice(index, 1);
            renderGrid();
        });
        tile.appendChild(removeBtn);

        tile.addEventListener('dragstart', function () {
            draggedIndex = index;
        });

        tile.addEventListener('dragover', function (e) {
            e.preventDefault();
        });

        tile.addEventListener('drop', function (e) {
            e.preventDefault();
            if (draggedIndex === null || draggedIndex === index) return;

            var moved = photos.splice(draggedIndex, 1)[0];
            photos.splice(index, 0, moved);
            draggedIndex = null;
            renderGrid();
        });

        grid.appendChild(tile);
    });

    var addTile = document.createElement('button');
    addTile.type = 'button';
    addTile.className = 'photo-tile photo-tile--add';
    addTile.textContent = '+';
    addTile.addEventListener('click', function () {
        picker.click();
    });
    grid.appendChild(addTile);

    submitButton.disabled = photos.length < MIN_PHOTOS;
}

picker.addEventListener('change', function () {
    Array.from(picker.files).forEach(function (file) {
        photos.push({ file: file, url: URL.createObjectURL(file) });
    });
    picker.value = '';
    renderGrid();
});

submitButton.addEventListener('click', function () {
    var dataTransfer = new DataTransfer();
    photos.forEach(function (photo) {
        dataTransfer.items.add(photo.file);
    });
    imagesInput.files = dataTransfer.files;
});

renderGrid();
