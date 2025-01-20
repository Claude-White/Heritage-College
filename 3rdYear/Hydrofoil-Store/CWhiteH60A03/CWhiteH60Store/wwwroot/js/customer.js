async function searchCustomer() {
    const input = document.querySelector('#search-input').value;
    let endpoint = "";
    
    if (input) {
        endpoint = `http://localhost:5115/api/Customers?param=${input}`;
    }
    else {
        endpoint = "http://localhost:5115/api/Customers";
    }
    
    const response = await fetch(endpoint, { method: 'GET' });
    
    if (response.ok) {
        const data = await response.json();
        updateTable(data);
    } else {
        console.error('Failed to fetch customer data');
    }
}

function updateTable(customers) {
    if (customers.$values) {
        customers = customers.$values;
    }
    const tbody = document.querySelector('#customer-table-body');
    tbody.innerHTML = '';

    if (customers.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td>
                    <h4 class="text-lg">No customers</h4>
                </td>
            </tr>
        `;
        return;
    }

    customers.forEach(customer => {
        const row = document.createElement('tr');
        row.classList.add('hover');
        row.innerHTML = `
            <td class="hidden md:table-cell">
                ${customer.firstName ?? ""}
            </td>
            <td class="hidden md:table-cell">
                ${customer.lastName ?? ""}
            </td>
            <td class="hidden sm:table-cell">
                ${customer.email}
            </td>
            <td class="hidden lg:table-cell">
                ${customer.phoneNumber ?? ""}
            </td>
            <td class="hidden lg:table-cell">
                ${customer.province ?? ""}
            </td>
            <td>
                <div class="join shadow-sm">
                    <a class="btn btn-sm join-item" href="/Customer/Edit/${customer.customerId}">Edit</a>
                    <a class="btn btn-sm hover:btn-neutral join-item" href="/Customer/Details/${customer.customerId}">Details</a>
                    <button class="btn btn-sm hover:btn-error join-item" onclick="showDeleteModal(${customer.customerId})">Delete</button>
                </div>
            </td>
        `;
        tbody.appendChild(row);
    });
}

function showDeleteModal(customerId) {
    const modal = document.querySelector('#delete-modal');
    const deleteAction = document.querySelector('#delete-action');
    deleteAction.innerHTML = `
        <button class="btn btn-info hover:btn-error" onclick="deleteCustomer(${customerId})">Confirm</button>
    `;
    modal.showModal();
}

async function deleteCustomer(customerId) {
    const response = await fetch(`http://localhost:5115/api/Customers/${customerId}`, { method: 'DELETE' });
    if (response.ok) {
        console.log("Customer deleted");
        await searchCustomer();
        const modal = document.querySelector('#delete-modal');
        modal.close();
    }
}