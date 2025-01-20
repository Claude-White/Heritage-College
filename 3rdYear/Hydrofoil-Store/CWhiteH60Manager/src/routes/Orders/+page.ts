import type { PageLoad } from "./$types";
import type { Order } from "$lib/types/Order";

export const ssr = false;

export const load = (async ({ fetch }) => {
    const { $values: orders } = await fetch(
        "http://localhost:5115/api/Order"
    ).then((res) => res.json());
    const typedOrders: Order[] = orders;
    return {
        orders: typedOrders,
    };
}) satisfies PageLoad;
