import { initializeBlobs } from './blob_generator.js';

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById('searchForm').addEventListener('submit', async function(e) {
        e.preventDefault();
        let searchInput = document.getElementById("searchInput");
        let searchInputHeader = document.getElementById("searchInputHeader");
        searchInputHeader.value = searchInput.value;
        let partialContent = document.getElementById("partialContent");
        fetch(`/Product/ReloadProductsPartial?productName=${encodeURIComponent(searchInput.value)}`)
            .then(response => response.json())
            .then(json => {
                partialContent.innerHTML = json.productList;
                document.getElementById('productCount').textContent = `Results: ${json.productCount || 0}`;
                initializeBlobs();
            })
            .catch(error => console.error("Error loading product list:", error));
    });
    document.getElementById('searchFormHeader').addEventListener('submit', async function(e) {
        e.preventDefault();
        let searchInput = document.getElementById("searchInput");
        let searchInputHeader = document.getElementById("searchInputHeader");
        searchInput.value = searchInputHeader.value;
        let partialContent = document.getElementById("partialContent");
        fetch(`/Product/ReloadProductsPartial?productName=${encodeURIComponent(searchInputHeader.value)}`)
            .then(response => response.json())
            .then(json => {
                partialContent.innerHTML = json.productList;
                document.getElementById('productCount').textContent = `Results: ${json.productCount || 0}`;
                initializeBlobs();
            })
            .catch(error => console.error("Error loading product list:", error));
    });
});