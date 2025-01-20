<script lang="ts">
    export const ssr = false;
    import type { SimplifiedProduct } from "$lib/types/Product";
    import { page } from "$app/stores";
    import { Search, SquarePen } from "lucide-svelte";
    import { writable } from "svelte/store";

    let { data } = $props();
    let { form } = $page;
    let products: SimplifiedProduct[] = form?.data || data.products || [];

    type ImageDto = {
        imageName: string;
        imageData: string;
    };

    let filteredProducts = writable<SimplifiedProduct[]>(products);
    let searchParam = $state("");

    const searchProducts = () => {
        const lowercasedSearch = searchParam.toLowerCase();
        filteredProducts.update(() =>
            products.filter((p) => {
                const description = p.description?.toLowerCase() || "";
                const manufacturer = p.manufacturer?.toLowerCase() || "";
                const category = p.category?.toLowerCase() || "";
                const stock = String(p.stock) || "";
                const buyPrice = String(p.buyPrice) || "";
                const sellPrice = String(p.sellPrice) || "";

                return (
                    description.includes(lowercasedSearch) ||
                    manufacturer.includes(lowercasedSearch) ||
                    category.includes(lowercasedSearch) ||
                    stock.includes(lowercasedSearch) ||
                    buyPrice.includes(lowercasedSearch) ||
                    sellPrice.includes(lowercasedSearch)
                );
            })
        );
    };

    const preload = async (src: string): Promise<string> => {
        const resp = await fetch(src);
        if (!resp.ok) {
            throw new Error(
                `Failed to fetch image from ${src}, status: ${resp.status}`
            );
        }
        const blob: ImageDto = await resp.json();
        return `data:image/jpeg;base64,${blob.imageData}`;
    };

    const ModalType = {
        Stock: "Stock",
        BuyPrice: "Buy Price",
        SellPrice: "Sell Price",
    };

    let showModal = $state(false);
    let selectedType = $state("");
    let selectedProductId = $state(0);
    let selectedValue = $state(0);
    let error = $state("");

    const handleSubmit = async () => {
        let requestURL = "";
        let stock = 0;
        let buyPrice = 0;
        let sellPrice = 0;

        if (selectedType === ModalType.Stock) {
            requestURL = `http://localhost:5115/api/Products/Stock/${selectedProductId}`;
            stock = Number(selectedValue) || 0;
        } else if (selectedType === ModalType.BuyPrice) {
            requestURL = `http://localhost:5115/api/Products/Price/${selectedProductId}`;
            buyPrice = Number(selectedValue) || 0;
        } else if (selectedType === ModalType.SellPrice) {
            requestURL = `http://localhost:5115/api/Products/Price/${selectedProductId}`;
            sellPrice = Number(selectedValue) || 0;
        }

        try {
            const res = await fetch(requestURL, {
                method: "PATCH",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    selectedProductId,
                    stock,
                    buyPrice,
                    sellPrice,
                }),
            });

            if (res.ok) {
                await fetchProducts();
                hideModal();
            } else {
                const data = await res.json();
                error = data.Stock?.[0] || data.Price?.[0] || "Update failed.";
            }
        } catch (e) {
            error = "An error occurred. Please try again.";
        }
    };

    const fetchProducts = async () => {
        const res = await fetch("http://localhost:5115/api/Products/Manager");
        const data = await res.json();
        products = data.$values;
        filteredProducts.set(products);
    };

    function editModal(type: string, productId: number, value: number) {
        selectedProductId = productId;
        selectedValue = value;
        if (type == ModalType.Stock) {
            selectedType = ModalType.Stock;
            selectedValue = 0;
        } else if (type == ModalType.BuyPrice) {
            selectedType = ModalType.BuyPrice;
        } else if (type == ModalType.SellPrice) {
            selectedType = ModalType.SellPrice;
        }
        error = "";
        showModal = true;
    }

    function hideModal() {
        showModal = false;
        selectedType = "";
        selectedProductId = 0;
        selectedValue = 0;
        error = "";
    }
</script>

<div class="flex gap-4 mb-4">
    <h1 class="h1">Products</h1>
    <div class="flex justify-start w-full">
        <div class="input-group input-group-divider grid-cols-[auto_1fr_auto]">
            <div class="input-group-shim"><Search /></div>
            <input
                name="search-param"
                type="search"
                placeholder="Search..."
                bind:value={searchParam}
                oninput={() => searchProducts()}
            />
        </div>
    </div>
</div>

<div class="table-container h-[93%] overflow-scroll">
    <table class="table table-hover">
        <thead>
            <tr>
                <th class="hidden lg:table-column"></th>
                <th>Description</th>
                <th>Manufacturer</th>
                <th>Category</th>
                <th class="text-right">Stock</th>
                <th class="text-right">Buy Price</th>
                <th class="text-right">Sell Price</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            {#each $filteredProducts as product}
                <tr data-product-id={product.productId}>
                    <td class="hidden h-16 lg:table-cell">
                        <div class="flex items-center justify-center h-full">
                            {#await preload(`http://localhost:5115/api/Products/Image/${product.productId}`)}
                                <div
                                    class="w-16 placeholder-circle animate-pulse"
                                ></div>
                            {:then base64}
                                <img
                                    class="w-16 h-16"
                                    src={base64}
                                    alt="Product"
                                />
                            {/await}
                        </div>
                    </td>
                    <td class="h-16">
                        <div class="flex items-center justify-between h-full">
                            {product.description}
                        </div>
                    </td>
                    <td class="h-16">
                        <div class="flex items-center justify-between h-full">
                            {product.manufacturer}
                        </div>
                    </td>
                    <td class="h-16">
                        <div class="flex items-center justify-between h-full">
                            {product.category}
                        </div>
                    </td>
                    <td class="h-16">
                        <div class="flex items-center justify-between h-full">
                            {#if product.stock > 0}
                                <span
                                    class="badge variant-filled variant-filled-success"
                                >
                                    In Stock
                                </span>
                            {:else}
                                <span
                                    class="badge variant-filled variant-filled-error"
                                >
                                    Out of Stock
                                </span>
                            {/if}

                            {product.stock}
                        </div>
                    </td>
                    <td class="h-16 text-right">
                        <div class="flex items-center justify-between h-full">
                            <button
                                type="button"
                                onclick={() =>
                                    editModal(
                                        ModalType.Stock,
                                        product.productId,
                                        product.stock
                                    )}
                                class="btn-icon variant-filled variant-soft"
                            >
                                <SquarePen />
                            </button>
                            ${product.buyPrice.toFixed(2)}
                        </div>
                    </td>
                    <td class="h-16 text-right">
                        <div class="flex items-center justify-between h-full">
                            <button
                                type="button"
                                onclick={() =>
                                    editModal(
                                        ModalType.BuyPrice,
                                        product.productId,
                                        product.buyPrice
                                    )}
                                class="btn-icon variant-filled variant-soft"
                            >
                                <SquarePen />
                            </button>
                            ${product.sellPrice.toFixed(2)}
                        </div>
                    </td>
                    <td class="h-16">
                        <div class="flex items-center justify-between h-full">
                            <button
                                type="button"
                                onclick={() =>
                                    editModal(
                                        ModalType.SellPrice,
                                        product.productId,
                                        product.sellPrice
                                    )}
                                class="btn-icon variant-filled variant-soft"
                            >
                                <SquarePen />
                            </button>
                        </div>
                    </td>
                </tr>
            {/each}
        </tbody>
    </table>
</div>

{#if showModal}
    <div class="fixed inset-0 z-50 flex items-center justify-center">
        <div class="p-8 rounded-lg shadow-lg w-96 variant-filled-surface">
            <h3>Update {selectedType}</h3>
            <input
                type="hidden"
                class="hidden"
                name="type"
                value={selectedType}
            />
            <input
                type="hidden"
                class="hidden"
                name="productId"
                value={selectedProductId}
            />
            <input
                class="input"
                bind:value={selectedValue}
                placeholder={`Enter new ${selectedType}`}
            />
            {#if error}
                <p class="error">{error}</p>
            {/if}
            <div class="flex gap-2 pt-4">
                <button
                    type="button"
                    onclick={() => hideModal()}
                    class="w-full btn variant-ghost">Cancel</button
                >
                <button
                    onclick={handleSubmit}
                    class="w-full btn variant-filled-primary"
                >
                    Save
                </button>
            </div>
        </div>
    </div>
{/if}
