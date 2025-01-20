import blobshape from 'https://cdn.jsdelivr.net/npm/blobshape@1.0.0/+esm';

export function getBlobSVG() {
    const { path } = blobshape({ size: 100, growth: 8, edges: 5, seed: null });
    return `<svg class="fill-[#86b9eb] opacity-15" viewBox="0 0 100 100"><path class="shadow group-hover:rotate-45 group-hover:scale-110 transition-transform duration-300 transform origin-center" d="${path}" /></svg>`;
}

export function initializeBlobs() {
    document.querySelectorAll('.blob-container').forEach(container => {
        const blobSVG = getBlobSVG();
        container.innerHTML = blobSVG;
    });
}

document.addEventListener("DOMContentLoaded", initializeBlobs);
