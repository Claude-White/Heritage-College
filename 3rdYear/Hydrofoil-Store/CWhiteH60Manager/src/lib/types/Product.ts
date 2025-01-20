type Product = {
    productId: number;
    prodCategory: {
        prodCat: string;
    };
    description: string;
    manufacturer: string;
    stock: number;
    buyPrice: number;
    sellPrice: number;
    notes: string;
};

type SimplifiedProduct = {
    productId: number;
    category: string;
    description: string;
    manufacturer: string;
    stock: number;
    buyPrice: number;
    sellPrice: number;
    notes: string;
};

export type { Product, SimplifiedProduct };
