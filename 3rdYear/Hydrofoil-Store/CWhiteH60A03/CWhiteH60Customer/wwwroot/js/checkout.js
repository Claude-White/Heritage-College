window.openModal = function() {
    let modal = document.getElementById("confirmModal");
    modal.showModal();
}

window.closeModal = function() {
    let modal = document.getElementById("confirmModal");
    modal.close()
}

window.updatePrices = function() {
    let subtotal = document.getElementById("subtotal");
    let tax = document.getElementById("tax");
    let total = document.getElementById("total");
    let province = document.getElementById("provinceSelector");
    let prices = document.querySelectorAll("#cart-item-price");
    
    let skeletons = document.querySelectorAll(".priceSkeletons");
    let labels = document.querySelectorAll(".priceLabels");
    let priceValues = document.querySelectorAll(".priceValues");
    
    skeletons.forEach((skeleton) => {
        skeleton.classList.remove("hidden");
    });

    labels.forEach((label) => {
        label.classList.add("hidden");
    });

    priceValues.forEach((price) => {
        price.classList.add("hidden");
    });
    
    let priceSubtotal = 0;
    prices.forEach((price) => {
        const priceText = price.textContent;
        const numericValue = parseFloat(priceText.replace(/[^0-9.-]/g, ''));
        priceSubtotal += numericValue;
    });
    subtotal.textContent = `$${(priceSubtotal).toLocaleString('en-US', { minimumFractionDigits: 0 })}`;
    fetch(`http://localhost:5115/api/Checkout/CalculateTax/${priceSubtotal}/${province.value}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        }
    })
        .then(async response => {
            if (response.ok) {
                let taxNumericValue = parseInt(await response.text());
                tax.textContent = `$${(taxNumericValue).toLocaleString('en-US', { minimumFractionDigits: 0 })}`;
                total.textContent = `$${(priceSubtotal + taxNumericValue).toLocaleString('en-US', { minimumFractionDigits: 0 })}`;
                skeletons.forEach((skeleton) => {
                    skeleton.classList.add("hidden");
                });

                labels.forEach((label) => {
                    label.classList.remove("hidden");
                });

                priceValues.forEach((price) => {
                    price.classList.remove("hidden");
                });
            }
            else {
                tax = `$${(0).toLocaleString('en-US', { minimumFractionDigits: 0 })}`;
            }
        })
        .catch((error) => {
            console.log(error);
        })
}

document.addEventListener("DOMContentLoaded", function() {
    updatePrices();

    let province = document.getElementById("provinceSelector");
    
    province.addEventListener("change", (e) => {
        updatePrices();
    })
});