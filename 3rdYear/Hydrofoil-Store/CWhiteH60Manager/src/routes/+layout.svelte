<script lang="ts">
    import "../app.css";
    import {
        AppRail,
        AppRailTile,
        AppRailAnchor,
    } from "@skeletonlabs/skeleton";
    import {
        Search,
        House,
        Package,
        NotebookText,
        CircleUser,
    } from "lucide-svelte";
    import { page } from "$app/stores";

    let { children } = $props();
</script>

<div class="flex h-full">
    <AppRail>
        <svelte:fragment slot="lead">
            <AppRailAnchor
                href="/"
                title="Home"
                selected={$page.url.pathname === "/"}
            >
                <svelte:fragment slot="lead"><House /></svelte:fragment>
                <span>Home</span>
            </AppRailAnchor>
        </svelte:fragment>
        <!-- --- -->
        <AppRailAnchor
            href="/Products"
            title="Products"
            selected={$page.url.pathname === "/Products"}
        >
            <svelte:fragment slot="lead"><Package /></svelte:fragment>
            <span>Products</span>
        </AppRailAnchor>
        <AppRailAnchor
            href="/Orders"
            title="Orders"
            selected={$page.url.pathname === "/Orders"}
        >
            <svelte:fragment slot="lead"><NotebookText /></svelte:fragment>
            <span>Orders</span>
        </AppRailAnchor>
    </AppRail>
    <main class="w-full p-4">
        {@render children()}
    </main>
</div>
