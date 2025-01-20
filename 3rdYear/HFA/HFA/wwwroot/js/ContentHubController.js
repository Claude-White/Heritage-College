function SetupContentRows() {
    var RESTResources = $('#RESTResources').data('resources');
    var AreRowsSummary = $('#AreRowsSummary').data('are-rows-summary') == 'True'; // Cursed

    // @todo: optimize this and refactor
    var notyf = new Notyf({
        duration: 5000,
        ripple: false,
        position: { x: 'right', y: 'bottom' },
        dismissible: true,
        types: [
            {
                type: 'success',
                duration: 3000,
                dismissible: false,
            },
            {
                type: 'error',
                position: { x: 'right', y: 'bottom' },
                duration: 10000,
            }
        ]
    });

    $("textarea.textareaResize").each(function () {
        var currentText = $(this).text()
        var trimmedText = currentText.trim();
        $(this).text(trimmedText)
        let numberOfMembers = trimmedText.split('\n').length;
        $(this).css("height", numberOfMembers * 25 + "px");
    })

    function displayToastError(action, error, xhr) {
        var errorMessage = `<b>Could not ${action} this entry!</b><br>`;

        var errorData = xhr.responseJSON;
        if (errorData) {
            if (typeof errorData.detail === 'string') {
                errorMessage += '· ' + errorData.detail;
            } else {
                errorMessage += Object.keys(errorData).map(function (key) {
                    var messages = errorData[key];
                    if (Array.isArray(messages)) {
                        var messagesWithDots = messages.map(function (message) {
                            return '· ' + message;
                        });
                        return messagesWithDots.join('<br>');
                    } else {
                        return '· ' + messages;
                    }
                }).join('<br>');
            }
        } else {
            errorMessage += `(${error})`;
        }

        notyf.error(errorMessage);
    }

    function getRowDataAsJson($row) {
        var rowData = {};

        $row.find('[data-index]').each(function () {
            var key = $(this).attr('data-index');
            var value = $(this).val();

            rowData[key] = value;
        });

        return rowData;
    }

    function getRowDataId($row) {
        var id = 0;

        $row.find('[data-index]').each(function () {
            if ($(this).attr('isId'))
                id = $(this).val();
        });

        return id;
    }

    function showEditActions($row) {
        $row.find('.action-confirm').show();
        $row.find('.action-cancel').show();
        $row.find('.action-delete').hide();
        $row.find('.action-update').hide();
    }

    function hideEditActions($row) {
        $row.find('.action-confirm').hide();
        $row.find('.action-cancel').hide();
    }

    // Cache original input values
    $('tr.content-row :input').each(function () {
        $(this).data('original-value', $(this).val());
    });

    // Show/hide actions on hover
    $(document).on("mouseenter", 'tr.content-row', function () {
/*        $(this).find('.action-move').show();
*/        if ($(this).data('is-editing') != true) {
            $(this).find('.action-delete').show();
            $(this).find('.action-update').show();
        }
    }).on("mouseleave", 'tr.content-row', function () {
/*        $(this).find('.action-move').hide();
*/        $(this).find('.action-delete').hide();
          $(this).find('.action-update').hide();
    })

    // Handle displaying of actions
    $('tr.content-row :input').on('input', function () {
        var $row = $(this).closest('tr.content-row');

        if ($row.data('is-new') != true) {
            var isChanged = false;
            $row.find(':input').each(function () {
                if ($(this).val() !== $(this).data('original-value')) {
                    isChanged = true;
                    return false;
                }
            });

            $row.data('is-editing', isChanged);

            if (isChanged) {
                showEditActions($row);
            } else {
                hideEditActions($row);
            }
        }
    });

    // Handle 'Confirm' action
    $('.action-confirm').on('click', function () {
        var $row = $(this).closest('tr.content-row');
        var $idInput = $row.find('input[isId="true"]');

        // Hide action container
        hideEditActions($row);

        // Send the request
        if ($row.data('is-new') == true) {
            var data = getRowDataAsJson($row)
            var dataIndex = $idInput.attr('data-index')
            data[dataIndex] = 0 // The database will set the Id

            // Create
            $.ajax({
                url: RESTResources.post,
                type: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                data: JSON.stringify(data),
                success: function (NewId) {
                    $row.data('is-new', false);

                    $row.find(`input[data-index="${dataIndex}"]`).val(NewId);
                    notyf.success("Entry created successfully!");

                    $row.find(':input').each(function () {
                        $(this).data('original-value', $(this).val());
                    });

                    $row.data('is-editing', false);
                    $row.find('.action-delete').show();
                    $row.find('.action-update').show();
                },
                error: function (xhr, status, error) {
                    showEditActions($row);
                    displayToastError("create", error, xhr)
                }
            });

        } else {
            // Update
            $.ajax({
                url: RESTResources.put,
                type: 'PUT',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                data: JSON.stringify(getRowDataAsJson($row)),
                success: function (response) {
                    notyf.success("Entry updated successfully!");

                    $row.find(':input').each(function () {
                        $(this).data('original-value', $(this).val());
                    });

                    $row.data('is-editing', false);
                    $row.find('.action-delete').show();
                    $row.find('.action-update').show();
                },
                error: function (xhr, status, error) {
                    showEditActions($row);
                    displayToastError("update", error, xhr);
                }
            });

        }
    });
    // Handle 'Update' action
    $("#action-update").on('click', function () {
        if (AreRowsSummary == true) {
            // Send request
            window.location = RESTResources.put;
        }
    });

    // Handle 'Create' action
    $('#action-create').on('click', function () {
        if (AreRowsSummary == true) {
            // Send request
            window.location = RESTResources.post;
        } else {
            // Check if the user already created an entry
            var newEntries = $('tr').filter(function () {
                return $(this).data('is-new') === true;
            });
            if (newEntries.length > 0) { return };

            var $section = $(this).closest('section');
            var $row = $section.find('#EntryTemplate').first().clone(true);
            $row.removeAttr('id');

            // Reset all the data values
            $row.find('*[data-index]').val('').data('original-value', '');

            // Prepend to table
            $section.find('tbody').prepend($row);
            $row.show();

            showEditActions($row);

            $row.data('is-editing', true);
            $row.data('is-new', true);
        }
    });

    // Handle 'Cancel' action
    $('.action-cancel').on('click', function () {
        var $row = $(this).closest('tr.content-row');
        if ($row.data('is-new') == true) {
            $row.remove();
        } else {
            $row.find(':input').each(function () {
                var originalValue = $(this).data('original-value');
                $(this).val(originalValue);
            });

            hideEditActions($row);
            $row.find('.action-delete').show();
            $row.find('.action-update').show();

            $row.data('is-editing', false);
        }
    });

    // Handle 'Delete' action
    var $currentlyFocusedRow
    $('.action-delete').on('click', function () {
        $currentlyFocusedRow = $(this).closest('tr.content-row');
        $('#confirm-delete-modal').get(0).showModal();
    });

    // Handle confirm delete modal
    $('#confirm-delete-button').on('click', function () {
        // Send the request
        $.ajax({
            url: RESTResources.delete + "/" + getRowDataId($currentlyFocusedRow),
            type: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json; charset=utf-8'
            },
            success: function (response) {
                $currentlyFocusedRow.remove();
                notyf.success("Entry deleted successfully!");
                $currentlyFocusedRow.remove();
            },
            error: function (xhr, status, error) {
                displayToastError("delete", error, xhr);
            }
        });
    });

    // Image Edit Modal Object (this is horrible, wished we wouldve went with vue or react)
    function getEditImageModal() {
        var $modal = $('#edit-image-modal');
        var $preview = $('#edit-image-preview');
        var $rotateImageButton = $("#rotate-image-button")
        var $flipXImageButton = $("#flip-x-image-button")
        var $flipYImageButton = $("#flip-y-image-button")
        var $confirmButton = $("#confirm-edit-image-button")
        var $cancelButton = $("#cancel-edit-image-button")

        var $input
        var cachedImageSelectedCallback

        var cropper = $preview.cropper({
            aspectRatio: 16 / 9,
            viewMode: 1,
            minCanvasWidth: 50,
            dragMode: 'move',
            cropBoxMovable: false,
            cropBoxResizable: false,
            center: false,
            autoCropArea: 1
        });

        function close() {
            $input.val(null);
            $input = null
        }

        $confirmButton.on('click', function () {
            // Get Image File
            var file = $input.get(0).files[0];
            var imageEditData = JSON.stringify(cropper.cropper('getData'))

            // Build Form Data
            var formData = new FormData();
            formData.append('file', file); 
            formData.append('imageEditData', imageEditData); 

            // Call Create Image API
            $.ajax({
                url: "/ContentHub/Images/CreateImage",
                type: 'POST',
                processData: false,
                contentType: false,
                headers: {
                    'Accept': 'application/json'
                },
                data: formData,
                success: function (image) {
                    notyf.success("Image created successfully!");
                    cachedImageSelectedCallback(image);
                },
                error: function (xhr, status, error) {
                    displayToastError("create", error, xhr);
                    close()
                }
            });
        })

        $cancelButton.on('click', close)

        $rotateImageButton.on('click', function (e) {
            e.preventDefault();
            cropper.cropper('rotate', 90);
        })

        var currentScaleX = 1
        $flipXImageButton.on('click', function (e) {
            e.preventDefault();
            if (currentScaleX == 1) {
                currentScaleX = -1
            } else {
                currentScaleX = 1
            }
            cropper.cropper('scaleX', currentScaleX);
        })

        var currentScaleY = 1
        $flipYImageButton.on('click', function (e) {
            e.preventDefault();
            if (currentScaleY == 1) {
                currentScaleY = -1
            } else {
                currentScaleY = 1
            }
            cropper.cropper('scaleY', currentScaleY);
        })

        function show($forInput, imgUrl, imageSelectedCallback) {
            $input = $forInput;
            cachedImageSelectedCallback = imageSelectedCallback
            $preview.cropper('replace', imgUrl, false);
            $modal.get(0).showModal()
        }

        return {
            "show" : show
        }
    }

    var imageModalObject = getEditImageModal()

    // Handle image upload
    $('.image-upload-container').each(function () {
        var $inputContainer = $(this).find('.image-upload-input-container:first');
        var $previewContainer = $(this).find('.image-upload-preview-container:first');
        var $previewImage = $(this).find('.image-upload-preview-image:first');
        var $input = $(this).find('.image-upload-input:first');
        var $imageIdInput = $(this).find('.image-id-input:first');
        var $clearImageButton = $(this).find('.clear-image-button:first')

        var imageSelectedCallback = function (image) {
            $input.prop('disabled', true)
            $previewImage.attr('src', image.path)
            $imageIdInput.val(image.imageId)
            $inputContainer.hide()
            $previewContainer.show()
        }

        var clearImageSelection = function () {
            $input.prop('disabled', false)
            $previewImage.attr('src', '')
            $input.val(null)
            $imageIdInput.val(null)
            $inputContainer.show()
            $previewContainer.hide()
        }

        $input.on("change", function () {
            const image = this.files[0];

            if (image) {
                const reader = new FileReader();
                reader.onload = () => {
                    const imgUrl = reader.result;
                    imageModalObject.show($input, imgUrl, imageSelectedCallback)
                };
                reader.readAsDataURL(image);
            }
        });

        var currentImageId = $imageIdInput.val();
        if (currentImageId != null && currentImageId != "") {
            $.ajax({
                url: "/ContentHub/Images/GetImage",
                type: 'GET',
                data: { imageId: currentImageId },
                success: function (image) {
                    imageSelectedCallback(image);
                },
                error: function (xhr, status, error) {
                    console.log(xhr)
                }
            });
        }

        $clearImageButton.on('click', clearImageSelection)
    });
}

$(function () {
    SetupContentRows();
});