function showImageModal(imageSource) {
    // Create a new image element
    var img = new Image();

    // Set the source of the image
    img.src = imageSource;

    // Set the CSS styles for the image to make it full-sized
    img.style.maxWidth = "100%";
    img.style.maxHeight = "100%";
    img.style.objectFit = "contain";

    // Create a modal container
    var modalContainer = document.createElement("div");
    modalContainer.className = "modal";
    modalContainer.style.display = "block";
    modalContainer.style.position = "fixed";
    modalContainer.style.zIndex = "9999";
    modalContainer.style.top = "0";
    modalContainer.style.left = "0";
    modalContainer.style.width = "100%";
    modalContainer.style.height = "100%";
    modalContainer.style.backgroundColor = "rgba(0,0,0,0.9)";
    modalContainer.style.overflowY = "auto";
    modalContainer.style.textAlign = "center";
    modalContainer.style.paddingTop = "10%";

    // Add the image element to the modal container
    modalContainer.appendChild(img);

    // Add a click event listener to the modal container to close it when clicked
    modalContainer.addEventListener("click", function () {
        modalContainer.parentNode.removeChild(modalContainer);
    });

    // Append the modal container to the document body
    document.body.appendChild(modalContainer);
}