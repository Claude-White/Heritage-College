// initialize constants
const paginationContainer = document.querySelectorAll(".content-row")
const paginationDiv = document.querySelector("#paginationDiv")
const attributes = ['btn', 'join-item', 'btn-secondary']
const paginationLimit = 8;
const pageCount = Math.ceil(paginationContainer.length / paginationLimit);
let currentPage;

//creates the buttons for pagination
const addNewPageNum = (index) => {
    pageBtn = document.createElement('button')

    attributes.forEach(item => {
        pageBtn.classList.add(item)
    });

    pageBtn.classList.add('page-list', index)
    pageBtn.textContent = index

    paginationDiv.appendChild(pageBtn)

}

const getPaginationDivs = () => {

    prevBtn = document.createElement('button')
    nextBtn = document.createElement('button')

    prevBtn.textContent = "«"
    nextBtn.textContent = "»"

    attributes.forEach(item => {
        prevBtn.classList.add(item)
        nextBtn.classList.add(item)
    });

    if (paginationContainer.length != 0) {
        paginationDiv.appendChild(prevBtn)
    }

    for (let i = 1; i <= pageCount; i++) {
        addNewPageNum(i);
    }
    if (paginationContainer.length != 0) {
        paginationDiv.appendChild(nextBtn)
    }

}


const setCurrentPage = (pageNum) => {
    currentPage = pageNum
    const prevRange = (pageNum - 1) * paginationLimit;
    const currRange = (pageNum) * paginationLimit;
    paginationContainer.forEach((item, index) => {
        item.classList.add("hidden");

        if (index >= prevRange && index < currRange) {
            item.classList.remove("hidden");
        }
    });


};

// toggle between diabled and enabled buttons back/next/active buttons
const disableButton = (button) => {
    button.setAttribute("disabled", true);
};
const enableButton = (button) => {
    button.removeAttribute("disabled");
};

const activeButton = (button) => {
    button.classList.remove('btn-secondary')
    button.classList.add('btn-primary')

}

const inactiveButton = (button) => {
    button.classList.remove('btn-primary')
    button.classList.add('btn-secondary')

}

const removePageBtn = (index) => {
    const lastPageBtn = document.querySelector('.btn.join-item.page-list:last-child');
    if (lastPageBtn) {
        lastPageBtn.parentNode.removeChild(lastPageBtn);
    } else {
        console.error(`Last page button not found.`);
    }
    location.reload(true);

}

const continuousPaginationCheck = () => {
    setInterval(removeEmptyPages, 1000);
};

const removeEmptyPages = () => {
    const visibleItems = document.querySelectorAll('.content-row:not(.hidden)').length;
    if (visibleItems === 0 && currentPage !== 1) {
        const prevPage = currentPage - 1;
        removePageBtn(currentPage);
        setCurrentPage(prevPage);
    }
};

const handlePageButtonsStatus = () => {
    if (currentPage === 1) {
        disableButton(prevBtn);
    } else {
        enableButton(prevBtn);
    }
    if (pageCount === currentPage) {
        disableButton(nextBtn);
    } else {
        enableButton(nextBtn);
    }
};

window.addEventListener("load", () => {
    getPaginationDivs();
    let firstBtn = document.querySelector('.btn.join-item.btn-secondary.page-list');
    setCurrentPage(1);
    activeButton(firstBtn)
    handlePageButtonsStatus();
    continuousPaginationCheck();
    allPaginationPages = document.querySelectorAll("#paginationDiv .page-list");


    prevBtn.addEventListener("click", () => {
        if (currentPage > 1) {
            const prevBtn = document.querySelector('.btn.join-item.page-list:nth-child(' + (currentPage) + ')');
            const currBtn = document.querySelector('.btn.join-item.page-list:nth-child(' + (currentPage + 1) + ')');

            setCurrentPage(currentPage - 1);
            handlePageButtonsStatus();
            activeButton(prevBtn);
            if (currBtn) {
                inactiveButton(currBtn);
            }
        }
    });

    nextBtn.addEventListener("click", () => {
        if (currentPage < pageCount) {
            const currBtn = document.querySelector('.btn.join-item.page-list:nth-child(' + (currentPage + 1) + ')');
            const nextBtn = document.querySelector('.btn.join-item.page-list:nth-child(' + (currentPage + 2) + ')');

            setCurrentPage(currentPage + 1);
            handlePageButtonsStatus();
            activeButton(nextBtn);
            if (currBtn) {
                inactiveButton(currBtn);
            }
        }
    });

    //page buttons
    allPaginationPages.forEach((button) => {
        const pageIndex = Number(button.textContent);

        if (pageIndex) {
            button.addEventListener("click", () => {
                setCurrentPage(pageIndex);
                handlePageButtonsStatus();
                activeButton(button);
                allPaginationPages.forEach((btn) => {
                    if (btn !== button) {
                        inactiveButton(btn);
                    }
                });
            });
        }
    });
});
