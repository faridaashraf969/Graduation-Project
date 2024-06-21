// Get the modal
var modal = document.getElementById("modal");

// Get the image and insert it inside the modal - use its "alt" text as a caption
var modalImg = document.getElementById("modal-img");

function openModal(img) {
    modal.style.display = "block";
    modalImg.src = img.src;
}

// When the user clicks on <span> (x), close the modal
function closeModal() {
    modal.style.display = "none";
}