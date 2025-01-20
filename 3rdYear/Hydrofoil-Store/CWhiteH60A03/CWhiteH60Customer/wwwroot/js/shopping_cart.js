import { Notyf } from "https://cdn.jsdelivr.net/npm/notyf@3.10.0/+esm";

window.addEventListener('DOMContentLoaded', () => {
    let notyf = new Notyf();
    let addToCartForm = document.querySelector('#add-to-cart');
    if (addToCartForm != null) {
        addToCartForm.addEventListener('submit', (e) => {
            e.preventDefault();

            let formData = new FormData(addToCartForm);

            fetch("/AddToCart", {
                method: "POST",
                body: formData,
            })
                .then(response => {
                    if (response.ok) {
                        notyf.success("Added to cart");
                    }
                    else {
                        notyf.error("Failed to add to cart");
                    }
                })
                .catch(error => {
                    notyf.error("Failed to add to cart");
                });
        });
    }
});