$(document).ready(function () {

    // Initialize modal safely
    let quantityModal = null;
    try {
        const modalElement = document.getElementById('quantityModal');
        if (modalElement) {
            quantityModal = new bootstrap.Modal(modalElement);
        }
    } catch (e) {
        toastr.error('Error initializing modal:', e);
    }

    let cart = [];
    let discounts = [];
    let itemTypes = [];
    let items = [];
    let branches = [];
    let currentBranchId = 0;
    let customers = [];
    let currentCustomerId = 0;
    let currentUser = null;


    // Initialize the POS system
    function initPOS() {
        fetchBranches()
            .then(() => fetchCustomers())
            .then(() => fetchCurrentUser())
            .then(() => fetchActiveDiscounts())
            .then(() => updateCartDisplay())
            .then(() => fetchItemTypes())
            .then(() => fetchItems(0, currentBranchId)) // first load: All Items for the first/only branch
            .catch(err => console.error('Init error:', err));
    }

    // Fetch current user
    function fetchCurrentUser() {
        return $.ajax({
            url: '/api/Sales/PosApi/user',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log("User received:", data);
                currentUser = data;
                initUser();
            },
            error: function (xhr, status, error) {
                toastr.error('Error fetching user: ' + error);
                console.error(xhr.responseText);
            }
        });
    }

    // Initialize cashier UI
    function initUser() {
        const $cashierSpan = $('#cashier');
        $cashierSpan.empty();

        if (currentUser) {
            $cashierSpan.text("Cashier: " + currentUser.name);
        } else {
            $cashierSpan.text("Cashier: (unknown)");
        }
    }

    function getActiveTypeId() {
        return parseInt($('.type-chip.active').data('type-id') || 0, 10);
    }

    // Start the POS system
    initPOS();

    // Fetch Customers from API
    function fetchCustomers() {
        return $.ajax({
            url: '/api/Sales/PosApi/customers',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log("Customers received:", data);
                customers = data;
                if (currentCustomerId == 0) {
                    currentCustomerId = customers[0].id;
                }
                initCustomers();
            },
            error: function (xhr, status, error) {
                toastr.error('Error fetching customers:', error, xhr.responseText);
            }
        });
    }

    // Initialize Customers dropdown
    function initCustomers() {
        const $customersList = $('#customersList');
        $customersList.empty();

        if (customers && customers.length > 0) {
            customers.forEach(customer => {
                $customersList.append($('<option>', {
                    value: customer.id,
                    text: customer.name
                }));
            });
        }

        // Add "Add New Customer" at the end
        $customersList.append($('<option>', {
            value: 'add-new',
            text: '➕ Add New Customer'
        }));
    }

    // When dropdown is clicked, fetch latest customers
    $(document).on('focus', '#customersList', function () {
        fetchCustomers();
    });

    // Handle customer selection
    $(document).on('change', '#customersList', function () {
        const selectedValue = $(this).val();
        if (selectedValue === 'add-new') {
            // Open in new tab
            window.open('/administrative/partner/save?id=0', '_blank');

            // Reset select back to "Anonymous" or empty
            $(this).val(0);
        } else {
            currentCustomerId = selectedValue;
            console.log("Selected customer ID:", selectedValue);
        }
    });

    // Fetch branches from API on page load
    function fetchBranches() {
        return $.ajax({
            url: '/api/Sales/PosApi/branches',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log("Branches received:", data);
                branches = data;
                initBranches();
            },
            error: function (xhr, status, error) {
                toastr.error('Error fetching branches:', error, xhr.responseText);
            }
        });
    }

    // Load branches list in the page 
    function initBranches() {
        const $branchesList = $('#branchesList');
        $branchesList.empty();

        if (branches && branches.length > 0) {
            branches.forEach(branch => {
                $branchesList.append($('<option>', {
                    value: branch.id,
                    text: branch.name
                }));
            });

            // Select first (or only) branch
            $branchesList.val(branches[0].id);
            currentBranchId = branches[0].id;

            if (branches.length === 1) {
                $branchesList.addClass('single-branch');
            }

            // Optional: trigger change (if you prefer this pattern)
            // $branchesList.trigger('change');
        } else {
            $branchesList.append($('<option>', {
                value: '',
                text: 'No branches available',
                disabled: true,
                selected: true
            }));
        }
    }

    // Reload items when the branch changes
    $('#branchesList').on('change', function () {
        cart = [];
        updateCartDisplay();
        currentBranchId = parseInt($(this).val(), 10);
        const typeId = getActiveTypeId(); // keep current type filter
        fetchItems(typeId, currentBranchId);
    });


    // Fetch item types from API on page load
    function fetchItemTypes() {
        return $.ajax({
            url: '/api/Sales/PosApi/itemtypes',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log("Item types received:", data);
                itemTypes = data;
                initItemTypes();
            },
            error: function (xhr, status, error) {
                toastr.error('Error fetching item types:', error, xhr.responseText);
            }
        });
    }

    // Initialize item types
    function initItemTypes() {
        const $typesRow = $('.item-types-row');
        $typesRow.empty();

        const allItemsButton = $('<button class="type-chip active" data-type-id="0">All Items</button>');
        $typesRow.append(allItemsButton);

        itemTypes.forEach(type => {
            const $button = $(`<button class="type-chip" data-type-id="${type.id}">${type.name}</button>`);
            $typesRow.append($button);
        });
    }


    // Item type click handler
    $(document).on('click', '.type-chip', function () {
        $('.type-chip').removeClass('active');
        $(this).addClass('active');

        const typeId = parseInt($(this).data('type-id'), 10) || 0;
        fetchItems(typeId, currentBranchId);
    });

    // Fetch items based on type (you need to implement this API endpoint)
    function fetchItems(typeFilter, currentBranchId) {
        return $.ajax({
            url: `/api/Sales/posapi/items?typeId=${typeFilter}&branchId=${currentBranchId}`,
            method: 'GET',
            success: function (data) {
                console.log("Items received:", data);
                items = data;
                renderItems();
            },
            error: function (xhr, status, error) {
                toastr.error('Error fetching items:', error, xhr.responseText);
            }
        });
    }

    // Render items based on type or search
    function renderItems(filteredItems = items) {
        const $itemsGrid = $('.items-grid');
        $itemsGrid.empty();

        if (!filteredItems) filteredItems = [];
        if (!Array.isArray(filteredItems)) filteredItems = [filteredItems];

        if (filteredItems.length === 0) {
            $itemsGrid.html('<div class="empty-state"><i class="fas fa-box-open fa-3x mb-3"></i><p>No items found</p></div>');
            return;
        }

        filteredItems.forEach(item => {
            const hasDiscount = (typeof item.discountPrice === 'number') && item.discountPrice < item.price;
            const displayPrice = hasDiscount ? item.discountPrice : item.price;
            const discountRate = item.discountRate ?? 0;
            const inStock = typeof item.quantity === 'number' && item.quantity > 0;
            const qtyText = typeof item.quantity === 'number' ? item.quantity : '0';

            let mediaContent;
            if (item.imageName) {
                const imageUrl = `/Images/${encodeURIComponent(item.imageName)}`;
                mediaContent = `<img src="${imageUrl}" alt="${item.name}" loading="lazy" class="item-img" />`;
            } else {
                mediaContent = `<div class="no-image-text">${item.name}</div>`;
            }

            const $itemCard = $(`
            <div class="item-card" data-item-id="${item.id}">
                <div class="item-card-media">
                    ${mediaContent}
                    ${hasDiscount ? `<div class="badge discount-badge">-${discountRate}%</div>` : ''}
                </div>
                <div class="item-name-single text-truncate">${item.name}</div>
                <div class="item-card-body">
                    <div class="price-single">${displayPrice.toFixed(2)} EGP</div>
                </div>
            </div>
        `);

            $itemsGrid.append($itemCard);
        });
    }




    // Barcode & Name search showing the item card
    $('#barcodeOrNameInput').on('keypress', function (e) {
        if (e.which === 13) { // Enter key 
            const barcodeOrName = $(this).val().trim();
            if (!barcodeOrName) return;

            // Case-insensitive partial name search + exact barcode search
            const matchedItems = items.filter(i =>
                i.barcode == barcodeOrName ||
                i.name.toLowerCase().includes(barcodeOrName.toLowerCase())
            );

            if (matchedItems.length > 0) {
                renderItems(matchedItems);
            } else {
                toastr.error("No items found for: " + barcodeOrName);
            }
        }
    });


    // Adding item to cart through clicking item card
    $(document).on('click', '.item-card', function () {
        const itemId = $(this).data('item-id');
        const item = items.find(i => i.id == itemId);
        handleAddItem(item);
    });

    // View add item to cart pop-up (Quantity Modal)
    function handleAddItem(item) {
        if (item) {
            if (quantityModal) {
                // Use Bootstrap modal if available 
                $('#modalItemName').text(`${item.name}${item.type ? " (" + item.type + ")" : ""}`);
                if (item.price == item.discountPrice) {
                    $('#modalItemPrice').text(item.price.toFixed(2) + ' EGP');
                }
                else {
                    $('#modalItemPrice').text(item.discountPrice.toFixed(2) + ' EGP');
                }
                $('#quantityInput')
                    .val(1) // default value
                    .attr("max", item.quantity); // set max available quantity

                // Show available stock
                $('#modalAvailableQty').text(`${item.quantity} Items`);

                updateModalTotal();

                // Store the current item in the modal for later use 
                $('#quantityModal').data('current-item', item);

                quantityModal.show();
            }
            // CHECK ITS USAGE
            else {
                // Fallback to prompt if modal is not available 
                const quantity = parseInt(prompt(`Enter quantity for ${item.name} (max ${item.quantity}):`, '1'), 10);
                if (isNaN(quantity) || quantity < 1 || quantity > item.quantity) {
                    console.warn(`⚠ Quantity exceeds stock for item ${item.name}. Max: ${item.quantity}`);
                    return toastr.error(`Max available quantity for ${currentItem.name} is ${currentItem.quantity}`);
                }

                const existingItemIndex = cart.findIndex(cartItem => cartItem.id === item.id);
                if (existingItemIndex !== -1) {
                    cart[existingItemIndex].quantity += quantity;
                } else {
                    cart.push({
                        id: item.id,
                        name: item.name,
                        price: item.price,
                        quantity: quantity
                    });
                }
                updateCartDisplay();
            }
        }
    }


    // Quantity input change handler
    $('#quantityInput').on('input', function () {
        const currentItem = $('#quantityModal').data('current-item');
        let val = parseInt($(this).val()) || 1;

        if (val < 1) val = 1;
        const cartItem = cart.find(item => item.id === currentItem.id);
        const existingQty = cartItem?.quantity ?? 0;

        if (currentItem && val + existingQty > currentItem.quantity) {
            console.warn(`⚠ Entered quantity (${val}) exceeds stock for "${currentItem.name}". Max: ${currentItem.quantity}`);
            toastr.error(`Max available quantity for "${currentItem.name}" is "${currentItem.quantity} items"`);
            val = currentItem.quantity - existingQty;
        }

        $(this).val(val);
        updateModalTotal();
    });

    // Update modal total price
    function updateModalTotal() {
        const quantity = parseInt($('#quantityInput').val()) || 1;
        const priceText = $('#modalItemPrice').text();
        const price = parseFloat(priceText.replace(' EGP', ''));
        const total = quantity * price;
        $('#modalItemTotal').text(total.toFixed(2) + ' EGP');
    }

    // Press Enter inside quantity input = click "Add to Cart"
    $('#quantityInput').on('keypress', function (e) {
        if (e.which === 13) { // Enter key
            e.preventDefault(); // prevent form submit or weird behavior
            $('#addToCartBtn').click(); // trigger AddToCart logic
        }
    });


    // Add to cart button handler
    $('#addToCartBtn').click(function () {
        const item = $('#quantityModal').data('current-item');
        const quantity = parseInt($('#quantityInput').val()) || 1;

        if (item) {
            // Check if item already in cart
            const existingItemIndex = cart.findIndex(cartItem => cartItem.id === item.id);

            if (existingItemIndex !== -1) {
                // Update quantity if item exists && Quantity entered <= Available
                if (cart[existingItemIndex].quantity + quantity <= item.quantity)
                    cart[existingItemIndex].quantity += quantity;
                else {
                    console.warn(`Adding the item's quantity in cart, you exceeded the maximum available quantity: "${item.quantity} items" for "${item.name}"`);
                    toastr.error(`Adding the item's quantity in cart, you exceeded the maximum available quantity: "${item.quantity} items" for "${item.name}"`);
                }
            } else {
                // Add new item to cart
                cart.push({
                    id: item.id,
                    name: item.name,
                    price: item.price,
                    quantity: quantity,
                    available: item.quantity,
                    discountPrice: item.discountPrice,
                    discountRate: item.discountRate
                });
                updateDiscountsDisplay();
            }

            updateCartDisplay();
            if (quantityModal) {
                quantityModal.hide();
            }
        }
    });

    // Update cart display
    function updateCartDisplay() {
        const $cartItems = $('#cartItems');
        $cartItems.empty();
        let totalQty = 0;
        let totalAmount = 0;

        if (cart.length === 0) {
            $cartItems.html(` 
            <div class="empty-state"> 
                <i class="fas fa-shopping-cart fa-3x mb-3"></i> 
                <p>No items in cart</p> 
            </div> 
        `);
        }
        else {
            cart.forEach(item => {
                const itemPrice = (item.price == item.discountPrice) ? item.price : item.discountPrice;
                const itemTotal = itemPrice * item.quantity;
                totalQty += item.quantity;
                totalAmount += itemTotal;

                $cartItems.append(` 
                <div class="cart-item"> 
                    <div class="cart-item-details"> 
                        <div class="fw-semibold text-truncate item-name">${item.name}</div> 
                        ${item.price == item.discountPrice
                        ? `<div class="text-muted small item-price">${item.price.toFixed(2)} EGP</div>`
                        : `<div class="text-muted small strikethrough-price">${item.price.toFixed(2)} EGP</div>
                               <div class="text-muted small item-price">${item.discountPrice.toFixed(2)} EGP</div>`
                    }
                    </div> 
                    <div class="cart-item-controls"> 
                        <input type="number" class="form-control quantity-input" 
                            value="${item.quantity}" min="1" max="${item.available}" data-id="${item.id}"> 
                        <div class="text-end" style="width: 80px;"> 
                            <div class="small text-muted">EGP</div> 
                            <div class="fw-semibold">${itemTotal.toFixed(2)}</div> 
                        </div> 
                        <button class="btn btn-sm btn-outline-danger remove-item" data-id="${item.id}"> 
                            <i class="fas fa-times"></i> 
                        </button> 
                    </div> 
                </div>`);
            });

            // Event handlers
            $('.quantity-input').on('change', function () {
                const id = $(this).data('id');
                let newQty = parseInt($(this).val()) || 1;
                if (newQty < 1) newQty = 1;
                const item = cart.find(item => item.id === id);
                if (item && newQty > item.available) {
                    toastr.error(`Max available quantity for "${item.name}" is "${item.available}"`);
                    newQty = item.available;
                    $(this).val(newQty); // force correct value in UI
                }
                if (item) {
                    item.quantity = newQty;
                }
                updateCartDisplay();
            });

            $('.remove-item').click(function () {
                const id = $(this).data('id');
                cart = cart.filter(item => item.id !== id);
                updateCartDisplay();
            });
        }

        $('#totalQty').text(totalQty);
        $('#totalAmount').text(totalAmount.toFixed(2));
        updateTotals();
    }

    // Update totals
    function updateTotals() {
        // Calculate subtotal
        let subtotal = 0;

        cart.forEach(item => {
            if (item.price == item.discountPrice)
                subtotal += item.price * item.quantity;
            else
                subtotal += item.discountPrice * item.quantity;
        });

        // Calculate discount amount
        let discountRate = 0;
        let discountAmount = 0;

        discounts.forEach(discount => {
            discountRate += discount.rate;
        });

        if (discountRate > 0) {
            discountAmount = subtotal * (discountRate / 100);
        }

        // Update discount display
        $('#totalDiscountRate').text(discountRate.toFixed(2) + '%');
        $('#totalDiscountAmount').text(discountAmount.toFixed(2));

        // Calculate grand total with ceiling rounding
        const grandTotal = subtotal - discountAmount;
        const roundedTotal = Math.ceil(grandTotal);

        // Update totals display
        $('#grandTotal').text(grandTotal.toFixed(2) + ' EGP');
        $('#roundedTotal').text(roundedTotal.toFixed(2) + ' EGP');
    }

    // Clear cart button
    $('#clearCartBtn').click(function () {
        cart = [];
        updateCartDisplay();
    });

    /*    // Clear discounts button
        $('#clearDiscountBtn').click(function () {
            discounts = [];
            updateDiscountsDisplay();
        });*/

    // Fetch Array of Active Discounts from Db
    function fetchActiveDiscounts() {
        return $.ajax({
            url: '/api/Sales/PosApi/discounts',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log("Active Discounts received:", data);
                discounts = data;
                updateDiscountsDisplay();
            },
            error: function (xhr, status, error) {
                toastr.error('Error fetching discounts:', error, xhr.responseText);
            }
        });
    }

    // Update discounts display
    function updateDiscountsDisplay() {
        const $discountsList = $('#discountsList');
        $discountsList.empty();

        if (discounts.length === 0) {
            $discountsList.html(`
                <div class="empty-state">
                    <i class="fas fa-tag fa-3x mb-3"></i>
                    <p>No discounts applied</p>
                </div>
            `);
        } else {
            // General Discounts
            discounts.forEach(discount => {
                $discountsList.append(`
                    <div class="discount-item">
                        <div class="text-truncate">${discount.name}</div>
                        <div class="d-flex align-items-center">
                            <span class="text-success me-2">${discount.rate}%</span>
                        </div>
                    </div>
                `);
            });
            /*            <button class="btn btn-sm btn-outline-danger remove-discount" data-id="${discount.id}">
                            <i class="fas fa-times"></i>
                        </button>*/
            /*            // Add remove buttons
                        $('.remove-discount').click(function () {
                            const id = $(this).data('id');
                            discounts = discounts.filter(d => d.id !== id);
                            updateDiscountsDisplay();
                        });*/
        }
        updateTotals();
    }

    // Utility: format currency
    function fmtEGP(v) {
        return Number(v).toFixed(2) + ' EGP';
    }

    // When checkout button is clicked
    $(document).on('click', '.checkout-btn', function () {
        if (!cart || cart.length === 0) {
            toastr.warning('Cart is empty.');
            return;
        }

        // Build receipt HTML using the current cart + discounts + totals from DOM
        let receiptHtml = `
        <div class="p-3">
            <div class="mb-2">
                <strong>Branch:</strong> ${$('#branchesList option:selected').text() || ''}
            </div>
            <div class="mb-2">
                <strong>Cashier:</strong> ${$('#cashier').text().replace("Cashier: ", "")}
            </div>
            <div class="mb-3">
                <strong>Customer:</strong> ${$('#customersList option:selected').text() || 'Anonymous'}
            </div>

            <hr/>
            <h6>Items</h6>
            <ul class="list-group mb-3">
    `;

        // Use the cart array to build item rows
        cart.forEach(item => {
            const priceUsed = (item.discountPrice !== undefined && item.discountPrice !== null) ? item.discountPrice : item.price;
            const lineTotal = priceUsed * item.quantity;

            // show price per unit (discounted price if available)
            let priceDisplay = '';
            if (priceUsed === item.price) {
                priceDisplay = fmtEGP(priceUsed);
            } else {
                priceDisplay = `<span class="text-muted small" style="text-decoration:line-through">${fmtEGP(item.price)}</span> <span class="fw-semibold">${fmtEGP(priceUsed)}</span>`;
            }

            receiptHtml += `
            <li class="list-group-item d-flex justify-content-between align-items-center">
                <div>
                  <div class="fw-semibold">${item.name}</div>
                  <div class="small text-muted">${priceDisplay} &nbsp; x ${item.quantity}</div>
                </div>
                <div class="fw-semibold">${fmtEGP(lineTotal)}</div>
            </li>
        `;
        });

        receiptHtml += `</ul>`;

        // Discounts (if any)
        if (discounts && discounts.length > 0) {
            receiptHtml += `<div class="mb-2"><strong>Applied Discounts</strong></div><ul class="list-group mb-3">`;
            discounts.forEach(d => {
                receiptHtml += `<li class="list-group-item d-flex justify-content-between">
                <div>${d.name}</div>
                <div class="text-success">${d.rate}%</div>
            </li>`;
            });
            receiptHtml += `</ul>`;
        }

        // Totals 
        const totalQtyText = $('#totalQty').text();
        const totalAmountText = $('#totalAmount').text();
        const totalDiscountRateText = $('#totalDiscountRate').text();
        const totalDiscountAmountText = $('#totalDiscountAmount').text();
        const grandTotalText = $('#grandTotal').text();
        const roundedTotalText = $('#roundedTotal').text();

        receiptHtml += `
        <div class="mb-2 d-flex justify-content-between">
            <span>Total Quantity</span><strong>${totalQtyText}</strong>
        </div>
        <div class="mb-2 d-flex justify-content-between">
            <span>Subtotal</span><strong>${totalAmountText} EGP</strong>
        </div>
        <div class="mb-2 d-flex justify-content-between">
            <span>Discounts (${totalDiscountRateText})</span><strong>${totalDiscountAmountText} EGP</strong>
        </div>
        <hr/>
        <div class="mb-2 d-flex justify-content-between">
            <span>Grand Total</span><strong>${grandTotalText}</strong>
        </div>
        <div class="mb-2 d-flex justify-content-between">
            <span>Rounded Total</span><strong>${roundedTotalText}</strong>
        </div>
        </div>
    `;

        // Insert receipt into #receiptArea only
        $('#receiptArea').html(receiptHtml);
        $('#checkoutModal').modal('show');
    });

    function buildInvoicePayload() {
        if (!currentBranchId) {
            toastr.error('Choose a branch');
            return null;
        }
        if (!currentUser || !currentUser.id) {
            toastr.error('User not found');
            return null;
        }
        if (cart.length === 0) {
            toastr.error('Cart is empty');
            return null;
        }

        return {
            branchId: currentBranchId,
            retailCustomerId: currentCustomerId,
            applicationUserId: currentUser.id,

            totalQuantity: parseInt($('#totalQty').text(), 10),
            totalAmount: parseFloat($('#totalAmount').text()),
            totalDiscountRate: parseInt($('#totalDiscountRate').text(), 10) || 0,
            totalDiscountAmount: parseFloat($('#totalDiscountAmount').text()) || 0,
            grandTotal: parseFloat($('#grandTotal').text()),
            roundedGrandTotal: parseInt($('#roundedTotal').text(), 10),

            generalDiscounts: discounts.map(d => d.id),

            operationItems: cart.map(c => ({
                itemId: c.id,
                quantity: c.quantity,
                sellingPrice: c.price,
                discountPrice: c.discountPrice,
                discountRate: c.discountRate || 0
            })),
        };
    }

    // Confirm Checkout
    $(document).on('click', '#confirmCheckoutBtn', function () {
        const paidCash = parseFloat($('#paidCashInput').val()) || 0;
        const roundedTotal = parseFloat($('#roundedTotal').text()) || 0;


        // Validate Paid Cash
        if (paidCash < roundedTotal) {
            toastr.error(`Paid cash must be at least ${roundedTotal.toFixed(2)} EGP`);
            return; // stop here, keep modal open
        }

        const change = paidCash - roundedTotal;

        // Build payload and add paidCash + change
        const payload = buildInvoicePayload();
        if (!payload) return;

        payload.paidCash = paidCash;
        payload.change = change;

        $.ajax({
            url: '/api/Sales/posapi/createSalesInvoice',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(payload),
            success: function (resp) {
                if (resp && resp.invoiceId) {
                    const pdfUrl = `/api/Sales/PosApi/receipt?operationId=${resp.invoiceId}`;
                    window.open(pdfUrl, '_blank');
                } else {
                    toastr.warning('Invoice created but PDF could not be opened automatically.');
                }

                toastr.success('Sales Invoice Created');
                cart = [];
                updateCartDisplay();
                $('#checkoutModal').modal('hide');
            },
            error: function (xhr) {
                const msg = xhr.responseJSON?.message || xhr.responseText || 'Unknown error';
                toastr.error(msg);
                console.error(msg);
            }
        });
    });

    // Paid Cash input handler
    $(document).on('input', '#paidCashInput', function () {
        const paidCash = parseFloat($(this).val()) || 0;
        const roundedTotal = parseFloat($('#roundedTotal').text()) || 0;

        if (paidCash < roundedTotal) {
            $('#changeAmount').text("0.00 EGP");
        } else {
            const change = paidCash - roundedTotal;
            $('#changeAmount').text(change.toFixed(2) + ' EGP');
        }
    });

    // Show Image Preview above the item card
    (function () {
        let hideTimer = null;
        const $preview = $('#imagePreview');
        const $previewImg = $('#previewImg');

        // 🔧 Adjustable vertical offset (how far above center the preview should appear)
        const verticalOffset = 0; // px → you can tweak this value

        function showPreview($card, imgSrc) {
            $previewImg.attr('src', imgSrc).show();

            const container = $('.pos-container');
            const containerOffset = container.offset();
            const containerWidth = container.outerWidth();

            const previewWidth = $preview.outerWidth();
            const previewHeight = $preview.outerHeight();

            // Center horizontally in the container
            let left = containerOffset.left + (containerWidth / 2) - (previewWidth / 2);

            // Position vertically: center of container minus offset
            let top = containerOffset.top + (container.outerHeight() / 2) - previewHeight - verticalOffset;

            // Keep inside viewport horizontally
            const winWidth = $(window).width();
            if (left < 8) left = 8;
            if (left + previewWidth > winWidth - 8) left = winWidth - previewWidth - 8;

            // Keep inside viewport vertically
            const scrollTop = $(window).scrollTop();
            const winHeight = $(window).height();
            if (top < scrollTop + 8) {
                top = scrollTop + 8;
            } else if (top + previewHeight > scrollTop + winHeight - 8) {
                top = scrollTop + winHeight - previewHeight - 8;
            }

            $preview.css({ top: top + 'px', left: left + 'px' })
                .stop(true, true).fadeIn(120);
        }

        function scheduleHide(delay = 100) {
            clearTimeout(hideTimer);
            hideTimer = setTimeout(() => $preview.fadeOut(100), delay);
        }
        function cancelHide() {
            clearTimeout(hideTimer);
        }

        // Hover handlers
        $(document).on('mouseenter', '.item-card', function () {
            cancelHide();
            const $card = $(this);
            const imgEl = $card.find('img.item-img').get(0);
            if (imgEl && imgEl.src) {
                showPreview($card, imgEl.src);
            }
        });

        $(document).on('mouseleave', '.item-card', function () {
            scheduleHide(150);
        });

        $preview.on('mouseenter', cancelHide)
            .on('mouseleave', () => scheduleHide(100));

        $(window).on('scroll resize', () => $preview.hide());
    })();

});