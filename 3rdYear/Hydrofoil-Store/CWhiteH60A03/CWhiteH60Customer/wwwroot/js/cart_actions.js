window.removeCartItem = function (itemId) {
    fetch(`/RemoveFromCart/${itemId}`, {
        method: "GET",
    })
    .then(response => {
        if (response.ok) {
            const itemRow = document.querySelector(`[data-item-id="${itemId}"]`);
            if (itemRow) {
                itemRow.remove();
            }
            notyf.success("Item removed");
        }
        else {
            notyf.error("Failed to remove item");
        }
    })
    
    .catch(error => {
        notyf.error("Failed to remove item");
    });
}

const cartItemUpdates = {};

window.incrementCartItem = function (itemId, change, price) {
    if (!cartItemUpdates[itemId]) {
        cartItemUpdates[itemId] = {
            quantity: 0,
            lastUpdateTime: 0
        };
    }
    
    const itemRow = document.querySelector(`[data-item-id="${itemId}"]`);
    const itemQuantity = itemRow.querySelector('#cart-item-quantity');
    const itemPrice = itemRow.querySelector('#cart-item-price');
    
    cartItemUpdates[itemId].quantity += change;
    cartItemUpdates[itemId].lastUpdateTime = Date.now();

    // Optimistic UI change
    let currentQuantity = parseInt(itemQuantity.textContent);
    let newQuantity = currentQuantity + change;
    
    if (newQuantity < 0) {
        cartItemUpdates[itemId].quantity = 0;
        return;
    }
    
    itemQuantity.textContent = newQuantity.toString();
    itemPrice.textContent = `$${(newQuantity * price).toLocaleString('en-US', { minimumFractionDigits: 0 })}`;
    
    scheduleCartUpdate();
};

function showLoader() {
    const loaderOverlay = document.createElement('div');
    loaderOverlay.id = 'cart-update-loader';
    loaderOverlay.style.position = 'fixed';
    loaderOverlay.style.top = '0';
    loaderOverlay.style.left = '0';
    loaderOverlay.style.width = '100%';
    loaderOverlay.style.height = '100%';
    loaderOverlay.style.backgroundColor = 'rgba(0, 0, 0, 0.2)';
    loaderOverlay.style.display = 'flex';
    loaderOverlay.style.justifyContent = 'center';
    loaderOverlay.style.alignItems = 'center';
    loaderOverlay.style.zIndex = '9999';
    
    const spinner = document.createElement('span');
    spinner.className = 'loading loading-spinner loading-lg';

    loaderOverlay.appendChild(spinner);
    document.body.appendChild(loaderOverlay);
}

function removeLoader() {
    const loader = document.getElementById('cart-update-loader');
    if (loader) {
        loader.remove();
    }
}

function scheduleCartUpdate() {
    if (window.cartUpdateTimeout) {
        clearTimeout(window.cartUpdateTimeout);
    }
    
    window.cartUpdateTimeout = setTimeout(() => {
        const updatesToSend = Object.entries(cartItemUpdates)
            .filter(([_, update]) =>
                update.quantity !== 0 &&
                Date.now() - update.lastUpdateTime >= 750
            );

        // Reset tracking if no updates
        if (updatesToSend.length === 0) {
            Object.keys(cartItemUpdates).forEach(key => {
                cartItemUpdates[key].quantity = 0;
            });
            return;
        }
        
        //showLoader();
        
        const batchUpdatePayload = updatesToSend.map(([itemId, update]) => ({
            itemId: parseInt(itemId),
            quantityChange: update.quantity
        }));
        
        let specialError = null;
        fetch('http://localhost:5115/api/CartItem/BatchUpdateQuantity', {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(batchUpdatePayload)
        })
            .then(async response => {
                if (!response.ok) {
                    throw new Error('Batch update failed');
                }

                const errorMessage = await response.text();
                if (errorMessage) {
                    specialError = errorMessage;
                    throw new Error(errorMessage);
                }
                else {
                    updatePrices();
                    notyf.success('Cart updated successfully');
                }

                // Reset quantities on success
                updatesToSend.forEach(([itemId]) => {
                    cartItemUpdates[itemId].quantity = 0;
                });
            })
            .catch(error => {
                if (specialError) {
                    notyf.error(specialError);
                }
                else {
                    notyf.error('Failed to update cart quantities');
                }

                // Revert changes if error
                updatesToSend.forEach(([itemId, update]) => {
                    const itemRow = document.querySelector(`[data-item-id="${itemId}"]`);
                    const itemQuantity = itemRow.querySelector('#cart-item-quantity');
                    const itemPrice = itemRow.querySelector('#cart-item-price');

                    // Revert quantity and price
                    const currentQuantity = parseInt(itemQuantity.textContent) - update.quantity;
                    itemQuantity.textContent = currentQuantity.toString();

                    const price = parseFloat(itemPrice.textContent.replace('$', '').replace(',', '')) / (currentQuantity + update.quantity);
                    itemPrice.textContent = `$${(currentQuantity * price).toLocaleString('en-US', { minimumFractionDigits: 0 })}`;
                });
            })
            .finally(() => {
                //removeLoader();
            });
    }, 750);
}