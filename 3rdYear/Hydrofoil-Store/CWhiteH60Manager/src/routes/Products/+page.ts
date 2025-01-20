import type { PageLoad } from "./$types";
import type { Product, SimplifiedProduct } from "$lib/types/Product";

export const ssr = false;

export const load = (async ({ fetch }) => {
    const { $values: products } = await fetch(
        "http://localhost:5115/api/Products/Manager"
    ).then((res) => res.json());
    const simplifiedProducts: SimplifiedProduct[] = products.map(
        (product: Product) => ({
            productId: product.productId,
            category: product.prodCategory.prodCat,
            description: product.description,
            manufacturer: product.manufacturer,
            stock: product.stock,
            buyPrice: product.buyPrice,
            sellPrice: product.sellPrice,
            notes: product.notes,
        })
    );
    return {
        products: simplifiedProducts,
    };
}) satisfies PageLoad;
