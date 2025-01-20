<script lang="ts">
    export const ssr = false;
    import { page } from "$app/stores";
    import { Search, SquarePen } from "lucide-svelte";
    import type { Order } from "$lib/types/Order";

    let { data } = $props();
    let { form } = $page;
    let orders: Order[] = form?.data || data.orders || [];

    let filteredOrders = $state<Order[]>(orders);
    let searchParam = $state("");
    let total = $state(0);

    filteredOrders.forEach((order) => {
        total += order.total;
    });

    const searchProducts = () => {
        total = 0;
        const lowercasedSearch = searchParam.toLowerCase();

        filteredOrders = orders.filter((order) => {
            const formatDate = (date: Date | string | null) => {
                if (!date) return null;
                return new Intl.DateTimeFormat("en-US", {
                    year: "numeric",
                    month: "long",
                    day: "numeric",
                }).format(new Date(date));
            };

            const dateCreated = formatDate(order.dateCreated);
            const dateFulfilled = formatDate(order.dateFulfilled);

            return (
                order.customer.email.toLowerCase().includes(lowercasedSearch) ||
                order.customer.firstName
                    ?.toLowerCase()
                    .includes(lowercasedSearch) ||
                order.customer.lastName
                    ?.toLowerCase()
                    .includes(lowercasedSearch) ||
                (dateCreated &&
                    dateCreated.toLowerCase().includes(lowercasedSearch)) ||
                (dateFulfilled &&
                    dateFulfilled.toLowerCase().includes(lowercasedSearch))
            );
        });

        filteredOrders.forEach((order) => {
            total += order.total;
        });
    };
</script>

<div class="flex gap-4 mb-4">
    <h1 class="h1">Orders</h1>
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
                <th>Customer Email</th>
                <th>Date Created</th>
                <th>Date Fulfilled</th>
                <th>Taxes</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            {#each filteredOrders as order}
                <tr data-order-id={order.orderId}>
                    <td class="h-16">
                        <div class="flex items-center justify-between h-full">
                            {order.customer.email}
                        </div>
                    </td>
                    <td class="h-16">
                        <div class="flex items-center justify-between h-full">
                            {new Intl.DateTimeFormat("en-US", {
                                year: "numeric",
                                month: "long",
                                day: "numeric",
                            }).format(new Date(order.dateCreated))}
                        </div>
                    </td>
                    <td class="h-16">
                        <div class="flex items-center justify-between h-full">
                            {#if order.dateFulfilled}
                                {new Intl.DateTimeFormat("en-US", {
                                    year: "numeric",
                                    month: "long",
                                    day: "numeric",
                                }).format(new Date(order.dateFulfilled))}
                            {:else}
                                Not Yet Fulfilled
                            {/if}
                        </div>
                    </td>
                    <td class="h-16">
                        <div class="flex items-center justify-between h-full">
                            ${order.taxes.toFixed(2)}
                        </div>
                    </td>
                    <td class="h-16 text-right">
                        <div class="flex items-center justify-between h-full">
                            ${order.total.toFixed(2)}
                        </div>
                    </td>
                </tr>
            {/each}
        </tbody>
        <tfoot>
            <tr>
                <td>Total</td>
                <td></td>
                <td></td>
                <td></td>
                <td>${total.toFixed(2)}</td>
            </tr>
        </tfoot>
    </table>
</div>
